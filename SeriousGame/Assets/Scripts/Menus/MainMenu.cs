using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Script controlling main menu UI
public class MainMenu : MonoBehaviour
{
    //Submenus that can be accessed
    [SerializeField]
    private GameObject[] menus;

    //Fade-to-black effect
    [SerializeField]
    private RawImage black;

    //When object is created
    private void Awake()
    {
        //Set the title submenu to be the active one
        menus[0].SetActive(true);
        menus[1].SetActive(false);
    }

    private void Start()
    {
        Levels.ChangeMusic(0);
    }

    //Transition to a different submenu
    public void ChangeMenu(int id)
    {
        AudioManager.instance.Play("ChangeMenu");
        StartCoroutine(ChangeMenuAnimation(id));
    }

    IEnumerator ChangeMenuAnimation(int id)
    {
        //Disable all menus
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }

        //Get original and target camera position
        Vector3 originalPos = Camera.main.transform.position;
        Vector3 cameraPos = (id == 0 ? new Vector3(-3, 0.5f, -10f) : new Vector3(3.8f, 0.5f, -10f)); //Title submenu has camera at different position

        float t = 0;
        float PAN_DURATION = 0.25f;

        //Linearly interpolate camera position
        while(t < PAN_DURATION)
        {
            Camera.main.transform.position = Vector3.Lerp(originalPos, cameraPos, t / PAN_DURATION);
            t += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = cameraPos;
        menus[id].SetActive(true); //Set new submenu to be active

    }



    private IEnumerator changingLevel;
    public void ChangeLevel(int id)
    {
        if (changingLevel != null) return;
        changingLevel = ChangeLevelAnimation(id);
        StartCoroutine(changingLevel);
    }

    IEnumerator ChangeLevelAnimation(int id)
    {
        float t = 0;
        float FADE_DURATION = 0.5f;

        while(t < FADE_DURATION)
        {
            //Change alpha
            Color c = Color.black;
            c.a = Mathf.Lerp(0, 1, t / FADE_DURATION);
            black.color = c;

            t += Time.deltaTime;
            yield return null;
        }
        black.color = Color.black;

        SceneManager.LoadScene(id);
    }
}
