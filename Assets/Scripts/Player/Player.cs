/*
 * Player.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;

public class Player : MonoBehaviour, IHealth
{
	[Header("Settings")]
	[SerializeField] KeyCode _DropButton = KeyCode.F;
	[SerializeField] float _ThrowForce = 5;

	Camera _MainCamera;
	[HideInInspector] public GameObject _CurrentGun = null;


	int _LocalHealth = 0;
    public int _Health 
	{ 
		get => _LocalHealth; 
		set { if (value > _LocalMaxHealth) value = _LocalMaxHealth;
			_LocalHealth = value; }
	}
    int _LocalMaxHealth = 0;
	public int _MaxHealth { get => _LocalMaxHealth; set => _LocalMaxHealth = value; }

    private void Awake() 
	{
		_MainCamera = Camera.main;
		Globals._MainPlayer = this;
	}

	private void Update() 
	{
		if (Input.GetKeyDown(_DropButton))
		{
			DropWeapon();
		}
	}

	public void DropWeapon()
	{
		if (_CurrentGun == null)
			return;
		 
		_CurrentGun.transform.parent = null;
		var cgRB = _CurrentGun.GetComponent<Rigidbody>();
		cgRB.isKinematic = false;
		CF.RecursiveSetColliders(cgRB.transform, true);
		cgRB.AddForce(_MainCamera.transform.forward * _ThrowForce, ForceMode.Impulse);
		_CurrentGun = null;
	}
}
