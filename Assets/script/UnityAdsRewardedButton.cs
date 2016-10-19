using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsRewardedButton : MonoBehaviour
{
	public string zoneId = "rewardedVideo";
	public int time;
    private Object[] items;
	

	void Start(){
		items = Resources.LoadAll("pre");
	}
	
    void OnGUI ()
    {
        if (string.IsNullOrEmpty (zoneId)) zoneId = null;

        Rect buttonRect = new Rect (10, 10, 150, 50);
		string buttonText = Advertisement.IsReady (zoneId) || Advertisement.IsReady () ? "Show Ad" : "Waiting...";

        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        if (GUI.Button (buttonRect, buttonText)) {
            Advertisement.Show (zoneId, options);
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
						Instantiate(item);
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