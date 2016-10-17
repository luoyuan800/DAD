﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.CompilerServices;

public class GameManager {
	public static GameManager gm = new GameManager();

	private bool isload = false;
	private int win = 0;
	private int total = 0;

	private int maxScore = 0;

	[MethodImpl(MethodImplOptions.Synchronized)]
	public int WinPercen(){
		if (!isload) {
			load ();
		}
		return win * 100 / total;
	}

	void load ()
	{
		isload = true;
		win = PlayerPrefs.GetInt ("win", 0);
		total = PlayerPrefs.GetInt ("total", 0);
		maxScore = PlayerPrefs.GetInt ("max", 0);
	}

	void save(){
		if (total > PlayerPrefs.GetInt ("total", 1) || maxScore > PlayerPrefs.GetInt ("max", 0)) {
			PlayerPrefs.SetInt ("win", win);
			PlayerPrefs.SetInt ("total", total);
			PlayerPrefs.SetInt ("max", maxScore);
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
}