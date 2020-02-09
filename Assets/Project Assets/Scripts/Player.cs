/*
 * Player.cs
 * Created by: Kaelan Bartlett
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: Controlling the player
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] KeyCode _JumpKey = KeyCode.Space;
    [SerializeField] KeyCode _ThrowKey = KeyCode.F;

    [Header("Movement")]
    [SerializeField] float _MovementSpeed;

    [Header("Jumping")]
    [SerializeField] float _JumpForce;
    bool _ReadyToJump = false;
    bool _Grounded;

    [Header("Crouching")]
    [SerializeField] [Range(0, 1)] float _CrouchSpeedMult = 0.5f;
    [SerializeField] float _CrouchScale;
    [SerializeField] float _CrouchGravityMult;
    bool _IsCrouching = false;
    Vector3 _MainScale;
    Vector3 _TargetScale;

    [Header("Guns")]
    [SerializeField] float _ThrowForce = 5;
    GameObject _CurrentGun = null;



    [Header("Camera")]
    [SerializeField] float _RotationSpeed = 1f;
    [SerializeField] float _YRotationMin = -90f;
    [SerializeField] float _YRotationMax = 90f;
    Camera _MainCamera;
    float _XRotation = 0;
    float _YRotation = 0;


    Rigidbody _Rigidbody;

    void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        _Rigidbody.useGravity = true;

        _MainScale = transform.localScale;
        _TargetScale = _MainScale;

        Cursor.lockState = CursorLockMode.Locked;
        _MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCamera();
        HandleCrouching();

        if (Input.GetKeyDown(_JumpKey) && _Grounded)
        {
            _ReadyToJump = true;
        }
    }
    private void FixedUpdate()
    {
        HandleMovement();

        if (_ReadyToJump)
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
    //Movement Functions
    void HandleMovement()
    {
        Vector3 mDirection = new Vector3(Input.GetAxis("Horizontal") * _MovementSpeed, _Rigidbody.velocity.y, Input.GetAxis("Vertical") * _MovementSpeed);
        _Rigidbody.velocity = transform.TransformDirection(mDirection);
    }

    void Jump()
    {
        _Rigidbody.velocity = new Vector3(_Rigidbody.velocity.x, _Rigidbody.velocity.y + _JumpForce, _Rigidbody.velocity.z);
        _ReadyToJump = false;
    }

    void HandleCrouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _IsCrouching = true;
            _TargetScale = new Vector3(_TargetScale.x, _CrouchScale,_TargetScale.z );
            _MovementSpeed *= _CrouchSpeedMult;

        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _IsCrouching = false;
            _TargetScale = _MainScale;
            _MovementSpeed /= _CrouchSpeedMult;
        }

        if (Mathf.Abs(transform.localScale.y - _TargetScale.y) < .1)
        {
            _IsCrouching = false;
            transform.localScale = _TargetScale;
        }

        if (_IsCrouching)
        {
            _Rigidbody.AddForce(Physics.gravity * _CrouchGravityMult);
        }

        transform.localScale = Vector3.Lerp(transform.localScale, _TargetScale, 0.4f);
    }

    //Camera Functions
    void HandleCamera()
    {
        Vector2 lookDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (lookDirection == Vector2.zero)
            return;

        lookDirection *= _RotationSpeed;
        _XRotation += lookDirection.x;
        _YRotation += lookDirection.y;
        ClampRotation();

        _MainCamera.transform.rotation = Quaternion.Euler(-_YRotation, _MainCamera.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(0, _XRotation, 0);

    }
    void ClampRotation()
    {
        if (_YRotation < _YRotationMin)
            _YRotation = _YRotationMin;
        else if (_YRotation > _YRotationMax)
            _YRotation = _YRotationMax;
    }
}
