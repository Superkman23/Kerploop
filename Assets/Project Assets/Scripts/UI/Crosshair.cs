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

    GameObject _PlayerGun;
    Gun _Gun;

    void Awake()
    {
        _PlayerGun = _Player._CurrentGun;
    }

    // Update is called once per frame
    void Update()
    {
        if(_PlayerGun != _Player._CurrentGun) //The player's gun has changed, switch to the new gun
        {
            _PlayerGun = _Player._CurrentGun;

            if (_PlayerGun == null)
            {
                _Gun = null;
                _Left.anchoredPosition = new Vector2(-15, 0);
                _Right.anchoredPosition = new Vector2(15, 0);
                _Bottom.anchoredPosition = new Vector2(0, -15);
                _Top.anchoredPosition = new Vector2(0, 15);
            }
            else
            {
                _Gun = _PlayerGun.GetComponent<Gun>();
            }
        }

        if (_PlayerGun != null)
        {
            _Left.anchoredPosition = new Vector2(-_Gun._CurrentSpread * 15, 0);
            _Right.anchoredPosition = new Vector2(_Gun._CurrentSpread * 15, 0);
            _Bottom.anchoredPosition = new Vector2(0, -_Gun._CurrentSpread * 15);
            _Top.anchoredPosition = new Vector2(0, _Gun._CurrentSpread * 15);
        }
    }
}
