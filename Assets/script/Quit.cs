using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour {
	public Canvas exitCanve; 
	private bool press;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			QuitGame ();
			/*if(!press){
				press = true;
				exitCanve.gameObject.SetActive(true);
			}else{
				press = false;
				exitCanve.gameObject.SetActive(false);
			}*/
		}
	}
	
	void QuitGame(){
		GameManager.gm.save ();
		Application.Quit ();
	}
}
