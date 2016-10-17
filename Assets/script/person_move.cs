using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class person_move : MonoBehaviour {
    public int speed = 4;
	public Text scoreText;
	public Text levelText;
    private bool walking = false;
	private Animator animator;
	private GameObject target;
	private List<GameObject> dieItems;
	//private GameObject[] dieItems;
	private GameObject[] touchableItems;
	public Button startButton;
	public Button restartButton;
	public Button shareButton;
	private float startX;
	private Rigidbody2D rb;
	private int hit = 0;
	private float preX;
	private int count;
	private int step = 700;
	public Text timer;
	public List<GameObject> items = new List<GameObject>();
	private string[] tips = {"接触的物品（次数）越多得分越高", "离旗帜越近的死亡得分越高"};
	// Use this for initialization
	void Start () {
		rb = (Rigidbody2D)GetComponent<Rigidbody2D> ();
		target = GameObject.Find ("flag");
		animator = GetComponent<Animator> ();
		scoreText.enabled = false;
		startButton.gameObject.SetActive (true);
		restartButton.enabled = false;
		restartButton.gameObject.SetActive (false);
		if (items != null) {
			items.Clear ();
			foreach(GameObject item in Resources.LoadAll("pre")){
				if (Random.Range (0, 2) == 0) {
					items.Add(Instantiate (item));
				}
			}
		}
		setWalking (false);
		startX = transform.position.x;
		shareButton.gameObject.SetActive (false);
		updateLeveText ();
	}
	
	// Update is called once per frame
	void Update () {
        if (walking)
        {
			if (step < 300) {
				timer.text = "倒计时：" + step;
			}
			if (step < 0) {
				Failed (null);
			} else {
				step--;
				transform.position = Vector2.Lerp (transform.position, target.transform.position, Time.deltaTime/1.5f);
				if (preX < transform.position.x + 2) {
					preX = transform.position.x;
					count = 0;
				} else {
					count++;
				}
				if (count > 120) {
					target.transform.position = Vector2.Lerp (target.transform.position, transform.position, Time.deltaTime);
				} else {
					target.transform.position = Vector2.Lerp (target.transform.position, transform.position, Time.deltaTime / 10);
				}
			}

        }
	}

	void Failed (GameObject gb)
	{
		setWalking (false);
		if (gb != null) {
			Destroy (gb);
		}
		GameManager.gm.addLost ();
		showScore ("你失败了！\n你必须杀死自己，不要碰到旗帜！");
		animator.SetBool ("ground", false);
	}

	public void OnCollisionEnter2D(Collision2D col){
		string tag = col.gameObject.tag;
		if (tag.Equals ("touchable") || tag.Equals ("item") || tag.Equals ("dropitem")) {
			hit++;
		}
		if (walking) {
			if (tag.Equals ("flag")) {
				Failed(col.gameObject);
			}
			if (tag.Equals ("item") || tag.Equals ("dropitem")) {
				if (tag.Equals ("item")) {
					animator.SetInteger ("status", 2);
				} else if (tag.Equals ("dropitem")) {
					animator.SetInteger ("status", 3);
				}
				float totalD = target.transform.position.x - startX;
				float nowD = transform.position.x-startX;
				setWalking (false);
				StartCoroutine (successfuly(totalD, nowD));

			}

		}
	
	}

	private IEnumerator successfuly(float totalD, float nowD){
		yield return new WaitForSeconds (1f);
		string tip = tips[Random.Range (0, tips.Length)];
		int score = (int)(nowD * 100 * hit / totalD);
		GameManager.gm.addWin (score);
		showScore(("你成功死掉了！\n得分：" + score + "\n" + tip));
	}

	private void showScore(string text){
		scoreText.enabled = true;
		scoreText.text = text;
		restartButton.gameObject.SetActive (true);
		startButton.gameObject.SetActive (false);
		shareButton.gameObject.SetActive (true);
		restartButton.enabled = true;
		startButton.enabled = false;
		updateLeveText ();
	}

	public void setWalking(bool walk){
		this.walking = walk;
		if (!walking) {
		} else {
			animator.SetInteger ("status", 1);
			if (items!= null) {
				foreach(GameObject item in items){
					if(item.tag.Equals("dropitem")){
						item.GetComponent<Rigidbody2D> ().isKinematic = false;
					}
					item.GetComponent<Collider2D> ().isTrigger = false;
					item.GetComponent<mouse_move_item> ().setMovable (false);
				}

			}

			target.GetComponent<Rigidbody2D> ().isKinematic = false;
			BoxCollider2D targetCollider2D = target.GetComponent<BoxCollider2D> ();
			targetCollider2D.isTrigger = false;
			GetComponent<Rigidbody2D> ().isKinematic = false;
			GetComponent<BoxCollider2D> ().isTrigger = false;
		}
	}

	public void start(){
		step = 500;
		setWalking (true);
	}

	public void restart(){
		SceneManager.LoadScene ("main");
	}

	public void Share(){
		Application.CaptureScreenshot ("dad_" + hit + ".png");
	}

	private void updateLeveText(){
		levelText.text = GameManager.gm.GetWin() + "/" + GameManager.gm.GetTotal() + "\n最高分：" + GameManager.gm.GetMaxScore();
	}
}