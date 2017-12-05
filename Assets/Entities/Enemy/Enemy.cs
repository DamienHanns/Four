using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity {

    [SerializeField] protected float damage = 1.0f;

	// Use this for initialization
	protected override void  Start () {
        base.Start();
	}
	
	
}
