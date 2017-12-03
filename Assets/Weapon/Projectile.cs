using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;

    public float moveSpeed = 10.0f;
    [SerializeField] float damage = 1.0f;

    [SerializeField] float collisionDisBuffer = 0.15f;

    private void Start()
    {
        Invoke("SelfDestruct", 4.0f);

        Collider[] initalCollisions = Physics.OverlapSphere(transform.position, 0.1f, collisionMask);
        if (initalCollisions.Length > 0)
        {
            OnHitObject(initalCollisions[0]);
        }

    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
 
	void Update () {
        float moveDistance = moveSpeed * Time.deltaTime;
        CheckCollisions(moveDistance);

        transform.Translate(Vector3.forward * moveDistance);
	}

    private void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + collisionDisBuffer, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    private void OnHitObject(RaycastHit hit)
    {
        IDamageable damagableObject = hit.collider.GetComponent<IDamageable>();

        if (damagableObject != null)
        {
            damagableObject.TakeDamage(damage, hit);
        }

        SelfDestruct();
    }

    private void OnHitObject(Collider collider)
    {
        IDamageable damagableObject = collider.GetComponent<IDamageable>();

        if (damagableObject != null)
        {
            damagableObject.TakeDamage(damage);
        }

        SelfDestruct();
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }

}
