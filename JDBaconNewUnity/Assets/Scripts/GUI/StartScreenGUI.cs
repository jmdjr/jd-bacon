using UnityEngine;
using System.Collections;

public class StartScreenGUI : MonoBehaviour {

    public GUISkin mSkin;
    public Texture2D mBackground;
    public GUIStyle mBackgroundStyle = new GUIStyle();

    private bool isLoading = false;
    public bool IsLoading { get { return isLoading; } set { isLoading = value; } }

    void OnGUI(){
        if (mSkin)
            GUI.skin = mSkin;
        else
            Debug.Log("StartMenuGUI: GUI Skin object missing!");

        GUI.Label(new Rect((Screen.width - (Screen.height * 2)) * 0.75f, 0, Screen.height * 2, Screen.height), "", mBackgroundStyle);
        GUI.Label(new Rect((Screen.width / 2), 50, 400, 100), "JDBacon Game!!", "StartScreenTitle");
        if (GUI.Button(new Rect((Screen.width / 2), Screen.height - 160, 140, 70), "Play"))
        {
            Debug.Log("Play Button Pressed");
            Application.LoadLevel("LevelOne");
        }
        if (GUI.Button(new Rect((Screen.width / 2), Screen.height - 80, 140, 70), "Quit"))
        {
            Debug.Log("Quit Button Pressed");
            Application.Quit();
        }
    }
	// Use this for initialization
	void Start () {
        mBackgroundStyle.normal.background = mBackground;  
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
