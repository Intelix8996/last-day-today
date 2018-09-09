using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VehicleController : NetworkBehaviour {

    public int SeatsCount = 4;

    public VehicleType Type;

    public GameObject SeatSocket;

    Rigidbody2D rb;

    [SerializeField]
    [SyncVar]
    public GameObject Driver;

    [SerializeField]
    [SyncVar]
    public int OccupiedSeats = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Drive (Vector3 driveVector)
    {
        if (isServer)
        {
            rb.MoveRotation(rb.rotation - (driveVector.z * driveVector.x));

            if (transform.eulerAngles.z >= 0 && transform.eulerAngles.z <= 90)
                rb.MovePosition(rb.position - Vector2.Lerp(new Vector2(1, 0), new Vector2(0, 1), transform.eulerAngles.z / 90) * driveVector.x / 5);
            else if (transform.eulerAngles.z > 90 && transform.eulerAngles.z <= 180)
                rb.MovePosition(rb.position - Vector2.Lerp(new Vector2(0, 1), new Vector2(-1, 0), (transform.eulerAngles.z - 90) / 90) * driveVector.x / 5);
            if (transform.eulerAngles.z > 180 && transform.eulerAngles.z <= 270)
                rb.MovePosition(rb.position + Vector2.Lerp(new Vector2(1, 0), new Vector2(0, 1), (transform.eulerAngles.z - 180) / 90) * driveVector.x / 5);
            else if (transform.eulerAngles.z > 270 && transform.eulerAngles.z <= 360)
                rb.MovePosition(rb.position + Vector2.Lerp(new Vector2(0, 1), new Vector2(-1, 0), (transform.eulerAngles.z - 270) / 90) * driveVector.x / 5);
        }
    }

    public enum VehicleType
    {
        Car = 0, 
        Bike = 1
    }
}
