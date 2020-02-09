using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] HealthManager _HealthManager;
    [SerializeField] RectTransform _Bar;
    Vector2 _FullSize;

    [Header("Misc")]
    [SerializeField] [Range(0,1)] float _ChangeSpeed;


    void Awake()
    {
        _FullSize = _Bar.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 targetSize = new Vector2(_FullSize.x * _HealthManager._HealthPercent, _FullSize.y);
        Debug.Log(targetSize);
        _Bar.sizeDelta = Vector2.Lerp(_Bar.sizeDelta, targetSize, _ChangeSpeed);
        _Bar.anchoredPosition = new Vector2((_Bar.sizeDelta.x - _FullSize.x) / 2, 0);
    }
}
