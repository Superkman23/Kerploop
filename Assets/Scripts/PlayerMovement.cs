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
    
    [Header("Gravity")]
    [SerializeField] private float _GravityMultiplier = 1;

    private Rigidbody _Rigidbody;

    void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var input = new Vector3(Input.GetAxis("Horizontal") * _MovementSpeed, _Rigidbody.velocity.y ,Input.GetAxis("Vertical") * _MovementSpeed);
        if (input.x == 0 && input.z == 0)
        {
            if (input.y != 0) // See if we are falling
                _Rigidbody.velocity = (Vector3.down * _Rigidbody.velocity.y) * _GravityMultiplier;

            return; // exit out early
        }
        
        _Rigidbody.velocity = transform.TransformDirection(input);
    }
}
