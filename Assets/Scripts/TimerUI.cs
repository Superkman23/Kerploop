using UnityEngine.UI;
using UnityEngine;

public class TimerUI : MonoBehaviour
{

    private Text _Text;
    [SerializeField] private float _SecondsPerHour;
    private float _Time;


    void Awake()
    {
        _Text = GetComponent<Text>();
        _Time = _SecondsPerHour * 12;
    }

    // Update is called once per frame
    void Update()
    {

        _Time += Time.deltaTime;
        float timeLeft = _Time;

        int hours = Mathf.FloorToInt(_Time / _SecondsPerHour);
        timeLeft -= Mathf.FloorToInt(hours * _SecondsPerHour);
        int minutes = Mathf.FloorToInt(timeLeft / (_SecondsPerHour / 60));

        if(hours > 12)
        {
            hours -= 12;
        }

        if (minutes < 10)
        {
            _Text.text = hours + ":0" + minutes + " AM";
        } else
        {
            _Text.text = hours + ":" + minutes + " AM";
        }




    }
}
