using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.IO;
using System;

public class Authentication : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void firebaseInit();
    [DllImport("__Internal")]
    private static extern void authenticationSignIn();
    [DllImport("__Internal")]
    private static extern bool verifyAuthentication();
    [DllImport("__Internal")]
    private static extern void createAuthenticationUI();
    [DllImport("__Internal")]
    private static extern void setUIDiv();
    [DllImport("__Internal")]
    private static extern void getBucketFiles();

    public Canvas titleScreen;

    private void Awake()
    {
        //if (verifyAuthentication())
        //    SceneManager.LoadScene("Robotic Arm Landing Scene");
    }

    // Start is called before the first frame update
    void Start()
    {
        firebaseInit();
        setUIDiv();
        createAuthenticationUI();
    }

    private void Update()
    {
        //if (verifyAuthentication())
        //    SceneManager.LoadScene("Robotic Arm Landing Scene");
    }

    public void LoadLandingScene()
    {
        Debug.Log("Delete Scene Message Received");
        titleScreen.gameObject.SetActive(true);
    }
}
