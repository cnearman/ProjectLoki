using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("Test_Player");
    }
}
