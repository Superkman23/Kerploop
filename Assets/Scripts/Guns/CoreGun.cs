/*
 * CoreGun.cs
 * Created by: Ambrosia
 * Created on: 2/2/2020 (dd/mm/yy)
 * Created for:  base class for Gun-like objects
 */

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class CoreGun : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] protected AudioClip _ShootNoise;
    [SerializeField] protected Light _Flash;
    [SerializeField] protected Transform _BulletSpawnPoint;
    [SerializeField] protected GameObject _BulletRay;
    protected AudioSource _AudioSource;

    [Header("General Gun Settings")]
    [SerializeField] protected bool _IsAutomatic;
    [SerializeField] protected float _AimingAccuracyMultiplier;
    [SerializeField] protected float _BulletSpread;
    [SerializeField] protected float _BulletMaxDistance;
    [SerializeField] protected int _BulletDamage;

    [Header("Positioning")]
    [SerializeField] public    Vector3 _DefaultPosition;
    [SerializeField] protected Vector3 _AimingPosition;
    protected Vector3 _TargetPosition;
    [SerializeField] protected float _RecoilAmount;
    protected bool _IsAiming;

    [Header("Shooting")]
    [SerializeField] protected int _ShotDelay;
    [SerializeField] [Range(0, 1)] protected float _ShotVolume;
    protected int _TimeTillNextShot; // To do with shot delay

    [Header("Ammo")]
    [SerializeField] protected int _ClipSize;
    protected int _MaxAmmo;
    protected int _CurrentInClip;
    protected int _CurrentAmmoTotal;

    [Header("Reloading")]
    [SerializeField] protected float _ReloadTime;
    protected WaitForSecondsRealtime _ReloadTimeDelay;
    [HideInInspector] public bool _IsReloading;

    [Header("Visuals")]
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
        _CurrentInClip = _ClipSize;
    }

    protected virtual void Awake()
    {
        _MaxAmmo = _ClipSize * 10;
        _CurrentAmmoTotal = _MaxAmmo;

        // Zero the intensity of the flash on the muzzle
        _Flash.intensity = 0;
        _ReloadTimeDelay = new WaitForSecondsRealtime(_ReloadTime);

        _AudioSource = GetComponent<AudioSource>();
    }

    public IEnumerator Reload()
    {
        // Exit out early so we don't need to do a check every time we want to reload
        if (_CurrentAmmoTotal == 0 || _CurrentInClip == _ClipSize)
            yield break;

        _IsReloading = true;
        yield return _ReloadTimeDelay;

        if (_IsReloading) //If an action was taken that stops reloading, the gun will not reload
        {
            int difference = _ClipSize - _CurrentInClip;
            int amountToDeduct = (_CurrentAmmoTotal >= difference) ? difference : _CurrentAmmoTotal;
            _CurrentInClip += amountToDeduct;
            _CurrentAmmoTotal -= amountToDeduct;

            _IsReloading = false;
        }
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
    public virtual void CreateTracer(Vector3 point)
    {
        GameObject go = Instantiate(_BulletRay, _BulletSpawnPoint.position, Quaternion.identity, gameObject.transform);
        BulletRay ray = go.GetComponent<BulletRay>();
        ray.SetRendererPosition(point);
        StartCoroutine(ray.WaitThenDestroy(0.25f));
    }
    public void HandlePosition()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, _TargetPosition, 0.1f);
    }
    public abstract void Shoot(Transform position);
    public int GetCurrentClipAmmo() => _CurrentInClip;
    public void RemoveBulletFromClip(int amount) => _CurrentInClip -= amount;
    public int GetCurrentTotalAmmo() => _CurrentAmmoTotal;
    public WaitForSecondsRealtime GetShotDelay() => _ReloadTimeDelay;
}
