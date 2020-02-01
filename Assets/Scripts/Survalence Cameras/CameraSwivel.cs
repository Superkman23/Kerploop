/*
 * CameraSwivel.cs
 * Created by: Kaelan Bartlett
 * Created on: 1/2/2020 (dd/mm/yy)
 * Created for: Rotating the camera's head
 */
using UnityEngine;

public class CameraSwivel : MonoBehaviour
{

    [SerializeField] float _RotationAmount;
    [SerializeField] [Range(0, 1)] float _Speed;
    bool _IsRotatingRight = true;

    Transform _LeftPos;
    Transform _RightPos;


    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
       if(_IsRotatingRight)
       {
            transform.rotation = Quaternion.Lerp(_LeftPos.rotation, _RightPos.rotation, Time.time * _Speed);
       } else {
            transform.rotation = Quaternion.Lerp(_RightPos.rotation, _LeftPos.rotation, Time.time * _Speed);
        }
    }
}
