using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Button that takes player to a level
public class LevelSelectButton : MonoBehaviour
{
    [SerializeField]
    private int level;

    private void Start()
    {
        //Only unlocked when level has been visited
        GetComponent<Button>().interactable = Levels.unlocked[level];
        GetComponentInChildren<TextMeshProUGUI>().enabled = Levels.unlocked[level];
    }
}
