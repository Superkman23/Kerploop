/*
 * HealthBar.cs
 * Created by: Kaelan Bartlett
 * Created on: 3/2/2020 (dd/mm/yy)
 * Created for: Drawing a healthbar on screen
 */
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] HealthManager _HealthManager;
    [SerializeField] Image _Bar;

    [Header("Misc")]
    [SerializeField] [Range(0,1)] float _ChangeSpeed;


    // Update is called once per frame
    void Update()
    {
        _Bar.fillAmount = Mathf.Lerp(_Bar.fillAmount, _HealthManager._HealthPercent, _ChangeSpeed);
    }
}
