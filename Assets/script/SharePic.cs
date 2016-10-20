using UnityEngine;
using System.Collections;
using System.IO;
     
	 //http://www.ceeger.com/forum/read.php?tid=15252
using System;


public class SharePic : MonoBehaviour
{
         
    public  static string imagePath;
    static AndroidJavaClass sharePluginClass;
    static AndroidJavaClass unityPlayer;
    static AndroidJavaObject currActivity;
    private static SharePic mInstance;
         
    public static SharePic instance {
        get{ return mInstance;}
    }
         
    void Awake ()
    {
        mInstance = this;
    }
         
    void Start ()
    {
		if (Application.platform == RuntimePlatform.Android) {
			imagePath = Application.persistentDataPath + "/dad.png";
			sharePluginClass = new AndroidJavaClass ("com.ari.tool.UnityAndroidTool");
			if (sharePluginClass == null) {
				Debug.Log ("sharePluginClass is null");
			} else {
				Debug.Log ("sharePluginClass is not null");
			}
			unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			currActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		}
    }
         
    public void CallShare (string handline, string subject, string text, string imageName, bool image)
    {
		imagePath = Application.persistentDataPath + "/" + imageName;
        Debug.Log ("share call start : " + imagePath);
        if (image) {
            sharePluginClass.CallStatic ("share", new object[] {
                handline,
                subject,
                text,
                imagePath
            });
        } else {
            sharePluginClass.CallStatic ("share", new object[] {
                handline,
                subject,
                text,
                ""
            });
        }
        Debug.Log ("share call end");
    }
         
	public void ScreenShot (string file)
    {
        StartCoroutine (GetCapture (file));
    }
         
	IEnumerator GetCapture (string file)
    {
             
		Debug.Log ("start capture path:" + Application.persistentDataPath + "/" + file);
		yield return new WaitForEndOfFrame ();
		try{    
        int width = Screen.width;
             
        int height = Screen.height;
             
        Texture2D tex = new Texture2D (width, height, TextureFormat.RGB24, false);
             
        tex.ReadPixels (new Rect (0, 0, width, height), 0, 0, true);
             
        byte[] imagebytes = tex.EncodeToPNG ();//png
             
        tex.Compress (false);
             
        //      image.mainTexture = tex;
		imagePath = Application.persistentDataPath + "/" + file;
        File.WriteAllBytes (imagePath, imagebytes);//save
		}catch(Exception e){
			Debug.LogError (e.Message);
		}
             
        Debug.Log ("end capture path:" + Application.persistentDataPath);
    }
}