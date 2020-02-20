/*
 * Gun.cs
 * Created by: Kaelan Bartlett
 * Created on: 10/2/2020 (dd/mm/yy)
 * Created for: Controlling all guns' actions
 */

using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] AudioClip _ShootNoise;
    [SerializeField] Light _Flash;
    [SerializeField] Transform _BulletSpawnPoint;
    [SerializeField] GameObject _BulletRay;
    AudioSource _AudioSource;
    [HideInInspector] public Rigidbody _Rigidbody;

    //Variables that don't fit any category
    bool _IsEquipped;
    [HideInInspector] public bool _CanShoot;


    [Header("Positioning")]
    [SerializeField] Vector3 _DefaultPosition;
    [SerializeField] Vector3 _AimingPosition;
    Vector3 _TargetPosition;


    [Header("Aiming")]
    [SerializeField] [Range(0, 1)] float _AimSpeed;
    bool _IsAiming;

    [Header("Spread")]
    [SerializeField] float _DefaultSpread; // Starting Spread (Not aiming)
    [SerializeField] float _AimingSpread; // The gun's spread while aiming
    [SerializeField] [Range(0, 1)] float _DefaultSpreadTime; // How quickly the gun returns to its target spread
    [SerializeField] [Range(0, 1)] float _AimingSpreadTime; // How quickly the gun returns to its target spread while aiming
    [SerializeField] float _SpreadPerShot; // How much the spread increases by per shot
    float _TargetSpread; // Spread the gun is trying to reach
    [HideInInspector] public float _CurrentSpread; // The spread the gun fires with

    [Header("Shooting")]
    public bool _IsAutomatic;

    // Variables that affect the bullet's functions
    [SerializeField] float _BulletMaxDistance;
    [SerializeField] int _BulletDamage;
    [SerializeField] int _BulletForce;
    [SerializeField] int _BulletsPerShot = 1;

    // Variables that are used when limiting how fast a gun can shoot
    [SerializeField] float _ShotDelay; // Minimum amount of time between shots
    float _TimeTillNextShot;


    [Header("Ammo")]
    [SerializeField] int _ClipSize; // How much ammo can be used before needing to reload
    [SerializeField] int _MaxAmmo;  // The maximum amount of ammo that can be held by the gun
    [HideInInspector] public int _CurrentInClip;
    int _CurrentAmmoTotal;


    [Header("Reloading")]
    [SerializeField] float _ReloadTime;
    float _ReloadTimeLeft;
    bool _IsReloading;


    [Header("Feedback")]
    // Visual feedback
    [SerializeField] float _ShotRecoil;
    // Audio Feedback
    [SerializeField] [Range(0, 1)] float _ShotVolume;

    void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        Aim(false); // Sets default position and spread
        _CurrentInClip = _ClipSize;
        _CurrentAmmoTotal = _MaxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (_IsEquipped)
            transform.localPosition = Vector3.Lerp(transform.localPosition, _TargetPosition, _AimSpeed);

        if (_TimeTillNextShot > 0)
            _TimeTillNextShot -= Time.deltaTime;
        else
            _TimeTillNextShot = 0;

        _CanShoot = (_IsReloading || _TimeTillNextShot > 0) ? false : true; // Determine whether this gun can shoot

        if (_ReloadTimeLeft <= 0)
        {
            if (_IsReloading) // Ensure the gun is still reloading
                FinishReloading();

            _ReloadTimeLeft = 0;
        }
        else
            _ReloadTimeLeft -= Time.deltaTime;
    }
    void FixedUpdate()
    {
        if (_IsEquipped)
            _CurrentSpread = (Mathf.Lerp(_CurrentSpread, _TargetSpread, (_IsAiming) ? _AimingSpreadTime : _DefaultSpreadTime ));
    }

    public void Pickup(Transform target) //Runs when the gun is picked up
    {
        Global.RecursiveSetColliders(transform, false);
        transform.parent = target;
        _Rigidbody.isKinematic = true;
        _IsEquipped = true;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    public void Drop()
    {
        Aim(false);
        _IsReloading = false; // Interupt reload process
        _CurrentSpread = 0;
        Global.RecursiveSetColliders(transform, true);
        transform.parent = null;
        _Rigidbody.isKinematic = false;
        _IsEquipped = false;
    }
    public void Aim(bool isAiming)
    {
        if(isAiming)
        {
            _IsAiming = true;
            _TargetSpread = _AimingSpread;
            _TargetPosition = _AimingPosition;
        }
        else
        {
            _IsAiming = false;
            _TargetSpread = _DefaultSpread;
            _TargetPosition = _DefaultPosition;
        }
    }
    public void Shoot()
    {
        float shotsLeft = _BulletsPerShot;

        while (shotsLeft-- > 0)
        {

            float spreadX = Random.Range(-_CurrentSpread, _CurrentSpread);
            float spreadY = Random.Range(-_CurrentSpread, _CurrentSpread);

            Vector3 spread = new Vector3(spreadX, spreadY, 0);
            Vector3 direction = _BulletSpawnPoint.forward + (transform.InverseTransformDirection(spread) / _BulletMaxDistance);

            if (Physics.Raycast(_BulletSpawnPoint.position, direction, out RaycastHit hit, _BulletMaxDistance))
            {
                CreateBullet(hit.point);
                var targetComponent = hit.rigidbody;
                if (targetComponent != null)
                    hit.rigidbody.AddForce(_BulletSpawnPoint.forward * _BulletForce, ForceMode.Impulse);
            }
            else
            {
                CreateBullet(transform.position + (direction.normalized * _BulletMaxDistance));
            }
        }

        _CurrentInClip--;
        _CurrentSpread += _SpreadPerShot;
        _TimeTillNextShot = _ShotDelay;
        transform.localPosition -= new Vector3(0, 0, _ShotRecoil);
    }
    void CreateBullet(Vector3 point) // Creates the line the bullet follows
    {
        GameObject go = Instantiate(_BulletRay, _BulletSpawnPoint.position, Quaternion.identity);
        BulletRay ray = go.GetComponent<BulletRay>();
        ray.SetRendererPosition(point);
        StartCoroutine(ray.WaitThenDestroy(0.1f));
    }
    public void StartReloading()
    {
        if (!_IsReloading) // Can't start if you're already reloading
        {
            _IsReloading = true;
            _ReloadTimeLeft = _ReloadTime;
        }
    }
    void FinishReloading()
    {
        int fillPotential = _ClipSize - _CurrentInClip; // Max amount of ammo that can be filled
        int fillActual = (_CurrentAmmoTotal >= fillPotential) ? fillPotential : _CurrentAmmoTotal; // Actual amount of ammo filled

        _CurrentInClip += fillActual;
        _CurrentAmmoTotal -= fillActual;

        _IsReloading = false;
    }
}
