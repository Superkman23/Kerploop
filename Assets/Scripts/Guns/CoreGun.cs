﻿/*
 * CoreGun.cs
 * Created by: Ambrosia
 * Created on: 2/2/2020 (dd/mm/yy)
 * Created for: a base class for Gun-like objects
 */

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class CoreGun : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] protected AudioClip _ShootNoise;
    [SerializeField] protected Light _Flash;
    protected LineRenderer _LineRenderer;
    protected AudioSource _AudioSource;

    [Header("General Gun Settings")]
    [SerializeField] protected bool _IsAutomatic;
    [SerializeField] protected float _AimingAccuracyMultiplier;
    [SerializeField] protected float _BulletSpread;
    [SerializeField] protected float _BulletMaxDistance;
    [SerializeField] protected int _BulletDamage;

    [Header("Positioning")]
    [SerializeField] protected Vector3 _DefaultPosition;
    [SerializeField] protected Vector3 _AimingPosition;
    protected bool _IsAiming;

    [Header("Shooting")]
    [SerializeField] protected int _ShotDelay;
    [SerializeField] [Range(0, 1)] protected float _ShotVolume;
    protected int _TimeTillNextShot; // To do with shot delay

    [Header("Ammo")]
    [SerializeField] protected int _MaxAmmo;
    [SerializeField] protected int _ClipSize;
    protected int _CurrentInClip;
    protected int _CurrentAmmoTotal;

    [Header("Reloading")]
    [SerializeField] protected float _ReloadTime;
    protected WaitForSecondsRealtime _ReloadTimeDelay;
    [HideInInspector] public bool _IsReloading;

    [Header("Visuals")]
    [SerializeField] protected float _FlashIntensity;
    [SerializeField] float _BulletLineOffset;

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
        _CurrentInClip = _ClipSize;
        _MaxAmmo = _ClipSize * 10;
        _CurrentAmmoTotal = _MaxAmmo;
    }

    protected virtual void Awake()
    {
        // Zero the intensity of the flash on the muzzle
        _Flash.intensity = 0;
        _ReloadTimeDelay = new WaitForSecondsRealtime(_ReloadTime);

        _AudioSource = GetComponent<AudioSource>();
        _LineRenderer = GetComponent<LineRenderer>();
        _LineRenderer.enabled = false;
        _LineRenderer.useWorldSpace = true;
    }

    public IEnumerator Reload()
    {
        // Exit out early so we don't need to do a check every time we want to reload
        if (_CurrentAmmoTotal == 0 || _CurrentInClip == _ClipSize)
            yield break;

        _IsReloading = true;
        yield return _ReloadTimeDelay;

        int difference = _ClipSize - _CurrentInClip;
        int amountToDeduct = (_CurrentAmmoTotal >= difference) ? difference : _CurrentAmmoTotal;
        _CurrentInClip += amountToDeduct;
        _CurrentAmmoTotal -= amountToDeduct;

        _IsReloading = false;
    }

    public void Aim(bool startAim)
    {
        if (startAim && !_IsAiming)
        {
            // We're now aiming
            _BulletSpread /= _AimingAccuracyMultiplier;
        }
        if (!startAim && _IsAiming)
        {
            // No longer aiming
            _BulletSpread *= _AimingAccuracyMultiplier;
        }

        _IsAiming = !_IsAiming;
    }

    public virtual IEnumerator DrawLineTo(Vector3 point)
    {
        _LineRenderer.enabled = true;
        _LineRenderer.SetPosition(0, transform.position + -transform.forward * _BulletLineOffset);
        _LineRenderer.SetPosition(1, point);

        yield return new WaitForSecondsRealtime(0.25f);

        _LineRenderer.enabled = false;
    }

    public abstract void Shoot(Transform position);

    public int GetCurrentClipAmmo() => _CurrentInClip;
    public void RemoveBulletFromClip(int amount) => _CurrentInClip -= amount;
    public int GetCurrentTotalAmmo() => _CurrentAmmoTotal;
    public WaitForSecondsRealtime GetShotDelay() => _ReloadTimeDelay;
}
