using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsRewardedButton : MonoBehaviour
{
	public string zoneId = "rewardedVideo";
	public GameObject person;
	public int time;
    private Object[] items;
	private person_move personScript;
	

	void Start(){
		items = Resources.LoadAll("pre");
		Advertisement.Initialize("1172895", true); 
		if(person!=null){
			personScript = person.GetComponent<person_move>();
		}
	}
	
    void OnGUI ()
    {
        if (string.IsNullOrEmpty (zoneId)) zoneId = null;

        Rect buttonRect = new Rect (10, 10, 150, 50);
		string buttonText = Advertisement.IsReady (zoneId) ? "Show Ad" : "Waiting...";

        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        if (GUI.Button (buttonRect, buttonText)) {
			if(Advertisement.IsReady (zoneId)){
				Advertisement.Show (zoneId, options);
			}else{
				Advertisement.Show ();
			}
        }
    }

    private void HandleShowResult (ShowResult result)
    {
        switch (result)
        {
        case ShowResult.Finished:
			if(time > 0){
				foreach(GameObject item in items){
					if(Random.Range(0,2) == 0){
						personScript.addItem(item);
						return;
					}
				}
				time --;
			}
            break;
        case ShowResult.Skipped:
            Debug.LogWarning ("Video was skipped.");
            break;
        case ShowResult.Failed:
			
			Debug.LogError ("Video failed to show." );
            break;
        }
    }
}