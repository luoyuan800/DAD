using UnityEngine;
using System.Collections;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager {
	public static GameManager gm = new GameManager();

	private bool isload = false;
	private int win = 0;
	private int total = 0;
	private bool isShowAdTip = true;
	private GameObject play;
	private person_move perosn;

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
		isShowAdTip = PlayerPrefs.GetInt ("ad", 1) == 1;
		play = GameObject.Find("Play");
		person = play.GetComponent<person_move>();
	}

	public void save(){
		if (total > PlayerPrefs.GetInt ("total", 1) || maxScore > PlayerPrefs.GetInt ("max", 0) ) {
			PlayerPrefs.SetInt ("win", win);
			PlayerPrefs.SetInt ("total", total);
			PlayerPrefs.SetInt ("max", maxScore);
			PlayerPrefs.SetInt ("ad", isShowAdTip?1:0);
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
	
	public void record(List<GameObject> gameObjects){
		string record = '';
		foreach(GameObject gameObject in gameObjects){
			record += gameObject.name + ":" + gameObject.transform.x + ":" + gameObject.transform.y + ";";
		}
		FileInfo file = new FileInfo(Application.persistentDataPath + "/record/" + GetWin() + ".dad");
		StreamWriter writer = file.CreateText();
		writer.WriteLine(record);
		writer.Close();
		writer.Dispose();
	}
	
	public List<String> readRecords(){
		List<string> records = new List<string>();
		DirectoryInfo direction = new DirectoryInfo(Application.persistentDataPath + "/record/");  
		FileInfo[] files = direction.GetFiles("*",SearchOption.AllDirectories); 
		foreach(FileInfo file in files){
			StreamReader reader = file.OpenText();
			records.add(reader.ReaderLine());
			reader.Close();
			reader.Dispose();
		}
		return records;
	}
	
	public void replayRecord(String record){
		string[] strings = record.Split(":");
		if(strings.length == 3){
			GameObject gobject = Resources.Load(strings[0]);
			gobject = person.addItem(gobject);
			gobject.transform.x = strings[1] as float;
			gobject.transform.y = strings[2] as float;
		}
	}
}
