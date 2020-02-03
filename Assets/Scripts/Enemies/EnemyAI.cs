/*
 * EnemyAI.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Viewing")]
    [SerializeField] float _ViewDistance;

    [Header("Movement/Rotation")]
    [SerializeField] float _MovementSpeed = 2;
    [SerializeField] float _RotationSpeed = 5;

    Transform _PlayerTransform;
    NavMeshAgent _Agent;

    private void Awake() {
        _Agent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        _PlayerTransform = Globals._MainPlayer.transform;
    }

    private void Update() {
        if (Vector3.Distance(transform.position, _PlayerTransform.position) < _ViewDistance)
        {
            _Agent.speed = _MovementSpeed;
            _Agent.SetDestination(_PlayerTransform.position);
            LookAtPlayer();
        }
    }

    private void LookAtPlayer() {
        Vector3 direction = (_PlayerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _RotationSpeed * Time.deltaTime);
    }
}
