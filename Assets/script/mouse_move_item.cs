using UnityEngine;
using System.Collections;

public class mouse_move_item : MonoBehaviour {
	private bool movable = true;
	public void setMovable(bool move){
		movable = move;
	}
	void Start ()
	{
		StartCoroutine(OnMouseDown());
	}

	IEnumerator OnMouseDown()
	{
		if (movable) {
			Vector3 screenSpace = Camera.main.WorldToScreenPoint (transform.position);

			Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
			while (Input.GetMouseButton (0)) {
				Vector3 curScreenSpace = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
				Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenSpace) + offset;
				transform.position = curPosition;
				yield return new WaitForFixedUpdate (); 
			}
		}
	}
}
