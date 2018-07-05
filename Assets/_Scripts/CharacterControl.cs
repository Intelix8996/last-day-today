using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

    Vector2 move, mouse;

    Vector3 centerPoint;

    [SerializeField]
    GameObject[] legs = new GameObject[2];

    [SerializeField]
    Camera cam;

    float legCycle;

    private void Start()
    {
        centerPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        StartCoroutine("legCycleLerp");
    }

    private void FixedUpdate()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");

        mouse = Input.mousePosition - centerPoint;

        transform.position += new Vector3(move.x / 3, move.y / 3, 0);

        if (mouse.x > 0)
            transform.eulerAngles = new Vector3(0, 0, ToDeg(Mathf.Atan(mouse.y / mouse.x)) + 90);
        else
            transform.eulerAngles = new Vector3(0, 0, ToDeg(Mathf.Atan(mouse.y / mouse.x)) - 90);

        cam.transform.position = transform.position + new Vector3(0, 0, -5);

        legs[0].transform.localPosition = Vector3.Lerp(new Vector3(0.3f, 0, 0), new Vector3(0.3f, -0.5f, 0), legCycle);
        legs[1].transform.localPosition = Vector3.Lerp(new Vector3(-0.25f, 0, 0), new Vector3(-0.25f, -0.5f, 0), 1 - legCycle);
    }

    float ToDeg(float radians)
    {
        return radians * (180 / Mathf.PI);
    }

    IEnumerator legCycleLerp()
    {
        while (true)
        {
            for (float i = 0; i <= 1; i += 0.075f)
            {
                legCycle = i;

                yield return new WaitForSeconds(0.01f);
            }

            for (float i = 1; i >= 0; i -= 0.075f)
            {
                legCycle = i;

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
