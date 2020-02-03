/*
 * SpreadShot.cs
 * Created by: Kaelan Bartlett
 * Created on: 2/2/20 (dd/mm/yy)
 * Created for: guns that shoot pellets (Shotgun)
 */

using UnityEngine;

public class SpreadShot : PlayerGun
{
    [SerializeField] float _RigidbodyForce = 5;
    [SerializeField] int _PelletsPerShot = 10;

    public override void Shoot(Transform position)
    {
        // Play the audio of the gun shooting
        _AudioSource.PlayOneShot(_ShootNoise, _ShotVolume);

        RaycastHit hit;
        int pelletsleft = _PelletsPerShot;
        while (pelletsleft-- > 0)
        {
            float spreadX = Random.Range(-_BulletSpread, _BulletSpread);
            float spreadY = Random.Range(-_BulletSpread, _BulletSpread);
            Vector3 spread = new Vector3(spreadX, spreadY, 0);

            Debug.DrawRay(position.position, (position.forward + spread) * _BulletMaxDistance, Color.green, 2);
            if (Physics.Raycast(position.position, position.forward + transform.InverseTransformDirection(spread), out hit, _BulletMaxDistance))
            {
                var hitRB = hit.rigidbody;
                if (hitRB != null)
                {
                    hitRB.AddForce(position.forward * _RigidbodyForce, ForceMode.Impulse);
                }
            }
        }
    }

}
