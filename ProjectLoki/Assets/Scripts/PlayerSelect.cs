using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSelect : BaseClass {

    public Text currentSelection;
    string currentPick;

	// Use this for initialization
	void Start () {
        currentPick = PlayerPrefs.GetString("CurrentCharacter");
	}
	
	// Update is called once per frame
	void Update () {
        currentSelection.text = currentPick;
	}

    public void SelectCharacter(string character)
    {
        PlayerPrefs.SetString("CurrentCharacter", character);
        currentPick = PlayerPrefs.GetString("CurrentCharacter");
    }

    public void StartGame()
    {
        Application.LoadLevel("Test_Player");
    }
}
