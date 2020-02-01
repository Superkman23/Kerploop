/*
 * PlayerControls.cs
 * Created by: Ambrosia
 * Created on: 1/2/2020 (dd/mm/yy)
 * Created for: player-specific controls besides basic FPS controls (movement/looking)
 */

using UnityEngine;

public class PlayerControls : MonoBehaviour
{
	// Uncomment these when there are variables in place
	[Header("Components")]
	[SerializeField] Light _EyeLight;
	bool _ELEnabled = true;

	[Header("Settings")]
	[SerializeField] KeyCode _EyeLightToggleButton = KeyCode.F;
	
	private void Awake() 
	{
		_EyeLight.enabled = _ELEnabled;
	}

	private void Update()
	{
		if (Input.GetKeyDown(_EyeLightToggleButton))
		{
			_ELEnabled = !_ELEnabled; // Invert _ELEnabled
		}
	}
}
