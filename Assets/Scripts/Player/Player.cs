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


	
    public int _Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int _MaxHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
