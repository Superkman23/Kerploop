/*
 * HealthPack.cs
 * Created by: Ambrosia
 * Created on: 20/2/2020 (dd/mm/yy)
 * Created for: needing the player to gather health
 */

using UnityEngine;

public class HealthPack : Carriable
{
    [Header("Settings")]
    [SerializeField] int _HealthToGain = 10;
    [SerializeField] AudioClip _HealthPickupNoise;

    protected override void Update()
    {
        base.Update();
    }

    void Get(HealthManager playerHealth)
    {
        if (playerHealth.GetHealth() >= playerHealth._MaxHealth)
            return;

        AudioSource.PlayClipAtPoint(_HealthPickupNoise, transform.position);
        playerHealth.AddHealth(_HealthToGain);
        Destroy(gameObject);
    }

    public override void Drop()
    {
        Global.RecursiveSetColliders(transform, true);
        transform.parent = null;
    }
    public override void UseOne(int type)
    {
        throw new System.NotImplementedException();
    }
    public override void UseTwo(int type)
    {
        throw new System.NotImplementedException();
    }
    public override void UseThree(int type)
    {
        throw new System.NotImplementedException();
    }

}
