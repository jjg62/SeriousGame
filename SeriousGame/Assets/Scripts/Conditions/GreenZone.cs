using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//The 'goal' in each level - if player enters while meeting a condition, they win
public class GreenZone : HasDisplay
{
    //Has level already finished
    private bool levelFinished = false;

    //When the object is created
    private new void Start()
    {
        base.Start();
        Levels.ChangeMusic(SceneManager.GetActiveScene().buildIndex);
    }

    //While an object is in the trigger area
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!levelFinished && collision.tag == "Player") //If it's the player, and level hasn't finished already
        {
            if (con.Test()) //If condition is passed
            {
                levelFinished = true;
                NextLevel(collision.gameObject); //Go to next level
            }
        }
    }

    //When an object first enters the trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!levelFinished && collision.tag == "Player") //If it's the player, and level hasn't finished already
        {
            if (!con.Test()) //If condition is failed, alert player
            {
                display.Flash();
            }
        }
    }

    //Go to next level
    private void NextLevel(GameObject player)
    {
        //Disable player controls
        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(FinishAnimation(player.transform)); 

        int nextLvl = SceneManager.GetActiveScene().buildIndex + 1;
        //If level has not been unlocked before, unlock it
        if (nextLvl < Levels.unlocked.Length)
        {
            Levels.UnlockLevel(nextLvl);
        }
    }

    //Gradually move player to center
    IEnumerator FinishAnimation(Transform player)
    {
        //Duration of movement
        const float duration = 1.0f;
        //Start fading to black
        Camera.main.GetComponent<Effects>().Fade(true, duration);

        float t = 0.0f;
        Vector2 originalPos = player.position;

        //Linearly interpolate position from original pos to center of green zone
        while(t < duration)
        {
            t += Time.deltaTime;
            player.position = Vector2.Lerp(originalPos, transform.position, t/duration);
            yield return null;
        }
        player.position = transform.position;

        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.Stop("Footstep");

        //Go to next level
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevel == Levels.endOfTutorial + 1 || nextLevel > SceneManager.sceneCountInBuildSettings) nextLevel = 0;
        SceneManager.LoadScene(nextLevel);
    }
}
