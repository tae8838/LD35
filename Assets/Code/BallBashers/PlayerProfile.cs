using UnityEngine;
using System.Collections;

public class PlayerProfile
{
	private int highScore;
	private string topWordString;
	private int topWordValue;
	private string lastPlayedWord;
	
	public static string highScoreKey = "_WNHighScore";
	public static string topWordStringKey = "_WNTopWordString";
	public static string topWordValueKey = "_WNTopWordValue";
	public static string lastWordKey = "_WNLastWord";
	
	public void LoadProfile()
	{
		highScore = PlayerPrefs.GetInt(highScoreKey, 0);
		topWordString = PlayerPrefs.GetString(topWordStringKey, "");
		topWordValue = PlayerPrefs.GetInt(topWordValueKey, 0);
		lastPlayedWord = PlayerPrefs.GetString(lastWordKey, "");
	}
	
	public int HighScore
	{
		get 
		{
			return highScore;
		}
		set
		{
			highScore = value;
			PlayerPrefs.SetInt(highScoreKey, highScore);
		}
	}
	
	public string TopWord
	{
		get 
		{
			return topWordString;
		}
		set
		{
			topWordString = value;
			PlayerPrefs.SetString(topWordStringKey, topWordString);
		}
	}
	
	public int TopWordValue
	{
		get 
		{
			return topWordValue;
		}
		set
		{
			topWordValue = value;
			PlayerPrefs.SetInt(topWordValueKey, topWordValue);
		}
	}
	
	public string LastPlayedWord
	{
		get 
		{
			return lastPlayedWord;
		}
		set
		{
			lastPlayedWord = value;
			PlayerPrefs.SetString(lastWordKey, lastPlayedWord);
		}
	}
}

