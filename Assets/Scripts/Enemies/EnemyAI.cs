/*
 * EnemyAI.cs
 * Created by: Ambrosia
 * Created on: 2/2/2020 (dd/mm/yy)
 * Created for: Managing the enemy's actions
 */

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Gun Options")]
    [SerializeField] CoreGun _CurrentGun;
    [SerializeField] float _ShootDelay;

    [Header("Viewing")]
    [SerializeField] float _ViewDistance;
    [SerializeField] float _ShootDistance;

    [Header("Movement/Rotation")]
    [SerializeField] float _MovementSpeed = 2;
    [SerializeField] float _RotationSpeed = 5;

    Transform _PlayerTransform;
    NavMeshAgent _Agent;
    bool _CanShoot = true;

    private void Awake()
    {
        _Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _PlayerTransform = Globals._MainPlayer.transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, _PlayerTransform.position);
        if (distance <= _ViewDistance && CanSeePlayer())
        {
            _Agent.speed = _MovementSpeed;
            if (distance > _Agent.stoppingDistance)
                _Agent.SetDestination(_PlayerTransform.position);

            LookAtPlayer();

            if (_CanShoot && distance <= _ShootDistance)
            {
                StartCoroutine(Shoot());
            }
        }
        _CurrentGun.HandlePosition();
    }

    private IEnumerator Shoot()
    {
        _CanShoot = false;
        if (_CurrentGun.GetCurrentClipAmmo() == 0)
        {
            StartCoroutine(_CurrentGun.Reload());
        }
        else
        {
            _CurrentGun.Shoot(transform);
            _CurrentGun.RemoveBulletFromClip(1);
        }

        yield return new WaitForSecondsRealtime(_ShootDelay);

        _CanShoot = true;
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (_PlayerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _RotationSpeed * Time.deltaTime);
    }

    private bool CanSeePlayer()
    {
        if (Physics.Raycast(transform.position,
                            (_PlayerTransform.position - transform.position).normalized,
                            out RaycastHit hit,
                            _ViewDistance))
        {
            return true;
        }
        return false;
    }
}
