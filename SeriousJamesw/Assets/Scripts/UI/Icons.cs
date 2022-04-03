using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can be used to statically access a dictionary of icons associated with characters used in condition strings
public class Icons : MonoBehaviour
{
    //Helper class - one instance of an icon
    [System.Serializable]
    private class Icon
    {
        public char key;
        public Sprite pic;
    }

    //List defined in inspector
    [SerializeField]
    private Icon[] iconList;

    //When object is first created
    private void Awake()
    {
        //Convert the list in inspector a static dictionary
        icons = new Dictionary<char, Sprite>();
        foreach(Icon i in iconList)
        {
            icons.Add(i.key, i.pic);
        }    
    }

    public static Dictionary<char, Sprite> icons;
}
