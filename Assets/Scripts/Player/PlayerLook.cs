/*
 * PlayerLook.cs
 * Created by: Ambrosia
 * Created on: 20/01/2020 (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _RotationSpeed = 1f;

    [Header("Y-axis Clamping")]
    [SerializeField] float _YRotationMin = -90f;
    [SerializeField] float _YRotationMax = 90f;

    float _XRotation = 0;
    float _YRotation = 0;

    Camera _MainCamera;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _MainCamera = Camera.main;
    }

    private void Awake()
    {
        _XRotation = transform.eulerAngles.y;
    }

    private void Update()
    {
        var lookDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // Don't rotate if player isn't even moving mouse
        if (lookDirection == Vector2.zero)
            return;

        lookDirection *= _RotationSpeed;
        _XRotation += lookDirection.x;
        _YRotation += lookDirection.y;
        ClampRotation();

        _MainCamera.transform.rotation = Quaternion.Euler(-_YRotation, _MainCamera.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(0, _XRotation, 0);
    }

    private void ClampRotation()
    {
        if (_YRotation < _YRotationMin)
            _YRotation = _YRotationMin;
        else if (_YRotation > _YRotationMax)
            _YRotation = _YRotationMax;
    }
}
