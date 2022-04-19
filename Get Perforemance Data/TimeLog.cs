
using System.Collections;
using System.Collections.Generic;
using ARC;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System;


public class TimeLog : MonoBehaviour
{
    [SerializeField] private Text createTime, moveTime, tiCSV;
    [SerializeField] private InputField userNameInputField;

    private string userName;

    public Stopwatch timeLog = new Stopwatch();

    public static List<string[]> timeCSV = new List<string[]>();
    public static int timeLogIDIndex = 0;

    ARCUser user;
    GameObject create, move, throwObj, username;

    void Start()
    {
        // Get the user
        user = GetComponent<ARCUser>();

        //Initiate stopwatch for time log
        timeLog.Start();

         //Get the User name
         //user.CmdSetUsername(FindObjectOfType<InputField>().text);

         // Get the button
         create = GameObject.Find("ButtonCreate");
         move = GameObject.Find("ButtonMove");
         username = GameObject.Find("InputFieldName");

        create.GetComponent<Button>().onClick.AddListener(CreateTimeLog);
         move.GetComponent<Button>().onClick.AddListener(MoveTimeLog);
        
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnValueChangedEvent()
    {
        userName = userNameInputField.text;
    }

        void CreateTimeLog()
    {
        AddRowTimeLog("Create Button");
        //test
        createTime.text = timeLog.ElapsedMilliseconds.ToString();

        tiCSV.text = "|";
        for (int i = 0; i < 4; i++)
        {
            tiCSV.text = tiCSV.text + "  " + timeCSV[timeCSV.Count - 1][i];
        }
    }

    void MoveTimeLog()
    {
        AddRowTimeLog("Move Button");
        //test
        moveTime.text = timeLog.ElapsedMilliseconds.ToString();

        tiCSV.text = "|";
        for (int i = 0; i < 4; i++)
        {
            tiCSV.text = tiCSV.text + "  " + timeCSV[timeCSV.Count - 1][i];
        }
    }

    void AddRowTimeLog(String buttonType) {
        //when button is clicked, add new row
        string[] writeRowTemp = new string[4];

        timeLogIDIndex++;
        writeRowTemp[0] = timeLogIDIndex.ToString(); //add log ID count
        writeRowTemp[1] = userName; //add playerID, need to fix later
        writeRowTemp[2] = buttonType; //add buttonType
        writeRowTemp[3] = timeLog.ElapsedMilliseconds.ToString(); //add timeMS
        timeCSV.Add(writeRowTemp); //add new Row
       
    }
        

}
