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
	List<CanvasRenderer> _HealthDisplayBackground = new List<CanvasRenderer>();

	[Header("Settings")]
	[SerializeField] KeyCode _DropButton = KeyCode.F;
	[SerializeField] float _ThrowForce = 5;
    [SerializeField] int _LocalMaxHealth = 100;
	[SerializeField] int _LocalHealth = 0;

	Camera _MainCamera;
    Rigidbody _Rigidbody;
    [HideInInspector] public GameObject _CurrentGun = null;

    public int _Health 
	{ 
		get => _LocalHealth; 
		set { if (value > _LocalMaxHealth) value = _LocalMaxHealth;
			_LocalHealth = value; }
	}
	public int _MaxHealth { get => _LocalMaxHealth; set => _LocalMaxHealth = value; }

    private void Awake() 
	{
		_LocalHealth = _LocalMaxHealth;

		_MainCamera = Camera.main;
        _Rigidbody = GetComponent<Rigidbody>();
        Globals._MainPlayer = this;

		foreach (Transform child in _HealthText.transform)
		{
			var renderer = child.GetComponent<CanvasRenderer>();
			if (renderer != null)
			{
				_HealthDisplayBackground.Add(renderer);
			}
		}
	}

	private void Update() 
	{
		if (Input.GetKeyDown(_DropButton))
		{
			DropWeapon();
		} 

		foreach (var backgroundObj in _HealthDisplayBackground)
		{
			backgroundObj.SetColor(Color.Lerp(Color.red, Color.green, (float)_Health / _MaxHealth));
		}
		_HealthText.text = _Health.ToString();
	}

	public void DropWeapon()
	{
		if (_CurrentGun == null)
			return;
		 
		_CurrentGun.transform.parent = null;

		var cgG = _CurrentGun.GetComponent<Gun>();
        cgG._IsGunEquipped = false;
        cgG._Thrown = true;

        var cgRB = _CurrentGun.GetComponent<Rigidbody>();
		cgRB.isKinematic = false;
		CF.RecursiveSetColliders(cgRB.transform, true);
		cgRB.AddForce(_MainCamera.transform.forward * _ThrowForce + _Rigidbody.velocity, ForceMode.Impulse);
		
		_CurrentGun = null;
	}
}
