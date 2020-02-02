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

    [Header("Settings")]
	[SerializeField] Vector3 _OffsetFromCamera = Vector3.right;
	[SerializeField] MouseButton _ShootButton = MouseButton.Left;

	[Header("Shooting")]
	[Tooltip("Maximum distance a bullet can go and still effect another object")]
	[SerializeField] protected float _BulletMaxDistance = 3000;
    [Tooltip("How inaccurate a gun is")]
    public float _Spread;
    [Tooltip("The force applied to an object that has a rigidbody and has been shot")]
	[SerializeField] protected float _RigidbodyForce = 10;
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



    [SerializeField] [Range(0, 1)] protected float _ShootNoiseVolume = 0.75f;

	[HideInInspector] public bool _IsGunEquipped = false;
	protected Camera _MainCamera;

	private void Awake() 
	{
        _ShotsRemaining = _ClipSize;
		_MainCamera = Camera.main;
		_AudioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (_IsGunEquipped)
		{
			// If the player has attempted to shoot
			if (Input.GetMouseButtonDown((int)_ShootButton) && _ShotsRemaining > 0 && _TimeUntilNextShot == 0 && !_Automatic)
			{
                Shoot();
                _ShotsRemaining--;
                _TimeUntilNextShot = _ShotDelay;
			}

            if (Input.GetMouseButton((int)_ShootButton) && _ShotsRemaining > 0 && _TimeUntilNextShot == 0 && _Automatic)
            {
                Shoot();
                _ShotsRemaining--;
                _TimeUntilNextShot = _ShotDelay;
            }

            if (_ShotsRemaining <= 0 || Input.GetKeyDown(KeyCode.R))
            {
                if (!_IsReloading)
                {
                    StartCoroutine(Reload());
                }
            }

		}
	}

    private void FixedUpdate()
    {
        if(_TimeUntilNextShot > 0)
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
		transform.localPosition = _OffsetFromCamera; // We've parented, so that'll be the camera's transform
		transform.localRotation = Quaternion.Euler(0, 180, 0); // Rotate the gun to point forward
		CF.RecursiveSetColliders(transform, false);
	}

    public abstract void Shoot();
}
