/*
 * PlayerGun.cs
 * Created by: Ambrosia
 * Created on: 2/2/2020 (dd/mm/yy)
 * Created for: specifying a type of gun that the player can pick up, throw and shoot
 */

using UnityEngine;

public abstract class PlayerGun : CoreGun, Interactable
{
    [Header("Controls")]
    [SerializeField] protected CF.MButton _ShootButton = CF.MButton.Left;
    [SerializeField] protected CF.MButton _AimButton = CF.MButton.Right;

    [HideInInspector] public bool _IsEquipped = false;
    [HideInInspector] public bool _GoingToThrow = false;
    protected Camera _MainCamera;
    Vector3 _TargetPosition;

    protected override void Awake()
    {
        base.Awake();
        _MainCamera = Camera.main;
        _CurrentInClip = _ClipSize;
        _TargetPosition = _DefaultPosition;
    }

    private void Update()
    {
        _Flash.intensity = Mathf.Lerp(_Flash.intensity, 0, 0.1f);

        // The rest of the code is only equip-specific so exit early if we aren't equipped
        if (_IsEquipped == false)
            return;

        HandleAiming();

        if (_IsReloading)
            return;

        // Handle throwing
        if (_GoingToThrow)
        {
            Aim(false);
            _GoingToThrow = false;
        }

        // if we have ammo in the clip and we are allowed to shoot (from the shot delay)
        if (_CurrentInClip > 0 && _TimeTillNextShot == 0)
        {
            // check if we're not using an automatic gun and we shoot
            if (Input.GetMouseButtonDown((int)_ShootButton) && !_IsAutomatic)
            {
                Shoot(_MainCamera.transform);
                _Flash.intensity = _FlashIntensity;
                RemoveBulletFromClip(1);
                _TimeTillNextShot = _ShotDelay;
            }

            // if we're using automatic
            if (Input.GetMouseButton((int)_ShootButton) && _IsAutomatic)
            {
                Shoot(_MainCamera.transform);
                _Flash.intensity = _FlashIntensity;
                RemoveBulletFromClip(1);
                _TimeTillNextShot = _ShotDelay;
            }
        }

        if (_CurrentInClip == 0 || Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    // All physics related functions
    private void FixedUpdate()
    {
        if (_TimeTillNextShot > 0)
            _TimeTillNextShot--;
    }

    void Interactable.OnInteractStart(GameObject interacting)
    {
        // If the player is trying to interact with the gun
        if (interacting.CompareTag("Player"))
        {
            if (Globals._MainPlayer.GetWeapon() == null)
            {
                Pickup();
            }
            else
            {
                Aim(false);
                Globals._MainPlayer.ThrowWeapon();
                Pickup();
            }
        }
    }

    private void HandleAiming()
    {
        // Handle aiming down sights
        if (Input.GetMouseButtonDown((int)_AimButton))
        {
            Aim(true);
            _TargetPosition = _AimingPosition;
        }
        if (Input.GetMouseButtonUp((int)_AimButton))
        {
            Aim(false);
            _TargetPosition = _DefaultPosition;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, _TargetPosition, 0.1f);
    }

    private void Pickup()
    {
        _IsEquipped = true;
        Globals._MainPlayer.SetWeapon(this);

        GetComponent<Rigidbody>().isKinematic = true;

        Camera mainCamera = Camera.main; // Grab the camera so we don't have to reference it multiple times
        transform.parent = mainCamera.transform; // Parent the gun onto the camera
        _TargetPosition = _DefaultPosition; // We've parented, so that'll be the camera's transform
        transform.localRotation = Quaternion.Euler(0, 180, 0); // Rotate the gun to point forward
        CF.RecursiveSetColliders(transform, false);
    }
}
