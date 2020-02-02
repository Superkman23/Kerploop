/*
 * Global.cs
 * Created by: Ambrosia
 * Created on: 2/2/2020 (dd/mm/yy)
 * Created for: containing global variables
 */

using UnityEngine;

public struct Globals
{
    public static Player _MainPlayer;
}

public struct CF // stands for Common Functions
{
    public static void RecursiveSetColliders(Transform root, bool value)
    {
        // Loops through all of the gun's parts
        foreach (Transform child in root)
        {
            var component = child.GetComponent<Collider>();
            if (component != null) // Disable the collider
                component.enabled = value;

            RecursiveSetColliders(child, value);
        }
    }
}