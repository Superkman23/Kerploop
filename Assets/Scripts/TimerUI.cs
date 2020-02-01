using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private float _SecondsPerHour = 60;

    private TextMeshProUGUI _Text;
    private float _Time;

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
            hours -= 12;
        
        if (minutes < 10)
            _Text.text = hours + ":0" + minutes + " AM";
        else
            _Text.text = hours + ":" + minutes + " AM";     
    }
}
