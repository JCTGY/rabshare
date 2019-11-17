using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{

    [DllImport("__Internal")] // upload the file as a string to javascript function (data is string of the data,
                              // filename is the name of the file save as (ddMMhhmmssffffff)
    private static extern string getUserIDString();

    public List<string> menuItems;
    public GameObject buttonPrefab;
    public static string trajectoryPath;
    public GameObject menuContent;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadManager.fileNames.Count != 0);
        LoadMenuItems();
        foreach (string item in menuItems)
        {
            GameObject button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, menuContent.transform);
            button.GetComponentInChildren<Text>().text = item;
            button.GetComponent<Button>().onClick.AddListener(() => updatePathString(item));
        }
    }

    /// <summary>
    /// Set the filename to the one that player select
    /// static string tragectoryPath will be use to request the download link
    /// </summary>
    /// <param name="path"> string of the filename that the player click</param>
    public void updatePathString(string path)
    {
        trajectoryPath = path;
    }

    /// <summary>
    /// Only display the fileNames of the current UserID
    /// can change to all by removing the if statement
    /// </summary>
    public void LoadMenuItems()
    {
        string userID = getUserIDString();
        menuItems.Clear();
        foreach (var path in LoadManager.fileNames)
        {
            if (path.Contains(userID))
            {
                string[] parts = path.Split(' ');
                menuItems.Add(parts[parts.Length - 1]);
            }
        }
    }
}