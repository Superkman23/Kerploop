/*
 * PlayerMovement.cs
 * Created by: Kaelan Bartlett
 * Created on: 31/01/2020 (dd/mm/yy)
 * Created for: Moving the player
 */


using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _MovementSpeed = 5;


    private Rigidbody _Rigidbody;

    void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var input = new Vector3(Input.GetAxis("Horizontal") * _MovementSpeed, _Rigidbody.velocity.y ,Input.GetAxis("Vertical") * _MovementSpeed);
        _Rigidbody.velocity = transform.TransformDirection(input);
    }
}
