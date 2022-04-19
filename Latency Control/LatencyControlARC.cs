using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class LatencyControlARC : MonoBehaviour
{
    Camera mCamera;
    Vector3[] initialpos;
    Quaternion[] initialrot;
    int[] frametime;
    int bufferlength;
    int framecnt;
    Vector3 lastId;
    Quaternion lastrot;
    //public ARCameraManager arCameraManager;

    public static int resWidth = 1920;
    public static int resHeight = 1080;
    
    private RenderTexture renderTexture;
    private RenderTexture targetTexture;
    private RenderTexture[] renderTextureBuf;
    private RawImage rawImage;

    public int offset_tracking = 0;
    public int offset_rendering = 0;

    private int index_tracking;
    private int index_rendering;

    private DeviceOrientation orientation;

    //ARCameraBackground m_CameraBackground;


    // Start is called before the first frame update
    void Start()
    {
        // Get the ARCamera
        mCamera = GameObject.Find("ARCamera").GetComponent<Camera>();

        // Frame count and times are defined here
        framecnt = 0;
        bufferlength = 60;  //Mathf.Max(offset_tracking,offset_rendering)+5;
        frametime = new int[bufferlength];

        // Define the buffer for Tracking delays
        initialpos = new Vector3[bufferlength];
        initialrot = new Quaternion[bufferlength];

        // Define the objects for the Display delay
        renderTexture = new RenderTexture(resWidth, resHeight, 32);
        targetTexture = new RenderTexture(resWidth, resHeight, 32);
        
        renderTextureBuf = new RenderTexture[bufferlength];
        for (int i = 0; i < bufferlength; i++)
        {
            renderTextureBuf[i] = new RenderTexture(resWidth, resHeight, 32);
            //Graphics.Blit(null, renderTextureBuf[i], m_CameraBackground.material);
        }
        
        rawImage = GameObject.Find("RawImage").GetComponent<RawImage>();
        rawImage.texture = targetTexture;

        mCamera.targetTexture = renderTextureBuf[0];//renderTexture;

        //offset_tracking = FindObjectOfType<LatencyInput>().trackingLatency;
        //offset_rendering = FindObjectOfType<LatencyInput>().displayLatency;

        Application.targetFrameRate = 60;

    }

    // Update is called once per frame
    void Update()
    {
        
        // Set Frame number, corresponding index in our circular buffers and store the frame time in ms
        framecnt++;
        int id = framecnt % bufferlength;
        frametime[id] = (int)(Time.realtimeSinceStartup * 1000);

        
        if (offset_tracking > 0) //offset_tracking = tracking Latency
        {
            // Store current position and rotation of the camera in the circular buffer
            initialpos[id] = mCamera.transform.position;
            initialrot[id] = mCamera.transform.rotation;

            while (frametime[id] - frametime[index_tracking] > offset_tracking)
            {
                index_tracking = (index_tracking + 1) % bufferlength;
            }


            // If a frame was found, set the camera position and rotation to the values
            //if (frametime[id] - frametime[index_tracking] > offset_tracking)
            //{
            mCamera.transform.position = initialpos[index_tracking];
            mCamera.transform.rotation = initialrot[index_tracking];
            //Debug.Log("Initialpos" + initialpos[index_tracking]);
            //Debug.Log("Frame nr" + frametime[index_tracking]);
            //Debug.Log("Index Success " + index_tracking + " - " + id);
            //}
            
        }
        
        if (offset_rendering > 0) //offset_tracking = rendering(display) Latency
        {

            mCamera.targetTexture = renderTextureBuf[id];

            while (frametime[id] - frametime[index_rendering] > offset_rendering)
            {
                index_rendering = (index_rendering + 1) % bufferlength;
            }
   
            rawImage.texture = renderTextureBuf[index_rendering];
            
        }
        else
        {
            mCamera.targetTexture = renderTexture;
            rawImage.texture = renderTexture;
        }
    }
}
