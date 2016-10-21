using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsRewardedButton : MonoBehaviour
{
	public string zoneId = "rewardedVideo";
	public GameObject person;
    private Object[] items;
	private person_move personScript;
	

	void Start(){
		items = Resources.LoadAll("pre");
		Advertisement.Initialize("1172895", false); 
		person = GameObject.FindGameObjectWithTag ("Player");
		if(person!=null){
			personScript = person.GetComponent<person_move>();
		}
	}
	
    void OnGUI ()
    {
        Rect buttonRect = new Rect (10, 10, 200, 80);
		string buttonText = Advertisement.IsReady (zoneId) || Advertisement.IsReady () ? "Show Ad" : "Waiting...";

        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        if (GUI.Button (buttonRect, buttonText)) {
			if(Advertisement.IsReady (zoneId)){
				Advertisement.Show (zoneId, options);
			}else{
				Advertisement.Show ("video", options);
			}
        }
    }

    private void HandleShowResult (ShowResult result)
    {
        switch (result)
        {
        case ShowResult.Finished:
				foreach(GameObject item in items){
					if(Random.Range(0,2) == 0){
						personScript.addItem(item);
						return;
					}
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