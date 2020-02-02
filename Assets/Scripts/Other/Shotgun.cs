/*
 * Shotgun.cs
 * Created by: Kaelan Bartlett
 * Created on: 2/2/20 (dd/mm/yy)
 * Created for: Shotguns
 */

using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] int _PelletsPerShot = 10;

	public override void Shoot()
	{
        // Play the audio of the gun shooting
        _AudioSource.PlayOneShot(_ShootNoise, _ShootNoiseVolume);

        RaycastHit hit;
        int pelletsleft = _PelletsPerShot;
        while (pelletsleft > 0)
        {
            float spreadX = Random.Range(-_Spread, _Spread);
            float spreadY = Random.Range(-_Spread, _Spread);
            Vector3 spread = new Vector3(spreadX, spreadY, 0);

            Debug.DrawRay(_MainCamera.transform.position, (_MainCamera.transform.forward + spread) * _BulletMaxDistance, Color.green,2);
            if (Physics.Raycast(_MainCamera.transform.position, _MainCamera.transform.forward + transform.InverseTransformDirection(spread), out hit, _BulletMaxDistance))
            {
                var hitRB = hit.rigidbody;
                if (hitRB != null)
                {
                    hitRB.AddForce(_MainCamera.transform.forward * _RigidbodyForce, ForceMode.Impulse);
                }
            }
            pelletsleft--;
        }
    }

}
