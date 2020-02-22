﻿/*
 * Bullet.cs
 * Created by: Kaelan 
 * Created on: 22/2/2020 (dd/mm/yy)
 * Created for: Physical Bullets
 */

using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int _Damage;
    public int _Force;
    public float _Speed;
	private void Update()
	{
        transform.Translate(Vector3.forward * _Speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {


        var targetHealthManager = other.gameObject.GetComponent<HealthManager>();

        if (targetHealthManager != null)
        {
            targetHealthManager.RemoveHealth(_Damage);
        }

        var targetRigidbody = other.attachedRigidbody;

        if (targetRigidbody != null)
        {
            targetRigidbody.AddForce(transform.forward * _Force, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }

}
