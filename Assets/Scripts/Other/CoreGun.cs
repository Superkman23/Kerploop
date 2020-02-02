/*
 * CoreGun.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class CoreGun : MonoBehaviour
{
	[Header("Required Components")]
	[SerializeField] protected AudioClip _ShootNoise;
	[SerializeField] protected Light _Flash;
	protected AudioSource _AudioSource;

	[Header("General Gun Settings")]
	[SerializeField] protected bool _IsAutomatic;
	[SerializeField] protected float _AimingAccuracyMultiplier;
	[SerializeField] protected float _BulletSpread;
	[SerializeField] protected float _BulletMaxDistance;

	[Header("Positioning")]
	[SerializeField] protected Transform _DefaultPosition;
	[SerializeField] protected Transform _AimingPosition;
	protected Transform _CurrentPosition;
	protected bool _IsAiming;
	
	[Header("Shooting")]
	// Must be in frames
	[SerializeField] protected int _ShotDelay;
	[SerializeField] [Range(0, 1)] protected float _ShotVolume;
	protected int _TimeTillNextShot; // To do with shot delay

	[Header("Ammo")]
	[SerializeField] protected int _MaxAmmo;
	[SerializeField] protected int _ClipSize;
	protected int _CurrentAmmo;

	[Header("Reloading")]
	[SerializeField] protected float _ReloadTime;
	protected WaitForSecondsRealtime _ReloadTimeDelay;
	protected bool _IsReloading;

	[Header("Muzzle Flash")]
	[SerializeField] protected float _FlashIntensity;

	protected CoreGun()
	{
		_IsAutomatic = false;
		
		_BulletMaxDistance = 100;
		_BulletSpread = 0;
		_AimingAccuracyMultiplier = 2;

		_ShotDelay = 1;
		_ShotVolume = 0.5f;
		_TimeTillNextShot = 0;

		_ReloadTime = 1;
		_IsReloading = false;

		_ClipSize = 6;
		_CurrentAmmo = _MaxAmmo;
	}

	protected virtual void Awake()
	{
		// Zero the intensity of the flash on the muzzle
		_Flash.intensity = 0;
		_ReloadTimeDelay = new WaitForSecondsRealtime(_ReloadTime);
		
		_AudioSource = GetComponent<AudioSource>();
	}

	public IEnumerator Reload()
	{
		// Exit out early so we don't need to do a check every time we want to reload
		if (_CurrentAmmo == 0)
			yield break;

		_IsReloading = true;
		yield return _ReloadTimeDelay;

		_CurrentAmmo -= _ClipSize;
		if (_CurrentAmmo < 0)
			_CurrentAmmo = 0;

		_IsReloading = false;
	}

	public void Aim (bool startAim)
	{
		if (startAim && !_IsAiming)
		{
			// We're now aiming
			_CurrentPosition = _AimingPosition;
			_BulletSpread /= _AimingAccuracyMultiplier;
		}
		if (!startAim && _IsAiming)
		{
			// No longer aiming
			_CurrentPosition = _DefaultPosition;
			_BulletSpread *= _AimingAccuracyMultiplier;
		}

		_IsAiming = !_IsAiming;
	}
}

/*


*/