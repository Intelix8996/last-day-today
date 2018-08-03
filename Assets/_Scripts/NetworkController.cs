using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : MonoBehaviour {

    NetworkManager Manager;

    string[] args;
    string arg;

    void Start () {
        Manager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();

        if (Application.isEditor)
            Manager.StartHost();

        args = System.Environment.GetCommandLineArgs();

        foreach (string arg in args)
        {
            this.arg += " " + arg + ",";
        }

        Debug.Log("Starting with arguments: [" + arg + "]");

        ParseArguments();
    }

    void ParseArguments()
    {
        for (int i = 0; i < args.Length; ++i)
        {
            switch (args[i])
            {
                case "-server":
                    Debug.Log("Startng server...");
                    Manager.StartServer();
                    Debug.Log("Server started");
                    break;
                case "-host":
                    Debug.Log("Startng host...");
                    Manager.StartHost();
                    Debug.Log("Host started");
                    break;
                case "-client":
                    Debug.Log("Startng client...");
                    Manager.StartClient();
                    Debug.Log("Client started");
                    break;
                case "-ip":
                    Manager.networkAddress = args[i + 1];
                    Debug.Log("IP set to " + args[i + 1]);
                    break;
                case "-port":
                    Manager.networkPort = Convert.ToInt32(args[i + 1]);
                    Debug.Log("Port set to " + args[i + 1]);
                    break;
            }
        }
    }
}