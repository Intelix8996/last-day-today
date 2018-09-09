using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UsingHandler : NetworkBehaviour {

    CharacterControl PlayerController;
    VehicleController Controller;

    private void Start()
    {
        PlayerController = GetComponent<CharacterControl>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<VehicleController>())
        {
            if (PlayerController.sittingInVehicle == null)
                Controller = collision.gameObject.GetComponent<VehicleController>();

            Vector3 driveVector = new Vector3(0, 0, 0);

            if (isLocalPlayer)
            {
                driveVector = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

                if (PlayerController.sittingInVehicle == Controller.gameObject && Controller.Driver == gameObject)
                    CmdDrive(driveVector);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (PlayerController.sittingInVehicle == null)
                    {
                        if (Controller.Driver == null)
                            CmdSetDriver(gameObject);
                        else if (Controller.OccupiedSeats < Controller.SeatsCount - 1)
                            CmdAddPassager(gameObject);
                    }
                    else
                    {
                        if (Controller.Driver == gameObject)
                            CmdSetDriver(null);
                        else
                            CmdRemovePassager(gameObject);
                    }

                    if (Controller.OccupiedSeats < Controller.SeatsCount - 1 || PlayerController.sittingInVehicle != null)
                        CmdInvertIsSitting(Controller.gameObject);
                }
            }

            if (PlayerController.sittingInVehicle == null)
                transform.position = new Vector3(transform.position.x, transform.position.y, 1);
            else if (PlayerController.sittingInVehicle == Controller.gameObject)
            {
                if (Controller.Type == VehicleController.VehicleType.Bike)
                    transform.position = Controller.SeatSocket.transform.position;
                else
                    transform.position = Controller.gameObject.transform.position + new Vector3(0, 0, 15);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (PlayerController.sittingInVehicle == null)
            Controller = null;
    }

    [Command]
    void CmdDrive(Vector3 driveVector)
    {
        Controller.Drive(driveVector);
    }

    [Command]
    void CmdInvertIsSitting(GameObject Vehicle)
    {
        if (PlayerController.sittingInVehicle == null)
            PlayerController.sittingInVehicle = Vehicle;
        else
            PlayerController.sittingInVehicle = null;

        PlayerController.RpcSyncIsSitting(PlayerController.sittingInVehicle);
    }

    [Command]
    void CmdSetDriver(GameObject Player)
    {
        Controller.Driver = Player;
    }

    [Command]
    void CmdAddPassager(GameObject Player)
    {
        Controller.OccupiedSeats++;
    }

    [Command]
    void CmdRemovePassager(GameObject Player)
    {
        Controller.OccupiedSeats--;
    }
}
