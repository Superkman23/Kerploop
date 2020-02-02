/*
 * PlayerGun.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;

public abstract class PlayerGun : CoreGun, Interactable
{
    [Header("Controls")]
    [SerializeField] protected CF.MButton _ShootButton = CF.MButton.Left;
    [SerializeField] protected CF.MButton _AimButton = CF.MButton.Right;

    protected Camera _MainCamera;

    [HideInInspector] public bool _IsEquipped = false;
    [HideInInspector] public bool _GoingToThrow = false;

    protected override void Awake()
    {
        base.Awake();
        _MainCamera = Camera.main;
    }

    private void Update()
    {
        _Flash.intensity = Mathf.Lerp(_Flash.intensity, 0, 0.1f);

        if (_IsEquipped == false)
            return;

        if (_GoingToThrow)
        {
            Aim(false);
            _GoingToThrow = false;
        }

        if (Input.GetMouseButtonDown((int)_AimButton))
        {
            Aim(true);
            transform.position = _AimingPosition.position;
        }
        if (Input.GetMouseButtonUp((int)_AimButton))
        {
            Aim(false);
            transform.position = _DefaultPosition.position;
        }

        if (GetAmmoInClip() > 0 && _TimeTillNextShot == 0)
        {
            // If the player has attempted to shoot
            if (Input.GetMouseButtonDown((int)_ShootButton) && !_IsAutomatic)
            {
                Shoot();
                _Flash.intensity = _FlashIntensity;
                _CurrentAmmo--;
                _TimeTillNextShot = _ShotDelay;
            }

            if (Input.GetMouseButton((int)_ShootButton) && _IsAutomatic)
            {
                Shoot();
                _Flash.intensity = _FlashIntensity;
                _CurrentAmmo--;
                _TimeTillNextShot = _ShotDelay;
            }
        }

        if (GetAmmoInClip() == 0 || Input.GetKeyDown(KeyCode.R))
        {
            if (!_IsReloading && _CurrentAmmo > _ClipSize)
            {
                StartCoroutine(Reload());
            }
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
            if (Globals._MainPlayer._CurrentGun == null)
            {
                Pickup();
            }
            else
            {
                Aim(false);
                Globals._MainPlayer.DropWeapon();
                Pickup();
            }
        }
    }

    private void Pickup()
    {
        _IsEquipped = true;
        Globals._MainPlayer._CurrentGun = gameObject;

        GetComponent<Rigidbody>().isKinematic = true;

        Camera mainCamera = Camera.main; // Grab the camera so we don't have to reference it multiple times
        transform.parent = mainCamera.transform; // Parent the gun onto the camera
        transform.localPosition = _CurrentPosition.localPosition; // We've parented, so that'll be the camera's transform
        transform.localRotation = Quaternion.Euler(0, 180, 0); // Rotate the gun to point forward
        CF.RecursiveSetColliders(transform, false);
    }

    protected int GetAmmoInClip()
    {
        if (_CurrentAmmo <= 0)
            return 0;
        
        return _CurrentAmmo & (_ClipSize-1);
    }

    protected abstract void Shoot(); 
}
