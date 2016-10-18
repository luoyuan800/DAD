using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour {
	public Canve exitCanve; 
	private bool press;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			if(!press){
				press = true;
				exitCance.gameObject.SetActive(true);
			}
		}else{
			press = false;
			exitCance.gameObject.SetActive(false);
		}
	}
	
	void Quit(){
		Application.Quit ();
	}
}
