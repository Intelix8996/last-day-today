using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterControl : NetworkBehaviour {

    Vector3 move, mouse, centerPoint;

    [SerializeField]
    GameObject[] legs = new GameObject[2];

    [SerializeField]
    Camera cam;

    [SerializeField]
    GameObject CameraPrefab;

    Rigidbody2D rb;

    float legCycle;

    [SerializeField]
    float Speed = 650, SpeedAlt = 250;

    [SerializeField]
    bool AlternateMovement = false;

    bool Sprint = false;

    NetworkController Controller;

    private void Start()
    {
        if (isLocalPlayer)
        {
            centerPoint = new Vector2(Screen.width / 2, Screen.height / 2);

            cam = Instantiate(CameraPrefab).GetComponent<Camera>() as Camera;
        }

        rb = GetComponent<Rigidbody2D>();
        Controller = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkController>();

        StartCoroutine("legCycleLerp");
    }

    private void FixedUpdate()
    {
        Debug.Log(NetworkManager.singleton.client.GetRTT());

        if (isLocalPlayer)
        {
            move.x = Input.GetAxis("Horizontal");
            move.y = Input.GetAxis("Vertical");

            mouse = Input.mousePosition - centerPoint;

            if (mouse.x > 0)
                transform.eulerAngles = new Vector3(0, 0, ToDeg(Mathf.Atan(mouse.y / mouse.x)) + 90);
            else
                transform.eulerAngles = new Vector3(0, 0, ToDeg(Mathf.Atan(mouse.y / mouse.x)) - 90);

            cam.transform.position = transform.position + new Vector3(0, 0, -15) + mouse / 225;
        }

        Move();

        legs[0].transform.localPosition = Vector3.Lerp(new Vector3(0.3f, 0, 1), new Vector3(0.3f, -0.5f, 1), legCycle);
        legs[1].transform.localPosition = Vector3.Lerp(new Vector3(-0.25f, 0, 1), new Vector3(-0.25f, -0.5f, 1), 1 - legCycle);
    }

    private void Move()
    {
        if (AlternateMovement)
        {
            rb.drag = 10;

            rb.AddForce(move * rb.mass * SpeedAlt);

            if (Mathf.Abs(rb.velocity.x) > 1.5f)
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * 1.5f, rb.velocity.y);

            if (Mathf.Abs(rb.velocity.y) > 1.5f)
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * 1.5f);
        }
        else
        {
            rb.drag = 15;

            rb.AddForce(move * rb.mass * Speed);
        }
    }

    float ToDeg(float radians) { return radians * (180 / Mathf.PI); }

    IEnumerator legCycleLerp()
    {
        while (true)
        {
            for (float i = 0; i <= 1; i += Mathf.Lerp(0, 0.075f, move.magnitude))
            {
                legCycle = i;

                yield return new WaitForSeconds(0.01f);
            }

            for (float i = 1; i >= 0; i -= Mathf.Lerp(0, 0.075f, move.magnitude))
            {
                legCycle = i;

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
