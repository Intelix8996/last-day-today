using System.Collections;
using System;
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

    public GameObject sittingInVehicle = null;

    NetworkController Controller;

    private void Start()
    {
        if (isLocalPlayer)
            cam = Instantiate(CameraPrefab).GetComponent<Camera>() as Camera;

        rb = GetComponent<Rigidbody2D>();
        Controller = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkController>();
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            //Debug.Log("Ping: " + NetworkManager.singleton.client.GetRTT());

            centerPoint = new Vector2(Screen.width / 2, Screen.height / 2);

            move.x = Input.GetAxis("Horizontal");
            move.y = Input.GetAxis("Vertical");

            mouse = Input.mousePosition - centerPoint;

            if (mouse.x > 0)
                rb.MoveRotation(ToDeg(Mathf.Atan(mouse.y / mouse.x)) + 90);
            else
                rb.MoveRotation(ToDeg(Mathf.Atan(mouse.y / mouse.x)) - 90);

            cam.transform.position = transform.position + new Vector3(0, 0, -50) + mouse / 225;
        }

        if (sittingInVehicle == null)
            Move();

        GetComponent<Animator>().SetFloat("Speed", rb.velocity.magnitude / 18);
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

    [ClientRpc]
    public void RpcSyncIsSitting(GameObject Vehicle)
    {
        sittingInVehicle = Vehicle;
    }
}
