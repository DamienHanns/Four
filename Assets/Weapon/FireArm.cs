using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArm : Weapon {

    public Transform barrelEnd;
    public Projectile projectile;
    public float projectileSpeed = 100.0f;
    public float shotsPerSecond = 3.0f;
    float timeBetweenShots;
    float nextShotTime;

	// Use this for initialization
	internal override void Start () {
        base.Start();
        timeBetweenShots = 1.0f / shotsPerSecond;
	}

    public override void UseWeapon()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + timeBetweenShots;
            base.UseWeapon();
            Projectile newProjectile = Instantiate(projectile, barrelEnd.position, barrelEnd.rotation);
            newProjectile.SetSpeed(projectileSpeed);
        }
    }
}
