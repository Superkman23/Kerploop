/*
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
    [SerializeField] GameObject _Player;
    [SerializeField] GameObject _Eyes;
    NavMeshAgent _Agent;

    [Header("Settings")]
    [SerializeField] float _ViewDistance;
    [SerializeField] float _MaxShootDistance;
    [SerializeField] float _RotationSpeed;
    [SerializeField] float _ShotDelay; // To prevent the AI from spamming non automatic guns 
    float _TimeTillNextShot;

    Vector3 _TargetPosition;

    void Awake()
	{
        _Agent = GetComponent<NavMeshAgent>();
        _TargetPosition = transform.position;
    }

    private void Start()
    {
        _Gun.Pickup(_Eyes.transform);
    }

    void Update()
	{
        if(Vector3.Distance(transform.position, _Player.transform.position) <= _ViewDistance)
        {
            _TargetPosition = _Player.transform.position;
            _Agent.SetDestination(_TargetPosition);
            LookAtPlayer();
        }

        if(Vector3.Distance(transform.position, _Player.transform.position) <= _MaxShootDistance)
        {
            if (_Gun._CanShoot && _Gun._CurrentInClip > 0 && _TimeTillNextShot <= 0)
            {
                _Gun.Shoot();
                _TimeTillNextShot = _ShotDelay;
            }
            if (_TimeTillNextShot > 0)
                _TimeTillNextShot -= Time.deltaTime;
        }

        if ( _Gun._CurrentInClip <= 0)
        {
            _Gun.StartReloading();
        }


    }
    private void LookAtPlayer()
    {
        Vector3 direction = (_Player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _RotationSpeed * Time.deltaTime);
    }

    public void OnDestroy()
    {
        _Gun.Drop();
    }



}
