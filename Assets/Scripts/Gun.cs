/*
 * Gun.cs
 * Created by: Ambrosia
 * Created on: 2/1/2020 (dd/mm/yy)
 * Created for: shooting a gun
 */

using UnityEngine;

public enum MouseButton 
{
	Left,
	Right,
	ScrollWheel
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour, Interactable
{	
	[Header("Components")]
	[SerializeField] Transform _ParticleSystemPosition;
	[SerializeField] GameObject _ParticleSystem;
	[SerializeField] AudioClip _ShootNoise;
	AudioSource _AudioSource;

	[Header("Settings")]
	[SerializeField] Vector3 _OffsetFromCamera = Vector3.right;
	[SerializeField] MouseButton _ShootButton = MouseButton.Left;

	[Header("Shooting")]
	[Tooltip("Maximum distance a bullet can go and still effect another object")]
	[SerializeField] float _BulletMaxDistance = 3000;
	[Tooltip("The force applied to an object that has a rigidbody and has been shot")]
	[SerializeField] float _RigidbodyForce = 10;
	[SerializeField] [Range(0, 1)] float _GunNoiseVolume = 0.75f;

	bool _PlayerHasGun = false;
	Camera _MainCamera;

	private void Awake() 
	{
		_MainCamera = Camera.main;
		_AudioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (_PlayerHasGun)
		{
			// If the player has attempted to shoot
			if (Input.GetMouseButtonDown((int)_ShootButton))
			{
				// Create the particle system
				var go = Instantiate(_ParticleSystem,
                    _ParticleSystemPosition.position,
                    Quaternion.Euler(0, -90 + transform.localEulerAngles.y, 0));
				// Play the audio of the gun shooting
				_AudioSource.PlayOneShot(_ShootNoise, _GunNoiseVolume);

				if (Physics.Raycast(
                    _MainCamera.transform.position, // Shoots from the main camera
                    _MainCamera.transform.forward,  // Shoots forwards
                    out RaycastHit hit, _BulletMaxDistance))
				{
					var hitRB = hit.rigidbody;
					if (hitRB != null)
					{						
						hitRB.AddForce(_MainCamera.transform.forward * _RigidbodyForce, ForceMode.Impulse);
					}
				}
			}
		}
	}
	
	void Interactable.OnInteractStart(GameObject interacting)
	{
		// If the player is trying to interact with the gun
		if (interacting.CompareTag("Player"))
		{
			// We've picked the gun up
			_PlayerHasGun = true;
			GetComponent<Rigidbody>().isKinematic = true;
			
			Camera mainCamera = Camera.main; // Grab the camera so we don't have to reference it multiple times
			transform.parent = mainCamera.transform; // Parent the gun onto the camera
			transform.localPosition = _OffsetFromCamera; // We've parented, so that'll be the camera's transform
			transform.localRotation = Quaternion.Euler(0, -90, 0); // Rotate the gun to point forward
		}
	}
}
