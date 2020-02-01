using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] int _SecondsPerHour = 60;
    [SerializeField] bool _IsAm = false;
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

        if (hours == 12 && minutes == 0 && !_DidChange)
        {
            _IsAm = !_IsAm;
            _DidChange = true;
        }

        _Text.text = hours + ((minutes < 10) ? ":0" : ":") + minutes;
        _Text.text += _IsAm ? " AM" : " PM"; 
    }
}
