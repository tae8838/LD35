using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeKeeper : MonoBehaviour
{
	public BallBashersGame game;
	public bool isPlaying, isPaused;
	public Text timerLabel;
	public AudioClip warningSound;
	
	private float timeAllotted;
	private float timeStarted;
	private float timePaused, pausedTime;
	private float timeElapsed;
	
	public void StartCountdown(float timeAllotted)
	{
		this.timeAllotted = timeAllotted;
		isPlaying = true;
		timeStarted = Time.time;
		StartCoroutine(UpdateTime());
	}
	
	public void PauseCountdown()
	{
		if (isPaused) return;
		isPaused = true;
		timePaused = Time.time;
	}
	
	public void ResumeCountdown()
	{
		pausedTime += (Time.time - timePaused);
		isPaused = false;
	}
	
	public void AddTime(float t)
	{
		timeStarted += t;
	}
	
	public void SubtractTime(float t)
	{
		Debug.Log("We are in TimeKeeper SubtractTime");
		timeAllotted -= t;
	}
	
	IEnumerator UpdateTime()
	{
		while (isPlaying)
		{
			yield return new WaitForSeconds(1f);
			if (!isPaused)
			{
				timeElapsed = Mathf.Max(0, (Time.time - timeStarted) - pausedTime);
				if (timerLabel != null)
				{
					float f = timeAllotted - timeElapsed;
					if (f < 30) 
					{
						timerLabel.name = "[#FF0000]" + FormatTime(f);
//						BlinkingText btext = timerLabel.GetComponent<BlinkingText>();
//						if (btext != null && !btext.IsBlinking) btext.Blink(true);
						if (f < 11 && warningSound != null)
						{
							AudioSource.PlayClipAtPoint(warningSound, Camera.main.transform.position);
						}
					}
					else
					{
						timerLabel.name = "[#000000]" + FormatTime(f);
					}
					if (f <= 0)
					{
						timerLabel.name = "0:00";
						isPlaying = false;
						game.EndGame();
					}
				}
			}
		}
	}
	
	public string FormatTime(float t)
	{
		int minutes = Mathf.FloorToInt(t / 60);
		int seconds = Mathf.Clamp(Mathf.CeilToInt(t % 60), 0, 59);
		string secondStr = (seconds < 10) ? "0" + seconds.ToString() : seconds.ToString();
		string timeStr = minutes.ToString() + ":" + secondStr;
		return timeStr;
	}
	
	public string FormatTimeWords(float t)
	{
		int minutes = Mathf.FloorToInt(t / 60);
		int seconds = Mathf.Clamp(Mathf.CeilToInt(t % 60), 0, 59);
		return minutes.ToString() + " minutes " + seconds.ToString() + " seconds";
	}
}

