/*
 * Pistol.cs
 * Created by: Ambrosia, Kaelan
 * Created on: 2/2/2020 (dd/mm/yy)
 * Created for: guns that shoot 1 bullet at a time
 */

using System.Collections;
using UnityEngine;

public class SingleShot : PlayerGun
{
    [SerializeField] float _RigidbodyForce = 5f;

    public override void Shoot(Transform position)
    {
        // Play the audio of the gun shooting
        _AudioSource.PlayOneShot(_ShootNoise, _ShotVolume);

        float spreadX = Random.Range(-_BulletSpread, _BulletSpread);
        float spreadY = Random.Range(-_BulletSpread, _BulletSpread);
        Vector3 spread = new Vector3(spreadX, spreadY, 0);

        if (Physics.Raycast(
            position.position, // Shoots from the main camera
            position.forward + transform.InverseTransformDirection(spread),  // Shoots forwards
            out RaycastHit hit, _BulletMaxDistance))
        {
            StartCoroutine(DrawLineTo(hit.point));

            var hitRB = hit.rigidbody;
            if (hitRB != null)
            {
                hitRB.AddForce(position.forward * _RigidbodyForce, ForceMode.Impulse);
            }
            var hitGO = hit.collider.gameObject.GetComponent<HealthManager>();
            if (hitGO != null)
            {
                hitGO._CurrentHealth -= _BulletDamage;
            }
        }
        else
        {
            StartCoroutine(DrawLineTo((position.forward + transform.InverseTransformDirection(spread)) * 100));
        }
    }
}
