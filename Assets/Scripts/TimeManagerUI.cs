using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManagerUI {

    Text minutesText;
    Text secondsText;

    public TimeManagerUI(Text minutes, Text seconds)
    {
        minutesText = minutes;
        secondsText = seconds;
    }

    void SplitTime(int time, out int minutes, out int seconds )
    {
        minutes = time / 60;
        seconds = time - minutes * 60;
    }

	public void UpdateTime (int totalTime) {
        int minutes, seconds;
        SplitTime(totalTime, out minutes, out seconds);
        minutesText.text = minutes.ToString();
        secondsText.text = seconds.ToString();
	}
}
