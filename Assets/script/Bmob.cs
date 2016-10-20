using UnityEngine;
using System.Collections;

public class Bmob : MonoBehaviour {
	public GameObject person;
	public GameObject effect;
	private bool bomb = false;
	private int time = 0;
	private GameObject eff;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (bomb) {
			time++;
		}
		if (bomb && time > 30) {
			Destroy (gameObject);
			Destroy (eff);
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		Collider2D[] colliders = Physics2D.OverlapCircleAll (gameObject.transform.position, 3f);
		foreach (Collider2D collider in colliders) {
			eff = (GameObject)Instantiate (effect, transform.position, transform.rotation);
			eff.transform.parent = transform;
			string tage = collider.gameObject.tag;
			if (tage.Equals ("item") || tage.Equals ("dropitem")) {
				Destroy (collider.gameObject);
			} else if (tage.Equals ("Player")) {
				collider.gameObject.GetComponent<person_move> ().win(4);
			}
			bomb = true;
		}
	}
}
