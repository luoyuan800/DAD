using UnityEngine;
using System.Collections;
using System;
using System.Runtime.CompilerServices;

public class GameManager {
	public static GameManager gm = new GameManager();

	private bool isload = false;
	private int win = 0;
	private int total = 0;
	private bool isShowAdTip = true;

	private int maxScore = 0;

	[MethodImpl(MethodImplOptions.Synchronized)]
	public int WinPercen(){
		if (!isload) {
			load ();
		}
		return win * 100 / total;
	}
	
	[MethodImpl(MethodImplOptions.Synchronized)]
	void load ()
	{
		isload = true;
		win = PlayerPrefs.GetInt ("win", 0);
		total = PlayerPrefs.GetInt ("total", 0);
		maxScore = PlayerPrefs.GetInt ("max", 0);
		isShowAdTip = PlayerPrefs.GetBool ("ad", ture);
	}

	void save(){
		if (total > PlayerPrefs.GetInt ("total", 1) || maxScore > PlayerPrefs.GetInt ("max", 0) || PlayerPrefs.GetBool ("ad", ture)!=isShowAdTip) {
			PlayerPrefs.SetInt ("win", win);
			PlayerPrefs.SetInt ("total", total);
			PlayerPrefs.SetInt ("max", maxScore);
			PlayerPrefs.SetBool ("ad", isShowAdTip);
}
	}

	public int GetWin(){
		if (!isload) {
			load ();
		}
		return win;
	}

	public int GetTotal(){
		if (!isload) {
			load ();
		}
		return total;
	}

	public int GetMaxScore(){
		if (!isload) {
			load ();
		}
		return maxScore;
	}

	public void addWin(int score){
		if(score > GetMaxScore()){
			maxScore = score;
		}
		win++;
		total++;

	}
	public void addLost(){
		total++;
	}
	
	public void closeAdTip(){
		isShowAdTip = false;
	}
	
	public bool isAd(){
		return isShowAdTip;
	}
	
	public void screenshot(){
		Application.CaptureScreenshot ("dad_" + win + ".png");
	}
}
