using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TerrainManager : NetworkBehaviour {

    public bool Verbose = false;

    [SerializeField]
    Maps Map;

    [Header("Procedural Ground Generation")]
    [SerializeField]
    Vector3 Origin;
    [SerializeField]
    public Vector3 Size;

    [SerializeField]
    public float Scale;

    public Vector2 Seed;

    [SerializeField]
    GameObject GroundPrefab;

    public float[,] Ground;

    [Header("Procedural Objects Generation")]
    [SerializeField]
    GameObject[] Objects;

    [SerializeField]
    GameObject MapParent;

    private void Start()
    {
        switch (Map)
        {
            case Maps.Procedural:
                GenerateGround();
                GenerateObjects();
                break;
            case Maps.Static:

                break;
        }
    }

    void GenerateObjects()
    {
        for (float i = Origin.y; i < Origin.y + Size.y; i += 5)
        {
            for (float j = Origin.x; j < Origin.x + Size.x; j += 5)
            {
                int seed = Random.Range(0, 1000);
                GameObject obj = null;

                if (seed > 875 && seed < 925)
                    obj = Instantiate(Objects[0], new Vector3(j, i, -5), Quaternion.identity, MapParent.transform) as GameObject;
                else if (seed > 995)
                    obj = Instantiate(Objects[1], new Vector3(j, i, -5), Quaternion.identity, MapParent.transform) as GameObject;

                if (obj != null)
                    NetworkServer.Spawn(obj);
            }

            if (Verbose)
                Debug.Log("Spawning objecst " + i + ".." + Origin.y + Size.y);
        }
    }

    void GenerateGround()
    {
        GameObject obj = Instantiate(GroundPrefab, Origin + new Vector3(0, 0, 15), Quaternion.Euler(90, -90, 90), MapParent.transform) as GameObject;

        Seed = new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000));

        obj.transform.localScale = new Vector3(Size.x / 10, 1, Size.y / 10);
        obj.transform.position += new Vector3((obj.transform.localScale.x / 2) * 10, (obj.transform.localScale.z / 2) * 10, 0);

        NetworkServer.Spawn(obj);
    }

    enum Maps
    {
        None = 0,
        Procedural = 1,
        Static = 2
    }
}
