/*
 * Gun.cs
 * Created by: Kaelan Bartlett
 * Created on: 10/2/2020 (dd/mm/yy)
 * Created for: Controlling all guns' actions
 */

using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] AudioClip _ShootNoise;
    [SerializeField] Light _Flash;
    [SerializeField] Transform _BulletSpawnPoint;
    [SerializeField] GameObject _BulletRay;
    protected AudioSource _AudioSource;

    [Header("General Gun Settings")]
    [SerializeField] bool _IsAutomatic;
    [SerializeField] float _AimingAccuracyMultiplier;
    [SerializeField] float _BulletSpread;
    [SerializeField] float _BulletMaxDistance;
    [SerializeField] int _BulletDamage;
    bool _IsEquipped;

    [Header("Positioning")]
    [SerializeField] Vector3 _DefaultPosition;
    [SerializeField] Vector3 _AimingPosition;
    Vector3 _TargetPosition;
    [SerializeField] [Range(0, 1)]  float _AimSpeed;
    [SerializeField] float _RecoilAmount;
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

    [HideInInspector] public Rigidbody _Rigidbody;

    void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        _TargetPosition = _DefaultPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (_IsEquipped)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _TargetPosition, _AimSpeed);
        }
    }

    public void Pickup(Transform target) //Runs when the gun is picked up
    {
        Global.RecursiveSetColliders(transform, false);
        transform.parent = target;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        _Rigidbody.isKinematic = true;
        _IsEquipped = true;
    }

    public void Drop()
    {
        Aim(false);
        Global.RecursiveSetColliders(transform, true);
        transform.parent = null;
        _Rigidbody.isKinematic = false;
        _IsEquipped = false;
    }

    public void Aim(bool isAiming)
    {
        if(isAiming == true)
        {
            _IsAiming = true;
            _TargetPosition = _AimingPosition;
        }
        else
        {
            _IsAiming = false;
            _TargetPosition = _DefaultPosition;
        }

    }

    public void Shoot()
    {

    }


}
