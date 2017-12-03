using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponContoller : MonoBehaviour {

    public Transform weaponPosition;
    public Weapon startingWeapon;

    Weapon currentWeapon;

    private void Start()
    {
        if (startingWeapon != null)
        {
            EquipWeapon(startingWeapon);
        }
    }

    public void EquipWeapon(Weapon weaponToEquip)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        currentWeapon = Instantiate(weaponToEquip, weaponPosition.position, weaponPosition.rotation,weaponPosition);
    }

    public void UseWeapon()
    {
        currentWeapon.UseWeapon();
    }
}
