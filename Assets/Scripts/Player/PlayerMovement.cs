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
    [SerializeField] float _MovementSpeed = 5;

    [Header("Jumping")]
    [SerializeField] KeyCode _JumpKey = KeyCode.Space;
    [SerializeField] float _JumpForce;
    bool _ReadyToJump = false;
    bool _Grounded;

    [Header("Crouching")]
    [SerializeField] [Range(0, 1)] float _CrouchSpeedMult = 0.5f;
    [SerializeField] Vector3 _CrouchScale;
    Vector3 _MainScale;
    Vector3 _TargetScale;

    [Header("Gravity")]
    [SerializeField] float _GravityMultiplier = 1;

    Rigidbody _Rigidbody;

    private void Awake()
    {
        _MainScale = transform.localScale;
        _TargetScale = _MainScale;
        _Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ManageCrouching();

        if (Input.GetKeyDown(_JumpKey) && _Grounded)
        {
            _ReadyToJump = true;
        }
    }


    private void FixedUpdate()
    {
        Movement();

        if(_ReadyToJump)
        {
            _Grounded = false;
            Jump();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        _Grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        _Grounded = false;
    }

    void Movement()
    {
        var mDirection = new Vector3(Input.GetAxis("Horizontal") * _MovementSpeed,
                                     _Rigidbody.velocity.y,
                                     Input.GetAxis("Vertical") * _MovementSpeed);

        if (mDirection.x == 0 && mDirection.z == 0)
        {
            if (mDirection.y != 0) // See if we are falling
                _Rigidbody.velocity = (Vector3.down * -_Rigidbody.velocity.y) * _GravityMultiplier;

            return; // exit out early
        }

        _Rigidbody.velocity = transform.TransformDirection(mDirection);
    }

    void Jump()
    {
        _Rigidbody.velocity = new Vector3(_Rigidbody.velocity.x, _Rigidbody.velocity.y + _JumpForce, _Rigidbody.velocity.z);
        _ReadyToJump = false;
    }

    void ManageCrouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _TargetScale = _CrouchScale;
            _MovementSpeed *= _CrouchSpeedMult;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _TargetScale = _MainScale;
            _MovementSpeed /= _CrouchSpeedMult;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, _TargetScale, 0.3f);
    }
}
