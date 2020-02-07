/*
 * Pistol.cs
 * Created by: Ambrosia, Kaelan
 * Created on: 2/2/2020 (dd/mm/yy)
 * Created for: Guns that shoot one bullet at a time
 */

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
        Vector3 direction = position.forward + transform.InverseTransformDirection(spread);

        if (Physics.Raycast(
            position.position, // Shoots from the main camera
            direction,  // Shoots forwards
            out RaycastHit hit, _BulletMaxDistance))
        {
            CreateTracer(hit.point);

            var hitRB = hit.rigidbody;
            var hitGO = hit.collider.gameObject.GetComponent<HealthManager>();

            if (hitGO != null)
            {
                hitGO._CurrentHealth -= _BulletDamage;
            }
            else if (hitRB != null)
            {
                hitRB.AddForce(position.forward * _RigidbodyForce, ForceMode.Impulse);
            }
        }
        else
        {
            CreateTracer(position.position + (direction.normalized * _BulletMaxDistance));
        }
        transform.localPosition -= new Vector3(0, 0,_RecoilAmount);
    }
}
