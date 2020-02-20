/*
 * HealthPack.cs
 * Created by: Ambrosia
 * Created on: 20/2/2020 (dd/mm/yy)
 * Created for: needing the player to gather health
 */

using UnityEngine;

public class HealthPack : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] int _HealthToGain = 10;
    [SerializeField] AudioClip _HealthPickupNoise;

    public void OnInteractStart(GameObject interactingParent)
    {
        if (interactingParent.CompareTag("Player"))
            Get(interactingParent.GetComponent<HealthManager>());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Get(other.GetComponent<HealthManager>());
    }

    void Get(HealthManager playerHealth)
    {
        if (playerHealth.GetHealth() >= playerHealth._MaxHealth)
            return;

        AudioSource.PlayClipAtPoint(_HealthPickupNoise, transform.position);
        playerHealth.AddHealth(_HealthToGain);
        Destroy(gameObject);
    }
}
