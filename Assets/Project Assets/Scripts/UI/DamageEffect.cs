/*
 * DamageEffect.cs
 * Created by: Kaelan 
 * Created on: 23/2/2020 (dd/mm/yy)
 * Created for: Tinting the screen red upon taking damage
 */

using UnityEngine;
using UnityEngine.UI;
public class DamageEffect : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] HealthManager _Manager;
    Image _Image;

    [Header("Settings")]
    [SerializeField] float _EffectStrength;
    [SerializeField] float _ChangeSpeed;
    public float _LastHealth;

	private void Awake()
	{
        _Image = GetComponent<Image>();
        _Image.color = new Color(_Image.color.r, _Image.color.g, _Image.color.b, 0);
    }
    private void Start()
    {
        _LastHealth = _Manager.GetHealth();
    }

    private void LateUpdate()
	{
        float targetOpacity = _Manager.GetHealth() < _LastHealth ? 
            (_LastHealth - _Manager.GetHealth()) * _EffectStrength : 
            Mathf.Lerp(_Image.color.a, 0, _ChangeSpeed);

        _LastHealth = _Manager.GetHealth();
        _Image.color = new Color(_Image.color.r, _Image.color.g, _Image.color.b, targetOpacity);
    }
}
