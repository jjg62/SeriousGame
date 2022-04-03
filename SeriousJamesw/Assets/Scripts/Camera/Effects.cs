using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Camera effects - currently only fading from/to black
public class Effects : MonoBehaviour
{
    //Reference to a black UI object covering screen
    [SerializeField]
    private RawImage black;

    //Stop any current fade effects and start a new one
    public void Fade(bool toOpaque, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeBlack(toOpaque, duration));
    }

    //At start of each level, fade from black
    private void Start()
    {
        Fade(false, 0.5f);
    }

    //Gradually change transparency
    IEnumerator FadeBlack(bool toOpaque, float duration)
    {
        //Define a transparent colour
        Color transparent = Color.black;
        transparent.a = 0;

        //Get starting and ending colour depending on toOpaque bool
        Color start = toOpaque ? transparent : Color.black;
        Color end = toOpaque ? Color.black : transparent;

        //Set starting colour
        black.color = start;
        float t = 0;

        //Linearly interpolate between start and end colours
        while(t < duration)
        {
            t += Time.deltaTime;
            black.color = Color.Lerp(start, end, t/duration);
            yield return null;
        }
        black.color = end;
    }
}
