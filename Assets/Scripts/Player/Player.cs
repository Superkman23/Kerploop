/*
 * Player.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TextMeshProUGUI _HealthText;
    [SerializeField] TextMeshProUGUI _TotalAmmoText;
    [SerializeField] TextMeshProUGUI _ClipAmmoText;
    Transform _AmmoUIParent = null;
    List<CanvasRenderer> _HealthDisplayObjs = new List<CanvasRenderer>();

    [Header("Throwing Weapon")]
    [SerializeField] KeyCode _ThrowButton = KeyCode.F;
    [SerializeField] float _ThrowForce = 5;

    GameObject _CurrentGun = null;
    CoreGun _CurrentGunComponent = null;

    Camera _MainCamera;
    Rigidbody _Rigidbody;
    HealthManager _PlayerHealth;


    private void Awake()
    {
        Globals._MainPlayer = this;
        _PlayerHealth = GetComponent<HealthManager>();
        _MainCamera = Camera.main;
        _AmmoUIParent = _ClipAmmoText.transform.parent;
        
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
        {
            if (_AmmoUIParent.gameObject.activeInHierarchy)
                _AmmoUIParent.gameObject.SetActive(false);
            return;
        }

        if (!_AmmoUIParent.gameObject.activeInHierarchy)
            _AmmoUIParent.gameObject.SetActive(true);
        
        _TotalAmmoText.text = _CurrentGunComponent.GetCurrentTotalAmmo().ToString();
        _ClipAmmoText.text = _CurrentGunComponent.GetCurrentClipAmmo().ToString();
    }

    private void DisplayHealth()
    {
        foreach (var obj in _HealthDisplayObjs)
            obj.SetColor(Color.Lerp(Color.red, Color.green, (float)_PlayerHealth._CurrentHealth / _PlayerHealth._MaxHealth));
        _HealthText.text = _PlayerHealth._CurrentHealth.ToString();
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
