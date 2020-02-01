using UnityEngine.UI;
using UnityEngine;

public class TimerUI : MonoBehaviour
{

    private Text _Text;
    [SerializeField] private float _SecondsPerMinute;
    private float _Time;



    void Awake()
    {
        _Text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _Time += Time.deltaTime;
        int hours = Mathf.FloorToInt(_Time / (_SecondsPerMinute * 60));
        int minutes = Mathf.FloorToInt((_Time - hours) / _SecondsPerMinute); 

        _Text.text = hours + ":" + minutes + "AM";

    }
}
