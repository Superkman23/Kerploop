/*
 * Player.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Player : MonoBehaviour, IHealth
{
    [Header("Components")]
    [SerializeField] TextMeshProUGUI _HealthText;
    [SerializeField] TextMeshProUGUI _TotalAmmoText;
    [SerializeField] TextMeshProUGUI _ClipAmmoText;
    List<CanvasRenderer> _HealthDisplayObjs = new List<CanvasRenderer>();

    [Header("Throwing Weapon")]
    [SerializeField] KeyCode _ThrowButton = KeyCode.F;
    [SerializeField] float _ThrowForce = 5;

    [Header("Health")]
    [SerializeField] int _LocalMaxHealth = 100;
    [SerializeField] int _LocalHealth = 0;

    GameObject _CurrentGun = null;
    CoreGun _CurrentGunComponent = null;

    Camera _MainCamera;
    Rigidbody _Rigidbody;

    public int _Health
    {
        get => _LocalHealth;
        set
        {
            if (value > _LocalMaxHealth) value = _LocalMaxHealth;
            _LocalHealth = value;
        }
    }
    public int _MaxHealth { get => _LocalMaxHealth; set => _LocalMaxHealth = value; }

    private void Awake()
    {
        Globals._MainPlayer = this;

        _LocalHealth = _LocalMaxHealth;
        _MainCamera = Camera.main;
        
        _Rigidbody = GetComponent<Rigidbody>();

        foreach (Transform child in _HealthText.transform)
        {
            var renderer = child.GetComponent<CanvasRenderer>();
            if (renderer != null)            
                _HealthDisplayObjs.Add(renderer);            
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(_ThrowButton))        
            ThrowWeapon();

        DisplayAmmo();
        DisplayHealth();
    }

    private void DisplayAmmo()
    {
        if (_CurrentGun == null)
            return;
        
        _TotalAmmoText.text = _CurrentGunComponent.GetCurrentTotalAmmo().ToString();
        _ClipAmmoText.text = _CurrentGunComponent.GetCurrentClipAmmo().ToString();
    }

    private void DisplayHealth()
    {
        foreach (var obj in _HealthDisplayObjs)        
            obj.SetColor(Color.Lerp(Color.red, Color.green, (float)_Health / _MaxHealth));        
        _HealthText.text = _Health.ToString();
    }

    // Weapon specific functions
    public void ThrowWeapon()
    {
        if (_CurrentGun == null)
            return;

        var cgG = _CurrentGun.GetComponent<PlayerGun>();

        if (cgG._IsReloading) // Must finish reloading first
            return;

        cgG._IsEquipped = false;
        cgG._GoingToThrow = true;

        _CurrentGun.transform.parent = null;

        var cgRB = _CurrentGun.GetComponent<Rigidbody>();
        cgRB.isKinematic = false;
        CF.RecursiveSetColliders(cgRB.transform, true);
        cgRB.AddForce(_MainCamera.transform.forward * _ThrowForce + _Rigidbody.velocity, ForceMode.Impulse);

        _CurrentGun = null;
        _CurrentGunComponent = null;
    }
    public void SetWeapon(CoreGun setTo)
    {
        _CurrentGunComponent = setTo;
        _CurrentGun = setTo.gameObject;
    }
    public GameObject GetWeapon() => _CurrentGun;
}
