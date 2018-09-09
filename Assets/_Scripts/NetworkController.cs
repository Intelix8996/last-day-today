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

        args = System.Environment.GetCommandLineArgs();

        foreach (string arg in args)
        {
            this.arg += " " + arg + ",";
        }

        Debug.Log("Starting with arguments: [" + arg + "]");

        ParseArguments();

        if (Application.isEditor)
            Manager.StartHost();
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
                    break;
                case "-host":
                    Debug.Log("Startng host...");
                    Manager.StartHost();
                    break;
                case "-client":
                    Debug.Log("Startng client...");
                    Manager.StartClient();
                    break;
                case "-ip":
                    Manager.networkAddress = args[i + 1];
                    Debug.Log("IP set to " + args[i + 1]);
                    break;
                case "-port":
                    Manager.networkPort = Convert.ToInt32(args[i + 1]);
                    Debug.Log("Port set to " + args[i + 1]);
                    break;
                case "-map.size":
                    GameObject.Find("TerrainManager").GetComponent<TerrainManager>().Size = new Vector2(Convert.ToInt32(args[i + 1]), Convert.ToInt32(args[i + 2]));
                    Debug.Log("Map size set to " + args[i + 1] + " " + args[i + 2]);
                    break;
                case "-map.scale":
                    GameObject.Find("TerrainManager").GetComponent<TerrainManager>().Scale = Convert.ToInt32(args[i + 1]);
                    Debug.Log("Map biome scale set to " + args[i + 1]);
                    break;
                case "-vg":
                    GameObject.Find("TerrainManager").GetComponent<TerrainManager>().Verbose = true;
                    Debug.Log("Procedural generation is in verbose mode");
                    break;
            }
        }
    }
}