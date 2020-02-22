/*
 * Enhancer.cs
 * Created by: Ambrosia
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhancer : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] float _LifeTime = 5;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			
		}
	}
}
