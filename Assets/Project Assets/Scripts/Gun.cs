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

    [Header("Positioning")]
    [SerializeField] Vector3 _DefaultPosition;
    [SerializeField] Vector3 _AimingPosition;
    Vector3 _TargetPosition;


    [Header("Aiming")]
    [SerializeField] [Range(0, 1)] float _AimSpeed;
    [SerializeField] float _AimingAccuracyMultiplier;
    bool _IsAiming;


    [Header("Shooting")]
    [SerializeField] bool _IsAutomatic;

    // Variables that affect the bullet's functions
    [SerializeField] float _BulletSpread;
    [SerializeField] float _BulletMaxDistance;
    [SerializeField] int _BulletDamage;
    [SerializeField] int _BulletForce;
    [SerializeField] int _BulletsPerShot = 1;

    // Variables that are used when limiting how fast a gun can shoot
    [SerializeField] int _ShotDelay;
    int _TimeTillNextShot;


    [Header("Ammo")]
    [SerializeField] int _ClipSize;
    protected int _MaxAmmo; 
    protected int _CurrentInClip;
    protected int _CurrentAmmoTotal;


    [Header("Reloading")]
    [SerializeField] float _ReloadTime;
    WaitForSecondsRealtime _ReloadTimeDelay;
    [HideInInspector] bool _IsReloading;


    [Header("Feedback")]
    // Visual feedback
    [SerializeField] protected float _FlashIntensity;
    [SerializeField] float _ShotRecoil;
    // Audio Feedback
    [SerializeField] [Range(0, 1)] float _ShotVolume;

    void Awake()
    {

        _Rigidbody = GetComponent<Rigidbody>();
        _TargetPosition = _DefaultPosition;

        _CurrentInClip = _ClipSize;
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
        _Rigidbody.isKinematic = true;
        _IsEquipped = true;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
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
        if(isAiming == _IsAiming) // If we're already in the position, don't switch
        {
            return;
        }
        if(isAiming == true)
        {
            _IsAiming = true;
            _BulletSpread /= _AimingAccuracyMultiplier;
            _TargetPosition = _AimingPosition;
        }
        else
        {
            _IsAiming = false;
            _BulletSpread *= _AimingAccuracyMultiplier;
            _TargetPosition = _DefaultPosition;
        }
    }

    public void Shoot()
    {
        float shotsLeft = _BulletsPerShot;

        while (shotsLeft-- > 0)
        {

            float spreadX = Random.Range(-_BulletSpread, _BulletSpread);
            float spreadY = Random.Range(-_BulletSpread, _BulletSpread);

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

        transform.localPosition -= new Vector3(0, 0, _ShotRecoil);
    }

    void CreateBullet(Vector3 point) // Creates the line the gullet follows
    {
        GameObject go = Instantiate(_BulletRay, _BulletSpawnPoint.position, Quaternion.identity);
        BulletRay ray = go.GetComponent<BulletRay>();
        ray.SetRendererPosition(point);
        StartCoroutine(ray.WaitThenDestroy(0.1f));
    }
}
