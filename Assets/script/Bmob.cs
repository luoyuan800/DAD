using UnityEngine;
using System.Collections;

public class Bmob : MonoBehaviour {
	public GameObject effect;
	private bool bomb = false;
	private GameObject eff;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D col){
		if (!bomb) {
			Collider2D[] colliders = Physics2D.OverlapCircleAll (gameObject.transform.position, 2f);
			foreach (Collider2D collider in colliders) {
				
				eff = (GameObject)Instantiate (effect, transform.position, transform.rotation);
				string tage = collider.gameObject.tag;
				if (tage.Equals ("item") || tage.Equals ("dropitem") || tage.Equals("touchable")) {
					if (!collider.gameObject.Equals(gameObject)) {
						Destroy (collider.gameObject);
					}
				} else if (tage.Equals ("Player")) {
					collider.gameObject.GetComponent<person_move> ().win (4);
				}
				Destroy (gameObject);
				Destroy (eff, 1.0f);
			}
		}
	}
}
