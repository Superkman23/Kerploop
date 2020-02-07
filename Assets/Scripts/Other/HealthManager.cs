/*
 * HealthManager.cs
 * Created by: Kaelan Bartlett
 * Created on: 3/2/2020 (dd/mm/yy)
 * Created for: Managing health
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [SerializeField] public int _MaxHealth;
    [HideInInspector] public int _CurrentHealth;
    [SerializeField] bool _IsPlayer = false;

    private void Awake()
    {
        _CurrentHealth = _MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (_CurrentHealth <= 0)
        {
            if (_IsPlayer)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                return;
            }
            
            //Shortened gun throw script so the enemy drops it's gun
            var cgRB = GetComponentInChildren<Rigidbody>();
            if (cgRB != null)
            {
                cgRB.isKinematic = false;
                cgRB.useGravity = true;
                CF.RecursiveSetColliders(cgRB.transform, true);
            }

            transform.DetachChildren();
            Destroy(gameObject);
        }
    }
}
