using UnityEngine;
using System.Collections;

public class Resolution : MonoBehaviour {
	public Camera mainCamera;
	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		mainCamera.aspect = 0.5f;
	}
	

}
