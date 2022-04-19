using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using ARC;
using debug = UnityEngine.Debug;

public class StartEndControl : MonoBehaviour
{
    ARCUser user;
    private GameObject start, end, create, move;

    private int fileCounter = 0;

    [SerializeField] private Text countDown;
    private float countDownSetTime = 5.0f;
    private bool countDownOn = false;

    PositionLog isGOEmpty = new PositionLog();

    // List for test...
    public List<string[]> testCSV = new List<string[]>();

    void Start()
    {
        user = GetComponent<ARCUser>();

        //gameObject.GetComponent<TimeLog>().enabled = false;
        //gameObject.GetComponent<PositionLog>().enabled = false;

        // Get the button
        start = GameObject.Find("ButtonStart");
        end = GameObject.Find("ButtonEnd");
        create = GameObject.Find("ButtonCreate");
        move = GameObject.Find("ButtonMove");

        start.GetComponent<Button>().onClick.AddListener(StartExperiment);
        end.GetComponent<Button>().onClick.AddListener(EndExperiment);
        create.GetComponent<Button>().onClick.AddListener(CountDown);
        move.GetComponent<Button>().onClick.AddListener(DisableCountDown);
        

        end.SetActive(false);
        start.SetActive(true);

        countDown.gameObject.SetActive(false);

        /*
        //List for test...
        string[] testRowCSV = new string[4];
        

        testRowCSV[0] = "Log ID";
        testRowCSV[1] = "Player ID";
        testRowCSV[2] = "Button Type";
        testRowCSV[3] = "Milli-Second";
        testCSV.Add(testRowCSV);
        
        testRowCSV[0] = "1";
        testRowCSV[1] = "1";
        testRowCSV[2] = "1";
        testRowCSV[3] = "1";
        
        string[] testRowCSV2 = new string[4] { "1", "1", "1", "1" };
        testCSV.Add(testRowCSV2);
        
        testRowCSV[0] = "2";
        testRowCSV[1] = "2";
        testRowCSV[2] = "2";
        testRowCSV[3] = "2";
        testCSV.Add(testRowCSV);
        
        testRowCSV[0] = "3";
        testRowCSV[1] = "3";
        testRowCSV[2] = "3";
        testRowCSV[3] = "3";
        testCSV.Add(testRowCSV);
        
        Debug.Log(testCSV[0][0]);
        Debug.Log(testCSV[1][0]);


        /*
        for ( int i=0; i < testCSV.Count; i++)
        {
            
        }
        */
        // for test...
        //WriteCSV(testCSV, true);

    }

    void Update()
    {
        if (countDownOn) // countdown
        {
            if (countDownSetTime > 0)
            {
                countDownSetTime -= Time.deltaTime;
            }
            else if (countDownSetTime <= 0)
            {
                countDownSetTime = 0.0f;
            }
            countDown.text = Mathf.Round(countDownSetTime).ToString();
        }
    }

    void StartExperiment()
    {
        //enable logging
        //gameObject.GetComponent<TimeLog>().enabled = true;
        //gameObject.GetComponent<PositionLog>().enabled = true;

        //clear the list file
        TimeLog.timeCSV.Clear();
        PositionLog.positionCSV.Clear();

        // Set the time Log file label
        string[] firstRowTimeCSV = new string[4];
        firstRowTimeCSV[0] = "Log ID";
        firstRowTimeCSV[1] = "Player ID";
        firstRowTimeCSV[2] = "Button Type";
        firstRowTimeCSV[3] = "Milli-Second";
        TimeLog.timeCSV.Add(firstRowTimeCSV);

        TimeLog.timeLogIDIndex = 0;

        // Set the position Log file label
        string[] firstRowPositionCSV = new string[8];
        firstRowPositionCSV[0] = "Log ID";
        firstRowPositionCSV[1] = "Player ID";
        firstRowPositionCSV[2] = "X_PositionReleased";
        firstRowPositionCSV[3] = "Y_PositionReleased";
        firstRowPositionCSV[4] = "Z_PositionReleased";
        firstRowPositionCSV[5] = "X_PositionOnGround";
        firstRowPositionCSV[6] = "Y_PositionOnGround";
        firstRowPositionCSV[7] = "Z_PositionOnGround";
        PositionLog.positionCSV.Add(firstRowPositionCSV);

        PositionLog.PositionLogIDIndex = 0;

        //clear the physics CubeList at the start of the experiment
        PositionLog.physicsCubeList.Clear();
        isGOEmpty.isGOEmpty = true;

        start.SetActive(false);
        end.SetActive(true);

        
    }

    void EndExperiment()
    {
        //write on CSV 
        WriteCSV(TimeLog.timeCSV, true);
        WriteCSV(PositionLog.positionCSV, false);

        isGOEmpty.isGOEmpty = true;

        //disable logging
        //gameObject.GetComponent<TimeLog>().enabled = false;
        //gameObject.GetComponent<PositionLog>().enabled = false;

        end.SetActive(false);
        start.SetActive(true);
    }

    void CountDown()
    {
        countDown.gameObject.SetActive(true);

        //countdown set
        countDownSetTime = 5.0f;
        countDown.text = countDownSetTime.ToString();

        countDownOn = true; //initiate countdown
    }

    void DisableCountDown()
    {
        //disable countDown when user click move button
        countDownOn = false;
        countDown.gameObject.SetActive(false);
    }


    void WriteCSV(List<string[]> fileCSV, bool isItTimeLog)
    {
        //writing...
        string[][] output = new string[fileCSV.Count][];

        //output = fileCSV.ToArray();

        for (int i = 0; i < output.Length; i++) { 
            output[i] = fileCSV[i];

        }
        

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        string filePath = GetPath(isItTimeLog);
        
        Stream fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);
        StreamWriter outStream = new StreamWriter(fileStream, Encoding.UTF8);
        outStream.WriteLine(sb);
        outStream.Close();
        
        /*
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
        */
    }

    private string GetPath(bool isItTimeLog)
    {
        

        if (isItTimeLog)
        {
            fileCounter++;
            // for test
            //return Application.dataPath + "/Examples/"+ "/ARCSandbox/" + "Time Log.csv";

            return Application.persistentDataPath + "Time Log" + fileCounter.ToString() + ".csv";
        }

        else
        {
            return Application.persistentDataPath + "Position Log" + fileCounter.ToString() + ".csv";
        }

        //" + fileCounter.ToString() + "
        /*
        #if UNITY_ANDROID
        #elif UNITY_EDITOR
        return Application.dataPath + "/CSV/" + "Time Log" + counter.ToString() + ".csv";
        #elif UNITY_IPHONE
        return Application.persistentDataPath + "/" + "Time Log" + counter.ToString() + ".csv";
        #else
        return Application.dataPath + "/" +"Time Log" + counter.ToString() + ".csv";
        #endif
        */

    }

}
