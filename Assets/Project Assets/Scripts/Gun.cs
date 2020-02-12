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
    bool _IsEquipped;

    [Header("Positioning")]
    [SerializeField] Vector3 _DefaultPosition;
    [SerializeField] Vector3 _AimingPosition;
    Vector3 _TargetPosition;
    [SerializeField] [Range(0, 1)]  float _AimSpeed;
    [SerializeField] float _RecoilAmount;
    protected bool _IsAiming;

    [Header("Shooting")]
    [SerializeField] int _BulletDamage;
    [SerializeField] int _BulletForce;
    [SerializeField] [Range(0, 1)] protected float _ShotVolume;
    [SerializeField] protected int _ShotDelay;
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
        float spreadX = Random.Range(-_BulletSpread, _BulletSpread);
        float spreadY = Random.Range(-_BulletSpread, _BulletSpread);

        Vector3 spread = new Vector3(spreadX, spreadY, 0);
        Vector3 direction = _BulletSpawnPoint.forward + (transform.InverseTransformDirection(spread) / _BulletMaxDistance);

        transform.localPosition -= new Vector3(0, 0, _RecoilAmount);

        if (Physics.Raycast(_BulletSpawnPoint.position, direction, out RaycastHit hit, _BulletMaxDistance))
        {
            CreateTracer(hit.point);
            var targetComponent = hit.rigidbody;
            if(targetComponent != null)
                hit.rigidbody.AddForce(_BulletSpawnPoint.forward * _BulletForce, ForceMode.Impulse);
        }
        else
        {
            CreateTracer(transform.position + (direction.normalized * _BulletMaxDistance));
        }
    }

    void CreateTracer(Vector3 point)
    {
        GameObject go = Instantiate(_BulletRay, _BulletSpawnPoint.position, Quaternion.identity);
        BulletRay ray = go.GetComponent<BulletRay>();
        ray.SetRendererPosition(point);
        StartCoroutine(ray.WaitThenDestroy(0.1f));
    }
}
