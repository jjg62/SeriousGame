using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Relation used to control behaviour of Function Gates
public class Relation : MonoBehaviour
{
    //Helper class for links between domain and codomain
    [System.Serializable]
    public class Link
    {
        public int input;
        public int output;
    }

    //List of items in domain
    public List<Item.Type> domain;
    //List of items in codomain
    public List<Item.Type> codomain;
    //List of links
    public List<Link> links;

    //See if a relation is a well-defined function
    public bool isFunction()
    {
        //Check there is only one link per input
        List<int> linkInputs = new List<int>();
        foreach (Link l in links)
        {
            if (linkInputs.Contains(l.input)) return false;
            else linkInputs.Add(l.input);
        }

        //And that there are as many links as inputs
        return linkInputs.Count == domain.Count;
    }

    //See if a relation is a bijective function
    public bool isBijective()
    {
        if (!isFunction()) return false;

        //Injective
        List<int> linkOutputs = new List<int>();
        foreach(Link l in links)
        {
            if (linkOutputs.Contains(l.output)) return false;
            else linkOutputs.Add(l.output);
        }
        //And domain size = codomain size (Surjective)
        return domain.Count == codomain.Count;
    }

    //Result of the function when applied to one input
    public Item.Type Apply(Item.Type input)
    {
        if (domain.Contains(input))
        {
            int inputIndex = domain.IndexOf(input);

            foreach(Link l in links)
            {
                //Find link with matching input
                if(l.input == inputIndex)
                {
                    //Return that link's output
                    return codomain[l.output];
                }
            }
        }
        return input;
    }

    //Result of the function inverse when applied to one input
    public Item.Type ApplyInverse(Item.Type input)
    {
        if (codomain.Contains(input))
        {
            int inputIndex = codomain.IndexOf(input);

            foreach (Link l in links)
            {
                //Find link with matching output
                if (l.output == inputIndex)
                {
                    //Return that link's input
                    return domain[l.input];
                }
            }
        }
        return input;
    }
}
