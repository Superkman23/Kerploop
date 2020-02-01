﻿/*
 * CameraSwivel.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;

public class CameraSwivel : MonoBehaviour
{
	// Uncomment these when there are variables in place
	//[Header("Components")]
	
	
	[Header("Settings")]
	[SerializeField] Vector3 _Rotation1;
	[SerializeField] Vector3 _Rotation2;
	Quaternion _RotQuat1;
	Quaternion _RotQuat2;
	[SerializeField] float _Speed = 0.9f;
	bool _GoingSecond = false;
	float _Time;

	private void Awake()
	{
		_RotQuat1 = Quaternion.Euler(_Rotation1);
		_RotQuat2 = Quaternion.Euler(_Rotation2);

		transform.rotation = _RotQuat1;
	}
	
	private void Update()
	{
		if (_GoingSecond) // 1 -> 2
		{
			transform.rotation = Quaternion.Slerp(_RotQuat1, _RotQuat2, _Time);

			if (transform.rotation == _RotQuat2)
			{
				_GoingSecond = false;
				_Time = 0;
			}
		}
		else // 2 -> 1
		{
			transform.rotation = Quaternion.Slerp(_RotQuat2, _RotQuat1, _Time);

			if (transform.rotation == _RotQuat1)
			{
				_GoingSecond = true;
				_Time = 0;
			}
		}
		_Time += Time.deltaTime * _Speed;
	}
}
