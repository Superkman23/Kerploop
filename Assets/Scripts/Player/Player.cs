﻿/*
 * Player.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;

public class Player : MonoBehaviour
{
	[HideInInspector] public GameObject _CurrentGun = null;

	private void Awake() 
	{
		Globals._MainPlayer = this;	
	}

	public void DropWeapon()
	{
		if (_CurrentGun == null)
			return;
		 
		_CurrentGun.transform.parent = null;
		var cgRB = _CurrentGun.GetComponent<Rigidbody>();
		cgRB.isKinematic = false;
		CF.RecursiveSetColliders(cgRB.transform, true);
		cgRB.AddForce(transform.forward * 5, ForceMode.Impulse);
	}
}
