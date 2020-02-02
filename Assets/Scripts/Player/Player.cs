/*
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
}
