﻿/*
 * Enemy AI.cs
 * Created by: Kaelan 
 * Created on: 20/1/2020 (dd/mm/yy)
 * Created for: Basic AI
 */

using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Gun _Gun;
    [SerializeField] Transform _Player;
    [SerializeField] Transform _Eyes;
    HealthManager _Health;
    NavMeshAgent _Agent;

    [Header("Settings")]
    [SerializeField] Color _ViewDistanceColor = Color.cyan;
    [SerializeField] float _ViewDistance;
    [SerializeField] Color _360DistanceColor = Color.red;
    [SerializeField] float _360Distance; // Distance the AI sees you no matter the angle
    [SerializeField] Color _ShootDistanceColor = Color.blue;

    [SerializeField] float _MaxShootDistance;
    [SerializeField] float _ViewAngle;
    [SerializeField] float _ShootAngle;

    [SerializeField] float _RotationSpeed;
    [SerializeField] float _AdditionalShotDelay = 0.5f;
    float _TimeTillNextShot;

    void Awake()
	{
        _Agent = GetComponent<NavMeshAgent>();
        _Health = GetComponent<HealthManager>();
    }

    private void Start()
    {
        _Gun.Pickup(_Eyes.transform);
    }

    void Update()
	{
        if (!_Gun._IsReloading && Physics.Raycast(origin: transform.position,
                                                  direction: (_Player.position - transform.position).normalized,
                                                  hitInfo: out RaycastHit hit,
                                                  maxDistance: _ViewDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Vector3 targetDir = _Player.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);

                if (angle <= _ViewAngle || Vector3.Distance(transform.position, _Player.transform.position) <= _360Distance)
                {
                    _Agent.SetDestination(hit.transform.position);
                    LookAtPlayer();

                    if (hit.distance <= _MaxShootDistance && angle <= _ShootAngle)
                    {
                        if (_Gun._CanShoot && _Gun._CurrentInClip > 0 && _TimeTillNextShot <= 0)
                        {
                            _Gun.Shoot();
                            _TimeTillNextShot = _Gun._ShotDelay + _AdditionalShotDelay;
                        }
                        if (_TimeTillNextShot > 0)
                            _TimeTillNextShot -= Time.deltaTime;
                    }
                }
            }
        }

        if ( _Gun._CurrentInClip <= 0)
        {
            _Gun.StartReloading();
        }

        if (_Health._HurtOrigin != Vector3.zero)
        {
            _Agent.SetDestination(_Health._HurtOrigin);
            _Health._HurtOrigin = Vector3.zero;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = _360DistanceColor;
        Gizmos.DrawWireSphere(transform.position, _360Distance);

        Gizmos.color = _ViewDistanceColor;
        Gizmos.DrawWireSphere(transform.position, _ViewDistance);

        Gizmos.color = _ShootDistanceColor;
        Gizmos.DrawWireSphere(transform.position, _MaxShootDistance);
    }

    void LookAtPlayer()
    {
        Vector3 direction = (_Player.position - transform.position).normalized;
        Quaternion lrRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        Quaternion udRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lrRotation, _RotationSpeed * Time.deltaTime);
        _Eyes.rotation = Quaternion.Slerp(_Eyes.rotation, udRotation, _RotationSpeed * Time.deltaTime);
    }

    public void Die()
    {
        _Gun.Drop();
        Destroy(gameObject);
    }
}
