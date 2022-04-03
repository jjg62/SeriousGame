using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Keeps track of levels, and which are unlocked
public class Levels : MonoBehaviour
{
    //Which levels are unlocked
    public static bool[] unlocked;

    //Furthest level reached
    private static int progress;

    public static string currentMusic;
    private static string[] levelMusic; //Song name for each level

    private void Awake()
    {
        //Init music
        levelMusic = new string[SceneManager.sceneCountInBuildSettings];
        levelMusic[0] = "MenuMusic";
        for (int i = 1; i < levelMusic.Length; i++)
        {
            if (i < 9) //First world
            {
                levelMusic[i] = "World1Music";
            }
            else if (i < 17) //Second world
            {
                levelMusic[i] = "World2Music";
            }
            else //Third world
            {
                levelMusic[i] = "World3Music";
            }
        }
    }

    private void Start()
    {
        progress = PlayerPrefs.GetInt("Progress", 1); //Load progress when game starts, if no data found, 1 is default
        unlocked = new bool[SceneManager.sceneCountInBuildSettings]; 
        for(int i = 1; i <= progress; i++)
        {
            //Unlock all levels up to progress
            unlocked[i] = true;
        }

    }

    //Checks if music change needed, and changes if so
    public static void ChangeMusic(int levelID)
    {
        if(currentMusic != levelMusic[levelID])
        {
            AudioManager.instance.Stop(currentMusic);
            AudioManager.instance.Play(levelMusic[levelID]);
            currentMusic = levelMusic[levelID];
        }
    }

    private void Update()
    {
        //Debug/cheat code -unlock all lvls
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnlockLevel(24);
        }
    }

    //When level is visited, unlock it in the menu
    public static void UnlockLevel(int id)
    {
        unlocked[id] = true;
        if(id > progress)
        {
            //If this is the furthest the player has reached, update progress
            progress = id;
            PlayerPrefs.SetInt("Progress", progress); //Save progress
        }
        
    }
}
