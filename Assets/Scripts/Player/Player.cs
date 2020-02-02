/*
 * Player.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;

public class Player : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] KeyCode _DropButton = KeyCode.F;

	Camera _MainCamera;
	[HideInInspector] public GameObject _CurrentGun = null;	

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
		cgRB.AddForce(_MainCamera.transform.forward * 5, ForceMode.Impulse);
		_CurrentGun = null;
	}
}
