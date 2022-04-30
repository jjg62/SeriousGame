using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class for a function on sets/bags
public abstract class SetFunction : MonoBehaviour
{
    public abstract List<Item.Type> Apply(List<Item> input);
    public abstract bool isBijective();
    public abstract List<Item.Type> ApplyInverse(List<Item> input);
    public abstract string GetDisplayString();
    public abstract bool CanApply();
}
