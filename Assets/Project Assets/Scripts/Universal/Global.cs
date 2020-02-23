/*
 * Global.cs
 * Created by: Kaelan Bartlett
 * Created on: 10/2/2020 (dd/mm/yy)
 * Created for: Functions used across many scripts
 */

using UnityEngine;

public class Global : MonoBehaviour
{
    static public void RecursiveSetColliders(Transform root, bool value)
    {
        var thisCollider = root.GetComponent<Collider>();
        if (thisCollider != null)
            thisCollider.enabled = value;

        // Loops through all of the gun's parts
        foreach (Transform child in root)
        {
            RecursiveSetColliders(child, value);
        }
    }
}

