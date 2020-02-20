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
    [SerializeField] public float _MaxHealth;
    [HideInInspector] public float _CurrentHealth;
    [HideInInspector] public float _HealthPercent;
    [SerializeField] bool _IsPlayer = false;


    private void Awake()
    {
        _CurrentHealth = _MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        _HealthPercent = _CurrentHealth / _MaxHealth;

        if (Input.GetKeyDown(KeyCode.Y))
        {
            _CurrentHealth--;
        }

        //Health is at 0 so the object has died
        if (_CurrentHealth <= 0)
        {
            if (_IsPlayer)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}