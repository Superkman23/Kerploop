/*
 * TestInteract.cs
 * Created by: Ambrosia
 * Created on: 1/2/2020 (dd/mm/yy)
 * Created for: showing off the power of the Interactable interface
 */

using UnityEngine;

public class TestInteract : MonoBehaviour, Interactable
{
    [SerializeField] Vector3 _Direction = Vector3.up;
    [SerializeField] float _Force = 250;

    public void OnInteractStart()
    {
        GetComponent<Rigidbody>().AddForce(_Direction * _Force);
    }
}
