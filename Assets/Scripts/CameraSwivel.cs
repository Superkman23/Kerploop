/*
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
	[SerializeField] Vector3 _FirstRotation;
	Quaternion _QFRotation;
	[SerializeField] Vector3 _SecondRotation;
	Quaternion _QSRotation;
	[SerializeField] [Range(0, 1)] float _Speed = 0.9f;
	bool _GoingSecond = false;

	private void Awake()
	{
		_QFRotation = Quaternion.Euler(_FirstRotation);
		_QSRotation = Quaternion.Euler(_SecondRotation);

		transform.rotation = _QFRotation;
	}
	
	private void Update()
	{
		if (_GoingSecond)
		{
			Vector3 lerped = Vector3.Lerp(transform.eulerAngles, _SecondRotation, _Speed / 100);
			transform.rotation = Quaternion.Euler(lerped);

			if (transform.rotation == _QSRotation)
				_GoingSecond = false;
		}
		else
		{
			Vector3 lerped = Vector3.Lerp(transform.eulerAngles, _FirstRotation, _Speed / 100);
			transform.rotation = Quaternion.Euler(lerped);

			if (transform.rotation == _QFRotation)
				_GoingSecond = true;
		}
	}
}
