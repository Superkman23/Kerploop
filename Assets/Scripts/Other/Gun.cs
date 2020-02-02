/*
 * Gun.cs
 * Created by: Ambrosia
 * Created on: 2/1/2020 (dd/mm/yy)
 * Created for: shooting a gun
 */

using System.Collections;
using UnityEngine;

public enum MouseButton
{
    Left,
    Right,
    ScrollWheel
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public abstract class Gun : MonoBehaviour, Interactable
{
    [Header("Components")]
    [SerializeField] protected AudioClip _ShootNoise;
    protected AudioSource _AudioSource;
    [SerializeField] Light _Flash;
    [SerializeField] float _FlashIntensity;


    [Header("Settings")]
    [SerializeField] Vector3 _OffsetFromCamera = Vector3.right;
    [SerializeField] Vector3 _AimingOffset = Vector3.right;
    Vector3 _CurrentOffset;
    [SerializeField] MouseButton _ShootButton = MouseButton.Left;
    [SerializeField] MouseButton _AimButton = MouseButton.Right;


    [Header("Shooting")]
    [Tooltip("The force applied to an object that has a rigidbody and has been shot")]
    [SerializeField] protected float _RigidbodyForce = 10;

    [Header("Bullet")]
    [Tooltip("Maximum distance a bullet can go and still effect another object")]
    [SerializeField] protected float _BulletMaxDistance = 3000;

    [Tooltip("How inaccurate the gun is")]
    [SerializeField] protected float _Spread;

    [Tooltip("How Many Shots can be taken before needing to reload")]
    [SerializeField] protected int _ClipSize = 1;
    int _ShotsRemaining;

    [Tooltip("How long it takes to reload")]
    [SerializeField] protected float _ReloadTime = 1;
    bool _IsReloading = false;

    [Tooltip("Minimum amount of time between shots")]
    [SerializeField] protected int _ShotDelay = 1;
    int _TimeUntilNextShot;

    [Tooltip("Determines whether you can hold the mouse down to repeatedly shoot")]
    [SerializeField] protected bool _Automatic;

    [Tooltip("The volume the gun shoots at")]
    [SerializeField] [Range(0, 1)] protected float _ShootNoiseVolume = 0.75f;

    [HideInInspector] public bool _IsGunEquipped = false;
    [HideInInspector] public bool _ToThrow = false; // Check to see if the gun is to be thrown
    private bool _IsAiming;
    protected Camera _MainCamera;

    private void Awake()
    {
        _Flash.intensity = 0;
        _ShotsRemaining = _ClipSize;
        _MainCamera = Camera.main;
        _AudioSource = GetComponent<AudioSource>();
        _CurrentOffset = _OffsetFromCamera;
    }

    private void Update()
    {
        if (_ToThrow)
        {
            AimStop();
            _ToThrow = false;
        }

        if (_IsGunEquipped)
        {
            HandleAiming();
            if (_ShotsRemaining > 0 && _TimeUntilNextShot == 0)
            {
                // If the player has attempted to shoot
                if (Input.GetMouseButtonDown((int)_ShootButton) && !_Automatic)
                {
                    Shoot();
                    _Flash.intensity = _FlashIntensity;
                    _ShotsRemaining--;
                    _TimeUntilNextShot = _ShotDelay;
                }

                if (Input.GetMouseButton((int)_ShootButton) && _Automatic)
                {
                    Shoot();
                    _Flash.intensity = _FlashIntensity;
                    _ShotsRemaining--;
                    _TimeUntilNextShot = _ShotDelay;
                }
            }

            if (_ShotsRemaining <= 0 || Input.GetKeyDown(KeyCode.R))
            {
                if (!_IsReloading)
                {
                    StartCoroutine(Reload());
                }
            }
        }

        // decay the intensity of the light
        _Flash.intensity = Mathf.Lerp(_Flash.intensity, 0, .1f);
    }

    private void FixedUpdate()
    {
        if (_TimeUntilNextShot > 0)
        {
            _TimeUntilNextShot--;
        }
    }

    IEnumerator Reload()
    {
        _IsReloading = true;
        yield return new WaitForSeconds(_ReloadTime);
        _ShotsRemaining = _ClipSize;
        _IsReloading = false;
    }


    void Interactable.OnInteractStart(GameObject interacting)
    {
        // If the player is trying to interact with the gun
        if (interacting.CompareTag("Player"))
        {
            if (Globals._MainPlayer._CurrentGun == null)
            {
                PickupGun();
            }
            else
            {
                AimStop();
                Globals._MainPlayer.DropWeapon();
                PickupGun();
            }
        }
    }

    private void PickupGun()
    {
        _IsGunEquipped = true;
        Globals._MainPlayer._CurrentGun = gameObject;

        GetComponent<Rigidbody>().isKinematic = true;

        Camera mainCamera = Camera.main; // Grab the camera so we don't have to reference it multiple times
        transform.parent = mainCamera.transform; // Parent the gun onto the camera
        transform.localPosition = _CurrentOffset; // We've parented, so that'll be the camera's transform
        transform.localRotation = Quaternion.Euler(0, 180, 0); // Rotate the gun to point forward
        CF.RecursiveSetColliders(transform, false);
    }

    private void HandleAiming()
    {
        if (Input.GetMouseButtonDown((int)_AimButton))
        {
            AimStart();
            transform.localPosition = _CurrentOffset;
        }

        if (Input.GetMouseButtonUp((int)_AimButton))
        {
            AimStop();
            transform.localPosition = _CurrentOffset;
        }
    }

    private void AimStart()
    {
        if (!_IsAiming)
        {
            _CurrentOffset = _AimingOffset;
            _Spread /= 2;
            _IsAiming = true;
        }
    }

    private void AimStop()
    {
        if (_IsAiming)
        {
            _CurrentOffset = _OffsetFromCamera;
            _Spread *= 2;
            _IsAiming = false;
        }
    }

    public abstract void Shoot();
}
