/*
 * Pistol.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;

public class Pistol : Gun
{
	public override void Shoot()
	{
		// Play the audio of the gun shooting
        _AudioSource.PlayOneShot(_ShootNoise, _ShootNoiseVolume);

        float spreadX = Random.Range(-_Spread, _Spread);
        float spreadY = Random.Range(-_Spread, _Spread);
        Vector3 spread = new Vector3(spreadX, spreadY, 0);

        Debug.DrawRay(_MainCamera.transform.position, (_MainCamera.transform.forward + spread) * _BulletMaxDistance, Color.green, 2);
        if (Physics.Raycast(
            _MainCamera.transform.position, // Shoots from the main camera
            _MainCamera.transform.forward + transform.InverseTransformDirection(spread),  // Shoots forwards
            out RaycastHit hit, _BulletMaxDistance))
        {
            var hitRB = hit.rigidbody;
            if (hitRB != null)
            {
                hitRB.AddForce(_MainCamera.transform.forward * _RigidbodyForce, ForceMode.Impulse);
            }
        }
	}
}
