/*
 * Crosshair.cs
 * Created by: Kaelan Bartlett
 * Created on: 3/2/2020 (dd/mm/yy)
 * Created for: Drawing the crosshair
 */
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Player _Player;
    [SerializeField] RectTransform _Left;
    [SerializeField] RectTransform _Right;
    [SerializeField] RectTransform _Top;
    [SerializeField] RectTransform _Bottom;

    Gun _PlayerGun;


    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _PlayerGun = _Player._CurrentGun.GetComponent<Gun>(); //TODO remove GetComponent from update function

        if (_PlayerGun != null)
        {
            _Left.anchoredPosition = new Vector2(-_PlayerGun._CurrentSpread * 15, 0);
            _Right.anchoredPosition = new Vector2(_PlayerGun._CurrentSpread * 15, 0);
            _Bottom.anchoredPosition = new Vector2(0, -_PlayerGun._CurrentSpread * 15);
            _Top.anchoredPosition = new Vector2(0, _PlayerGun._CurrentSpread * 15);
        }
        else
        {

        }
    }
}
