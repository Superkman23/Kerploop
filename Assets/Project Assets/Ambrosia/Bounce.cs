/*
 * Bounce.cs
 * Created by: Ambrosia
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;

public class Bounce : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _Force = 50;

    private void OnCollisionEnter(Collision collision)
    {
        var rb = collision.transform.GetComponent<Rigidbody>();
        if (rb == null)
            return;
        rb.AddForce(Vector3.up * _Force);
    }
}
