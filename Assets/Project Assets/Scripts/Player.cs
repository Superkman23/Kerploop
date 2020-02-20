/*
 * Player.cs
 * Created by: Kaelan Bartlett
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: Controlling the player
 */

using UnityEngine;

public enum MouseButton
{
    LMB,
    RMB,
    MMB
}

public class Player : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] KeyCode _SprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode _JumpKey = KeyCode.Space;
    [SerializeField] KeyCode _ThrowKey = KeyCode.F;
    [SerializeField] KeyCode _InteractKey = KeyCode.E;
    [SerializeField] MouseButton _AimButton = MouseButton.RMB;
    [SerializeField] MouseButton _ShootButton = MouseButton.LMB;
    [SerializeField] KeyCode _ReloadKey = KeyCode.R;

    [Header("Movement")]
    [SerializeField] float _MovementSpeed;
    [SerializeField] float _SprintSpeedMult = 2;

    [Header("Jumping")]
    [SerializeField] float _JumpForce;
    bool _ReadyToJump = false;
    bool _Grounded;

    [Header("Crouching")]
    [SerializeField] [Range(0, 1)] float _CrouchSpeedMult = 0.5f;
    [SerializeField] float _CrouchScale;
    [SerializeField] float _CrouchGravityMult;
    bool _IsStartingCrouch;
    Vector3 _MainScale;
    Vector3 _TargetScale;

    [Header("Interacting")]
    [SerializeField] float _InteractRange;

    [Header("Guns")]
    [SerializeField] float _ThrowForce = 5;
    [HideInInspector] public Gun _CurrentGun = null;

    [Header("Camera")]
    [SerializeField] float _RotationSpeed = 1f;
    [SerializeField] float _YRotationMin = -90f;
    [SerializeField] float _YRotationMax = 90f;
    float _XRotation = 0;
    float _YRotation = 0;

    [Header("View Bobbing")]
    [SerializeField] float _BobbingSpeed = 0.18f;
    [SerializeField] float _BobbingAmount = 0.2f;
    float _Midpoint = 0.5f;
    float _Timer = 0.0f;

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

        _XRotation = transform.eulerAngles.y;
        _YRotation = transform.eulerAngles.x;
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

        if (_CurrentGun != null) // All the things the player can do with the gun
        {
            if (Input.GetKeyDown(_ThrowKey))
            {
                DropGun();
            }
            if (Input.GetMouseButtonDown((int)_AimButton))
            {
                _CurrentGun.Aim(true);
            }
            if (Input.GetMouseButtonUp((int)_AimButton))
            {
                _CurrentGun.Aim(false);
            }
            if (Input.GetKeyDown(_ReloadKey) || _CurrentGun._CurrentInClip <= 0)
            {

                _CurrentGun.StartReloading();
            }
            if (Input.GetMouseButtonDown((int)_ShootButton) && !_CurrentGun._IsReloading && _CurrentGun._CurrentInClip > 0)
            {
                _CurrentGun.Shoot();
            }
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
        // Gets player input and caps the magnitude at one (prevents moving faster than you should),
        // Then applies the movement relative to the camera using transform maths

        Vector2 rawInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (rawInput == Vector2.zero)
            return;

        Vector2 clampedInput = Vector2.ClampMagnitude(rawInput, 1.0f);
        Vector3 newVelocity = transform.TransformDirection(new Vector3(clampedInput.x * _MovementSpeed,
                                                                       _Rigidbody.velocity.y,
                                                                       clampedInput.y * _MovementSpeed));
        _Rigidbody.velocity = newVelocity;
    }

    void HandleSprinting()
    {
        if (Input.GetKeyDown(_SprintKey))
        {
            _MovementSpeed *= _SprintSpeedMult;
            _BobbingSpeed *= _SprintSpeedMult;
        }
        if (Input.GetKeyUp(_SprintKey))
        {
            _MovementSpeed /= _SprintSpeedMult;
            _BobbingSpeed /= _SprintSpeedMult;
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
            _IsStartingCrouch = true; // Is only set to true here because we don't want more gravity while returning to normal size
            _TargetScale = new Vector3(_TargetScale.x, _CrouchScale, _TargetScale.z);
            _Midpoint *= _CrouchScale;
            _MovementSpeed *= _CrouchSpeedMult;

        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _TargetScale = _MainScale;
            _Midpoint /= _CrouchScale;
            _MovementSpeed /= _CrouchSpeedMult;
        }

        if (Mathf.Abs(transform.localScale.y - _TargetScale.y) < .1) // No longer transitioning if scales are close enough
            _IsStartingCrouch = false;
        if (_IsStartingCrouch) // Increase gravity while crouching to fall faster, exists to make starting crouching faster
            _Rigidbody.AddForce(Physics.gravity * _CrouchGravityMult);

        transform.localScale = Vector3.Lerp(transform.localScale, _TargetScale, 0.4f); // Smoothly transition into and out of crouching
    }

    //Gun functions
    void Interact()
    {
        if (Physics.Raycast(_MainCamera.transform.position, _MainCamera.transform.forward, out RaycastHit hit, _InteractRange))
        {
            Gun targetGun = hit.collider.gameObject.transform.parent.GetComponent<Gun>();
            if (targetGun != null) // Only run if the player interacted with a gun
            {
                if (_CurrentGun != null) // Throw the existing gun if the player is already holding one
                    DropGun();

                targetGun.Pickup(_MainCamera.transform); // Pickup the gun
                _CurrentGun = targetGun; //Set the current gun as this gun
            }
        }
    }
    void DropGun()
    {
        Gun equippedGun = _CurrentGun.GetComponent<Gun>();
        equippedGun.Drop(); // Drop held gun
        equippedGun._Rigidbody.AddForce(_Rigidbody.velocity + _MainCamera.transform.forward * _ThrowForce, ForceMode.Impulse); // Adds a force to the gun when you throw it
        _CurrentGun = null; // Set current gun to none because it's no longer held
    }

    //Camera Functions
    void HandleCamera()
    {
        Vector2 lookDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // Gets player input

        if (lookDirection == Vector2.zero) //Only run if the player's mouse has moved
            return;

        lookDirection *= _RotationSpeed;
        _XRotation += lookDirection.x;
        _YRotation += lookDirection.y;
        ClampRotation(); //Prevents the camera from over rotating

        _MainCamera.transform.rotation = Quaternion.Euler(-_YRotation, _MainCamera.transform.eulerAngles.y, 0); // Rotate the camera around the X axis to look up or down
        transform.rotation = Quaternion.Euler(0, _XRotation, 0); // Rotates the player around the Y axis to look left or right. Doing this rotates the camera so we don't need to rotate the camera.
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
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0 || !_Grounded || _Rigidbody.velocity.magnitude <= 0.25f)
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
