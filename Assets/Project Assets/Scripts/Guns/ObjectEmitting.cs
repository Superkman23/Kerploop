/*
 * ObjectEmitting.cs
 * Created by: Kaelan 
 * Created on: 22/2/2020 (dd/mm/yy)
 * Created for: Guns that shoot physical bullets
 */

using UnityEngine;

public class ObjectEmitting : Gun
{
    [Header("Object Emitting Components")]
    [SerializeField] GameObject _Bullet;

    public override void Shoot()
    {
        float spreadX = Random.Range(-_CurrentSpread, _CurrentSpread);
        float spreadY = Random.Range(-_CurrentSpread, _CurrentSpread);

        Vector3 spread = new Vector3(spreadX, spreadY);

        Quaternion targetRotation = Quaternion.Euler(spreadX + transform.rotation.x, spreadY + transform.rotation.y, transform.rotation.z);

        Instantiate(_Bullet, _BulletSpawnPoint.position, targetRotation);

        _AudioSource.PlayOneShot(_ShootNoise, _ShotVolume);

        _CurrentInClip--;
        _CurrentSpread += _SpreadPerShot;

        _TimeTillNextShot = _ShotDelay;
        transform.localPosition += Vector3.back * _ShotRecoil;
    }
}
