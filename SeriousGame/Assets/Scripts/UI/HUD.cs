using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Class allowing global access to elements of the HUD
//Provides various functions called by HUD buttons
public class HUD : MonoBehaviour
{
    #region Singleton
    public static HUD instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    //Elements of the HUD
    public InventoryUI inventory;
    public FunctionEditor functionEditor; //Currently not used
    public ViewSets viewSets;

    //Image for button that toggles music
    [SerializeField]
    private Image soundButtonImg;

    //Sprites for when music is on/off
    [SerializeField]
    private Sprite musicOn;

    [SerializeField]
    private Sprite musicOff;

    //Before first frame
    private void Start()
    {
        //Load in whether player has music on or off
        musicMute = PlayerPrefs.GetInt("MusicMute", 0) != 0;
        SetMusicMute(musicMute);
    }

    //Restart current level
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Return to main menu
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    //Called by music mute button
    private bool musicMute = false;
    public void ToggleMusicMute()
    {
        musicMute = !musicMute;
        SetMusicMute(musicMute);
    }


    private void SetMusicMute(bool mute)
    {
        //Change Button Sprite
        soundButtonImg.sprite = mute ? musicOff : musicOn;
        soundButtonImg.color = mute ? Color.red : Color.cyan;
        
        //Set volume of music
        AudioManager.instance.SetVolume(Levels.currentMusic, mute ? 0 : 0.12f);
        //Save player's preference
        PlayerPrefs.SetInt("MusicMute", mute ? 1 : 0);
    }

    //Called by button to display bags
    private bool showingSets = false;
    public void ViewSets()
    {
        showingSets = !showingSets;

        viewSets.gameObject.SetActive(showingSets);
        viewSets.Refresh(); //Update the sets displayed
    }
}
