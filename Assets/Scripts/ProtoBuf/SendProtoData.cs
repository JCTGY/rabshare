using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.IO;
using System;
using ProtoBuf;

public class SendProtoData : MonoBehaviour
{
    // Start is called before the first frame update

    [DllImport("__Internal")] // upload the file as a string to javascript function (data is string of the data,
                              // filename is the name of the file save as (ddMMhhmmssffffff)
    private static extern void uploadFile(string data, string filename);

    [DllImport("__Internal")] // upload the file as a string to javascript function (data is string of the data,
                              // filename is the name of the file save as (ddMMhhmmssffffff)
    private static extern void firebaseInit();

    [DllImport("__Internal")] // upload the file as a string to javascript function (data is string of the data,
                              // filename is the name of the file save as (ddMMhhmmssffffff)
    private static extern string getUserIDString();

    InputField nameTextFiled;
    InputField feedBackTextTextFiled;

    public GameObject QuitButton;
    public GameObject RestartButton;
    public GameObject Feedback;
    public GameObject Name;
    public GameObject Send;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Call javascript function uploasFile
    /// Send the deserialize protobuf gamedetail object
    /// filename is User ID + Timestamp
    /// </summary>
    public void SendButton()
    {
        nameTextFiled = Name.GetComponent<InputField>();
        feedBackTextTextFiled = Feedback.GetComponent<InputField>();

        ProtoManager.gameDetail.Name = nameTextFiled.text;
        ProtoManager.gameDetail.FeedBack = feedBackTextTextFiled.text;
        ProtoManager.gameDetail.FinalScore = GameMaster.CurrentScore;
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, ProtoManager.gameDetail);
                string stringBase64 = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                //firebaseInit();
                string userName = getUserIDString();
                Debug.Log("Username in Unity: " + userName);
                string fileName = userName + " " + DateTime.Now.ToString("MMddhhmmssffffff");

                Debug.Log("Filename in Unity: " + fileName);
                uploadFile(stringBase64, fileName);
            }

        }

        SetupResetButton();
    }

    void SetupResetButton()
    {
        Feedback.SetActive(false);
        Name.SetActive(false);
        Send.SetActive(false);
        QuitButton.SetActive(true);
        RestartButton.SetActive(true);
    }


}
