/*
 * AmmoBar.cs
 * Created by: Kaelan 
 * Created on: 21/1/2020 (dd/mm/yy)
 * Created for: Displaying the ammobar
 */

using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Player _Player;
    [SerializeField] Image _Bar;
    [SerializeField] Text _CurrentInClip;
    [SerializeField] Text _CurrentAmmoTotal;
    void Update()
    {
        if (_Player._CurrentGun != null)
        {
            _CurrentInClip.text = _Player._CurrentGun._CurrentInClip.ToString();
            _CurrentAmmoTotal.text = _Player._CurrentGun._CurrentAmmoTotal.ToString();

            if (_Player._CurrentGun._IsReloading)
                _Bar.fillAmount = _Player._CurrentGun._ReloadTime - _Player._CurrentGun._ReloadTimeLeft / _Player._CurrentGun._ReloadTime;
            else
                _Bar.fillAmount = _Player._CurrentGun._CurrentInClip / (float)_Player._CurrentGun._ClipSize;
        }
        else
            _Bar.fillAmount = 1;
    }
}