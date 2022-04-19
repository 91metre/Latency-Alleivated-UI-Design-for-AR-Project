using ARC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PositionLog : MonoBehaviour
{
    [SerializeField]
    private Text releaseX, releaseY, releaseZ,
                 groundX, groundY, groundZ, posCSV,
                 prefabX, prefabY, prefabZ;

    public static List<GameObject> physicsCubeList = new List<GameObject>();

    public static List<string[]> positionCSV = new List<string[]>();

    [SerializeField] private InputField userNameInputField;

    private string userName;

    private string[] writeRowTemp1 = new string[5];
    private string[] writeRowTemp2 = new string[3];

    public static int PositionLogIDIndex = 0;

    ARCUser user;
    GameObject move;

    private GameObject GO;
    private Vector3 GOPos;
    public bool isGOEmpty = true;

    // var for check Obj is moved
    private Vector3 priorFrameTransform;
    private bool groundPosWritten = false;
    private int positionBufferLength = 2;
    private int framecnt = 0;
    private int positionBufferLengthID;
    private int counter = 0;
    private bool isBeingMoved = true;

    private Rigidbody GORigidbody;

    void Start()
    {
        // Get the user
        user = GetComponent<ARCUser>();

        //Get the User name
        //user.CmdSetUsername(FindObjectOfType<InputField>().text);
        
        // Get the button
        move = GameObject.Find("ButtonMove");
        move.GetComponent<Button>().onClick.AddListener(MovePositionLog);

        //userName.text = user.username;

    }   

    // Update is called once per frame
    void Update()
    {
        // Display prefab position
        prefabX.text = GOPos.x.ToString();
        prefabY.text = GOPos.y.ToString();
        prefabZ.text = GOPos.z.ToString();

        if (!isGOEmpty)
        {
            GroundPositionLog();
        }
    }

    public void OnValueChangedEvent()
    {
        userName = userNameInputField.text;
    }

    public static void AddPrefabToList(GameObject go)
    {
        physicsCubeList.Add(go);
    }

    void MovePositionLog()
    {

        //get the position of last object in the list
        GO = physicsCubeList[physicsCubeList.Count - 1];
        GOPos = GO.gameObject.transform.position;

        PositionLogIDIndex++;
        groundPosWritten = false;

        writeRowTemp1[0] = PositionLogIDIndex.ToString();
        writeRowTemp1[1] = userName;
        writeRowTemp1[2] = GOPos.x.ToString();
        writeRowTemp1[3] = GOPos.y.ToString();
        writeRowTemp1[4] = GOPos.z.ToString();
        
        isGOEmpty = false;

        //test
        releaseX.text = GOPos.x.ToString();
        releaseY.text = GOPos.y.ToString();
        releaseZ.text = GOPos.z.ToString();

        
    }

    void GroundPositionLog()
    {
        GO = physicsCubeList[physicsCubeList.Count - 1];
        GOPos = GO.gameObject.transform.position;
        GORigidbody = GO.gameObject.GetComponent<Rigidbody>();

        if (GORigidbody.IsSleeping())
        {
            isBeingMoved = false;
        }
        else
        {
            isBeingMoved = true;
        }
        

        if (!isBeingMoved && !groundPosWritten)
        {
            string[] writeRowTemp = new string[8];

            writeRowTemp2[0] = GOPos.x.ToString();
            writeRowTemp2[1] = GOPos.y.ToString();
            writeRowTemp2[2] = GOPos.z.ToString();

            for (int i = 0; i < 5; i++)
            {
                writeRowTemp[i] = writeRowTemp1[i];
            }
            for (int i = 0; i < 3; i++)
            {
                writeRowTemp[i+5] = writeRowTemp2[i];
            }
            positionCSV.Add(writeRowTemp); //add new row

            groundPosWritten = true;

            //test
            groundX.text = GOPos.x.ToString();
            groundY.text = GOPos.y.ToString();
            groundZ.text = GOPos.z.ToString();

            posCSV.text = "|";
            for (int i = 0; i < 8; i++)
            {
                posCSV.text = posCSV.text +" "+ positionCSV[positionCSV.Count - 1][i];
            }
        }
    }
}
