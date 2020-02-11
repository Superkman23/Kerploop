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
    [SerializeField] KeyCode _SprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode _JumpKey = KeyCode.Space;
    [SerializeField] KeyCode _ThrowKey = KeyCode.F;
    [SerializeField] KeyCode _InteractKey = KeyCode.E;

    [Header("Movement")]
    [SerializeField] float _MovementSpeed;
    [SerializeField] float _SprintSpeedMult = 2;
    bool _IsSprinting;
    bool _IsMoving;

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

    [Header("Interacting")]
    [SerializeField] float _InteractRange;


    [Header("Guns")]
    [SerializeField] float _ThrowForce = 5;
    GameObject _CurrentGun = null;

    [Header("Camera")]
    [SerializeField] float _RotationSpeed = 1f;
    [SerializeField] float _YRotationMin = -90f;
    [SerializeField] float _YRotationMax = 90f;
    [SerializeField] float _FOVIncreasePerUnit;
    float _TargetFOV;
    float _StartingFOV;
    float _XRotation = 0;
    float _YRotation = 0;

    [Header("View Bobbing")]
    [SerializeField] float _BobbingSpeed = 0.18f;
    [SerializeField] float _BobbingAmount = 0.2f;
    float _Midpoint = 0.5f;
    private float _Timer = 0.0f;


    Camera _MainCamera;
    Rigidbody _Rigidbody;

    void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        _Rigidbody.useGravity = true;

        _MainScale = transform.localScale;
        _TargetScale = _MainScale;

        Cursor.lockState = CursorLockMode.Locked;
        _MainCamera = Camera.main;
        _StartingFOV = _MainCamera.fieldOfView;
    }
    void Update()
    {
        HandleCamera();
        HandleCrouching();
        HandleSprinting();
        ApplyViewBobbing();

        if (Input.GetKeyDown(_JumpKey) && _Grounded)
        {
            _ReadyToJump = true;
        }
        if (Input.GetKeyDown(_InteractKey))
        {
            Interact();
        }

        if(_CurrentGun != null && Input.GetKeyDown(_ThrowKey))
        {
            DropGun();
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
        Vector3 mDirection = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 1.0f);
        Vector3 newVelocity = new Vector3(mDirection.x * _MovementSpeed, _Rigidbody.velocity.y, mDirection.z * _MovementSpeed);
        _Rigidbody.velocity = transform.TransformDirection(newVelocity);
    }
    void HandleSprinting()
    {
        if (Input.GetKeyDown(_SprintKey))
        {
            _MovementSpeed *= _SprintSpeedMult;
            _BobbingSpeed *= _SprintSpeedMult;
            _IsSprinting = true;
        }
        if (Input.GetKeyUp(_SprintKey))
        {
            _MovementSpeed /= _SprintSpeedMult;
            _BobbingSpeed /= _SprintSpeedMult;
            _IsSprinting = false;
        }
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

    //Gun functions
    void Interact()
    {
        if (Physics.Raycast(_MainCamera.transform.position, _MainCamera.transform.forward, out RaycastHit hit,_InteractRange))
        {
            Gun targetGun = hit.collider.gameObject.transform.parent.GetComponent<Gun>();
            if (targetGun != null)
            {
                if(_CurrentGun == null)
                {
                    Debug.Log("Pickup");
                    targetGun.Pickup(_MainCamera.transform);
                    _CurrentGun = targetGun.gameObject;
                }
                else
                {
                    DropGun();
                    targetGun.Pickup(_MainCamera.transform);
                    _CurrentGun = targetGun.gameObject;
                }
            }
        }
    }
    void DropGun()
    {
        _CurrentGun.GetComponent<Gun>().Drop(); //drop held gun
        _CurrentGun = null; //set current gun to none because it's no longer held
    }
    //Camera Functions
    void HandleCamera()
    {
        CameraFOV();
        CameraDirection();
    }
    void CameraFOV()
    {
        if (_IsSprinting)
        {
            _TargetFOV = _StartingFOV + _Rigidbody.velocity.magnitude * _FOVIncreasePerUnit;
            _MainCamera.fieldOfView = Mathf.Lerp(_MainCamera.fieldOfView, _TargetFOV, 0.1f);
        }
        else
        {
            _TargetFOV = _StartingFOV;
        }
        _MainCamera.fieldOfView = Mathf.Lerp(_MainCamera.fieldOfView, _TargetFOV, 0.1f);
    }
    void CameraDirection()
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
    void ApplyViewBobbing()
    {
        //Code based from http://wiki.unity3d.com/index.php/Headbobber, converted to C#

        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0 || !_Grounded)
        {
            _Timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(_Timer);
            _Timer += _BobbingSpeed;
            if (_Timer > Mathf.PI * 2)
            {
                _Timer -= Mathf.PI * 2;
            }
        }

        Vector3 cameraPosition = _MainCamera.transform.localPosition;
        if (waveslice != 0)
        {
            float translateChange = waveslice * _BobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange *= totalAxes;
            cameraPosition.y = _Midpoint + translateChange;
        }
        else
        {
            cameraPosition.y = _Midpoint;
        }

        _MainCamera.transform.localPosition = cameraPosition;
    }
}
