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
    [SerializeField] Vector3 _Force;

    private void OnCollisionEnter(Collision collision)
    {
        var rb = collision.transform.GetComponent<Rigidbody>();
        if (rb == null)
            return;

        rb.AddForce(_Force);
    }
}
