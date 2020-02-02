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

        if (Physics.Raycast(
            _MainCamera.transform.position, // Shoots from the main camera
            _MainCamera.transform.forward,  // Shoots forwards
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
