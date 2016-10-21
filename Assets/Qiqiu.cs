using UnityEngine;
using System.Collections;

public class Qiqiu : MonoBehaviour {
	public GameObject effect;
	public person_move move;
	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
		animator.speed = 0;
		move = GameObject.FindGameObjectWithTag ("Player").GetComponent<person_move>();
	}
	
	// Update is called once per frame
	void Update () {
		if (move.isWalking ()) {
			animator.speed = 1;
		}
	}

	public void bomb(){
		GameObject eff = (GameObject)Instantiate (effect, transform.position, transform.rotation);
		Destroy (gameObject);
		Destroy (eff, 1.0f);
	}
}
