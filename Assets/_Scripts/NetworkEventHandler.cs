using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkEventHandler : NetworkManager {

    [SerializeField]
    GameObject[] SpawnPrefabs;

    public override void OnStartHost()
    {
        base.OnStartHost();

        Debug.Log("Host started");
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        StartCoroutine("HandleServerStartup");
    }

    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);

        Debug.Log("Client started");
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        Debug.Log("Player (" + conn.address + ") ID: " + conn.connectionId + " connected");
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        InvokeGroundSync();

        Debug.Log("Player (" + conn.address + ") ID: " + conn.connectionId + " is ready!");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        Debug.Log("Player (" + conn.address + ") ID: " + conn.connectionId + " disconnected");
    }

    void InvokeGroundSync()
    {
        TerrainGenerator Generator = GameObject.Find("Ground(Clone)").GetComponent<TerrainGenerator>();
        TerrainManager Manager = GameObject.Find("TerrainManager").GetComponent<TerrainManager>();

        Generator.RpcSyncMaterial(Manager.Seed, Manager.Size, Manager.Scale);
    }

    IEnumerator HandleServerStartup()
    {
        bool isServerReady = false;

        while (!isServerReady)
        {
            if (NetworkServer.active)
                isServerReady = true;
            else
                yield return new WaitForSeconds(.01f);
        }

        Debug.Log("Server started!");

        int Number = 0;
        foreach (GameObject prefab in SpawnPrefabs)
        {
            Number++;

            GameObject obj = Instantiate(prefab, prefab.transform.position, Quaternion.identity) as GameObject;
            NetworkServer.Spawn(obj);

            Debug.Log("Spawned " + Number + ".." + SpawnPrefabs.Length + " objects");
        }

        Debug.Log(SpawnPrefabs.Length + " objects spawned");

        yield return null;
    }
}
