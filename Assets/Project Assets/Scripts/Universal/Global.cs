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
        // Loops through all of the gun's parts
        foreach (Transform child in root)
        {
            var collider = child.GetComponent<Collider>();
            if (collider != null) // Disable the collider
                collider.enabled = value;

            RecursiveSetColliders(child, value);
        }
    }
}

