using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] int _IRLSecondsPerHour = 60;
    [SerializeField] int _StartingHour = 12;

    [SerializeField] bool _IsAm = false;
    bool _DidChange = true;

    TextMeshProUGUI _Text;
    float _Time;

    private void Awake()
    {
        _Text = GetComponent<TextMeshProUGUI>();
        //Starts the game at the specified hour in the day
        _Time = _IRLSecondsPerHour * _StartingHour;
    }

    private void Update()
    {
        _Time += Time.deltaTime;
        float timeLeft = _Time;

        //Uses total time to calculate hours
        int hours = Mathf.FloorToInt(_Time / _IRLSecondsPerHour);
        //Removes the hours to only use the extra time
        timeLeft -= Mathf.FloorToInt(hours * _IRLSecondsPerHour);
        //Uses the remaining time to calculate the minutes
        int minutes = Mathf.FloorToInt(timeLeft / (_IRLSecondsPerHour / 60));

        //Set's the hours to 1 when there are more than 12 hours (12:59 AM to 1:00 AM)
        if (hours > 12) 
        {
            _DidChange = false; //Prepares for the next AM to PM swap
            _Time -= 12 * _IRLSecondsPerHour; //Removes the extra 12 hours
        }


        //Swaps from AM to PM when the time reaches 12:00 (11:59 AM to 12:00 PM)
        if (hours == 12 && minutes == 0 && !_DidChange)
        {
            _IsAm = !_IsAm; //Switches from AM to PM
            _DidChange = true; //Makes the if statement only trigger once each time it reaches 12
        }

        //Writes the text on the UI
        _Text.text = hours + ((minutes < 10) ? ":0" : ":") + minutes;
        _Text.text += _IsAm ? " AM" : " PM"; 
    }
}
