using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class PlayerInput : MonoBehaviour {
    
    PlayerController playerController;
    Camera mainCam;

    void Start () {
        playerController = GetComponent<PlayerController>();
        mainCam = Camera.main;
	}


    void Update () {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveDir = moveInput.normalized;
        playerController.Move(moveDir );

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (ground.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);

            playerController.LookAt(point);
        }

        if (Input.GetButton("Fire1"))
        {
            playerController.UseWeapon();
        }
    }
}
