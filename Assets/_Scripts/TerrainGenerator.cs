using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TerrainGenerator : NetworkBehaviour
{
    Vector2 Seed;

    float Scale;

    [ClientRpc]
    public void RpcSyncMaterial(Vector2 seed, Vector2 size, float scale)
    {
        Seed = seed;
        Scale = scale;

        Texture2D Texture = GenerateNoiseTexture(GenerateNoiseArray((int)size.y, (int)size.x), (int)size.y, (int)size.x);

        transform.localScale = new Vector3(size.x / 10, 1, size.y / 10);

        GetComponent<Renderer>().material.SetTexture("_BlendMap", Texture);
        GetComponent<Renderer>().material.SetTextureScale("_MainTex", size * 5 / 10);
        GetComponent<Renderer>().material.SetTextureScale("_SecondTex", size * 5 / 10);
    }

    public float[,] GenerateNoiseArray(int height, int width)
    {
        Vector2 Offset = Seed;

        float[,] texture = new float[width, height];

        for (float i = 0; i < height; ++i)
        {
            for (float j = 0; j < width; ++j)
            {
                float x = Offset.x + j / width * Scale;
                float y = Offset.y + i / height * Scale;

                texture[(int)j, (int)i] = Mathf.PerlinNoise(x, y);

                texture[(int)j, (int)i] -= .5f;
                texture[(int)j, (int)i] *= 1.5f;
                texture[(int)j, (int)i] += .75f;
            }
        }

        return texture;
    }

    public Texture2D GenerateNoiseTexture(float[,] noiseArray, int height, int width)
    {
        Texture2D Texture = new Texture2D(width, height);
        Color[] Colors = new Color[width * height];

        for (float i = 0; i < height; ++i)
        {
            for (float j = 0; j < width; ++j)
            {
                Colors[(int)i * width + (int)j] = new Color(noiseArray[(int)j, (int)i], noiseArray[(int)j, (int)i], noiseArray[(int)j, (int)i]);
            }
        }

        Texture.SetPixels(Colors);
        Texture.Apply();

        return Texture;
    }
}
