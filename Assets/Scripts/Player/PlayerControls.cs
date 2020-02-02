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
    //[Header("Components")]
    Camera _MainCamera;

    [Header("Settings")]
    [SerializeField] KeyCode _InteractKey = KeyCode.E;
    [SerializeField] float _InteractDistance = 1f;

    [HideInInspector] public bool _HasEquipped;

    private void Awake()
    {
        _MainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_InteractKey))
        {
            if (Physics.Raycast(_MainCamera.transform.position, _MainCamera.transform.forward, out RaycastHit hitInfo, _InteractDistance))
            {
                var interactable = hitInfo.transform.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.OnInteractStart(gameObject);
                }
            }
        }
    }
}
