using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] float _SecondsPerHour = 60;
    [SerializeField] bool _IsAm;
    bool _DidChange;

    TextMeshProUGUI _Text;
    float _Time;

    private void Awake()
    {
        _Text = GetComponent<TextMeshProUGUI>();
        _Time = _SecondsPerHour * 12;
    }

    private void Update()
    {
        _Time += Time.deltaTime;
        float timeLeft = _Time;

        int hours = Mathf.FloorToInt(_Time / _SecondsPerHour);
        timeLeft -= Mathf.FloorToInt(hours * _SecondsPerHour);
        int minutes = Mathf.FloorToInt(timeLeft / (_SecondsPerHour / 60));

        if (hours > 12)
        {
            _DidChange = false;
            _Time -= 12 * _SecondsPerHour;
        }

        if(hours == 12 && minutes == 0 && !_DidChange)
        {
            if (_IsAm)
                _IsAm = false;
            else
                _IsAm = true;

            _DidChange = true;
        }

        if (minutes < 10)
            _Text.text = hours + ":0" + minutes;
        else
            _Text.text = hours + ":" + minutes;

        _Text.text += _IsAm ? " AM" : " PM"; 
    }
}
