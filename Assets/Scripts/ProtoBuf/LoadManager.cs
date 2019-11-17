using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.IO;
using System;
using UnityEngine.Networking;
using System.Text;
using ProtoBuf;

public class LoadManager : MonoBehaviour
{
    /// <summary>
    /// List<string> fileName: save all the filename in the cloud bucket </string>
    /// </summary>
    public static List<string> fileNames { get; set; } = new List<string>();

    [DllImport("__Internal")]
    private static extern void getProtoData(string fileName);
    [DllImport("__Internal")]
    private static extern bool verifyAuthentication();


    private string protoUrl;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// javascript function:  getBucketFiles will call this function
    /// load file name to List<string>fileName </string>
    /// </summary>
    /// <param name="loadNames"></param>
    public void LoadFilePaths(string loadNames)
    {
        string path = loadNames;
        fileNames.Add(path);
    }

    /// <summary>
    /// display the filename in the Replay Scenen as table view
    /// filename iis only under the userID
    /// </summary>
    public void LoadProtoDatas()
    {
        string fullFileName = " display ";
        foreach (String fullName in fileNames)
        {
            if (fullName.Contains(ButtonMenu.trajectoryPath))
                fullFileName = fullName;
        }
        Debug.Log("LoadProtoData" + fullFileName);
        getProtoData(fullFileName);
    }

    /// <summary>
    /// javascript function: getProtoData() => return download url
    /// Send to GetData() for www request
    /// </summary>
    /// <param name="JSprotoUrl"></param>
    public void LoadProtoUrl(string JSprotoUrl)
    {
       //yield return new WaitUntil(() => (getProtoData()).Length != 0);
       protoUrl = JSprotoUrl;
       StartCoroutine(GetData());
    }

    /// <summary>
    /// Download the data form : protourl
    /// Base64 decoded and deserialized the data to ProtoGameDetail class
    /// Then, load the Build Scene for replay 
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(protoUrl);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            string b64ProtoString = Encoding.UTF8.GetString(results, 0, results.Length);
            Stream stream = new MemoryStream(Convert.FromBase64String(b64ProtoString));
            LoadProtoData.loadGameDetail = Serializer.Deserialize<ProtoGameDetail>(stream);
            Debug.Log(LoadProtoData.loadGameDetail.FeedBack);
            Debug.Log("count of the position: " + LoadProtoData.loadGameDetail.ClawBodyPosition.Count);
            LoadBuildScene();
        }
    }

    /// <summary>
    /// Load build Scene and set isReplay to true
    /// </summary>
    void LoadBuildScene()
    {
        GameMaster.isReplay = true;
        SceneManager.LoadScene("_Scenes/Robotic Arm Build Scene");
    }
}
