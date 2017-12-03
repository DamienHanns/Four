using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody), (typeof (WeaponContoller)))]
public class PlayerController : LivingEntity {

    Vector3 velocity;
    Rigidbody myRb;
    WeaponContoller weaponContoller;

	protected override void Start () {
        base.Start();
        myRb = GetComponent<Rigidbody>();
        weaponContoller = GetComponent<WeaponContoller>();
	}

    public void Move(Vector3 moveVelocity)
    {
        velocity = moveVelocity;
    }

    private void FixedUpdate()
    {
        myRb.velocity = ( (velocity * 100) * Time.fixedDeltaTime);
    }

    public void LookAt(Vector3 lookPoint)
    {
        Vector3 ajustedLookPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z );
        transform.LookAt(ajustedLookPoint);
    }

    public void UseWeapon()
    {
        weaponContoller.UseWeapon();
    }
}
