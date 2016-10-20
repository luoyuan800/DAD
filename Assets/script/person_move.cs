using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class person_move : MonoBehaviour {
    public int speed = 4;
	public Text scoreText;
	public Text levelText;
	public Button startButton;
	public Button restartButton;
	public Button shareButton;
	public Button nextButton;
	public Text timer;
	
	private List<GameObject> items = new List<GameObject>();
	private string[] tips = {"接触的物品（次数）越多得分越高", "离旗帜越近的死亡得分越高","分享你的记录给朋友看看"};
	private bool walking = false;
	private Animator animator;
	private GameObject target;
	private float startX;
	private Rigidbody2D rb;
	private int hit = 0;
	private float preX;
	private int count;
	private int step = 500;
	private bool isReplay = false;
	// Use this for initialization
	void Start () {
		rb = (Rigidbody2D)GetComponent<Rigidbody2D> ();
		target = GameObject.Find ("flag");
		animator = GetComponent<Animator> ();
		scoreText.enabled = false;
		startButton.gameObject.SetActive (true);
		restartButton.enabled = false;
		restartButton.gameObject.SetActive (false);
		nextButton.gameObject.SetActive (false);
		if (items != null) {
			items.Clear ();
			foreach(GameObject item in Resources.LoadAll("pre")){
				if (Random.Range (0, 2) == 0) {
					addItem(item);
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
			if (step < 200) {
				timer.text = "倒计时：" + step;
			}
			if (step < 1) {
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
					target.transform.position = Vector2.Lerp (target.transform.position, transform.position, Time.deltaTime / 8);
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
		if (!isReplay) {
			GameManager.gm.addLost ();
		}
		showScore ("你失败了！\n你必须在限定时间内杀死自己，不要碰到旗帜！");
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
			float speed = rb.velocity.magnitude;
			if (tag.Equals ("item") || tag.Equals ("dropitem") || (speed > 50 && tag.Equals("background"))) {
				if (tag.Equals ("item")) {
					win(2);
				} else if (tag.Equals ("dropitem")) {
					if(col.gameObject.GetComponent<Rigidbody2D> ().mass > 1){
						win (4);
					}else{
						win(3);
					}
				}
			}
		
		}
	
	}

	public void win(int index){
		animator.SetInteger ("status", index);
		float totalD = target.transform.position.x - startX;
		float nowD = transform.position.x-startX;
		StartCoroutine (successfuly(totalD, nowD));}

	private IEnumerator successfuly(float totalD, float nowD){
		setWalking (false);
		yield return new WaitForSeconds (1f);
		string tip = tips[Random.Range (0, tips.Length)];
		int score = (int)(nowD * 100 * hit / totalD);
		if (!isReplay) {
			GameManager.gm.addWin (score);
		}
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
		nextButton.gameObject.SetActive (true);
		updateLeveText ();
	}

	public void setWalking(bool walk){
		this.walking = walk;
		if (!walking) {
		} else {
			animator.SetInteger ("status", 1);
			if (items!= null) {
				foreach(GameObject item in items){
					Rigidbody2D rgb = item.GetComponent<Rigidbody2D> ();
					if(item.tag.Equals("dropitem") && rgb!=null){
						rgb.isKinematic = false;
					}
					if(rgb!=null){
						item.GetComponent<Collider2D> ().isTrigger = false;
					}
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
		//SharePic.instance.ScreenShot ();
		string file = "dad_" + GameManager.gm.GetWin() + ".png";
		//Application.CaptureScreenshot (file);
		SharePic.instance.ScreenShot (file);
		SharePic.instance.CallShare ("share", "", "share text and image ha ha ha ha ha ha ~", file, true);
		//Application.CaptureScreenshot ("dad_" + hit + ".png");
	}

	private void updateLeveText(){
		levelText.text = GameManager.gm.GetWin() + "/" + GameManager.gm.GetTotal() + "\n最高分：" + GameManager.gm.GetMaxScore();
	}

	public void play(){
		startButton.gameObject.SetActive (false);
		restartButton.gameObject.SetActive (false);
	}
	
	public GameObject addItem(GameObject item){
		if (items.Count < 8) {
			GameObject ins = Instantiate (item);
			items.Add (ins);
			return ins;
		} else {
			return null;
		}
	
	}
}