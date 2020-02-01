/*
 * CrashSound.cs
 * Created by: Ambrosia
 * Created on: 1/2/2020 (dd/mm/yy)
 * Created for: playing a sound when an object hits another object at speed
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class CrashSound : MonoBehaviour
{
	// Uncomment these when there are variables in place
	[Header("Components")]
	[SerializeField] AudioClip _Noise;

	Rigidbody _Rigidbody;
	AudioSource _Audio;
	
	[Header("Settings")]
	[SerializeField] float _VelocityForNoise = 1.5f;
	[SerializeField] [Range(0, 1)] float _VolumeOfNoise = 0.75f;
	private bool _CanMakeNoise = false;
	
	
	private void Awake()
	{
		_Rigidbody = GetComponent<Rigidbody>();
		_Audio = GetComponent<AudioSource>();
	}
	
	// All physics related functions
	private void FixedUpdate()
	{
		_CanMakeNoise = (_Rigidbody.velocity.magnitude > _VelocityForNoise);
	}

	private void OnCollisionEnter(Collision other)
	{
		if (_CanMakeNoise)
		{
			_Audio.PlayOneShot(_Noise, _VolumeOfNoise);
		}	
	}
}
