using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cardinality of inventory must be some number
public class ConditionCardinality : Condition
{
    //The required cardinality
    [SerializeField]
    private int requiredSize;

    //Which inventory is being checked (player, podium, etc.)
    [SerializeField]
    private IInventoryContainer invContainer;

    public override string GetCondString()
    {
        string size = requiredSize.ToString();
        return "|" + invContainer.GetIcon() + "|=/" + requiredSize + "/"; 
    }

    public override bool Test()
    {
        return invContainer.GetInventory() != null && invContainer.GetInventory().GetCardinality() == requiredSize;
    }
}
