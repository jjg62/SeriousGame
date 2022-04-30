using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cardinality of the inventory must be greater than some value
public class ConditionCardinalityGreater : Condition
{
    //The value that the cardinality must be greater than
    [SerializeField]
    private int requiredSize;

    //Container being checked
    [SerializeField]
    private IInventoryContainer invContainer;

    public override string GetCondString()
    {
        string size = requiredSize.ToString();
        return "|" + invContainer.GetIcon() + "|/>//" + requiredSize + "/";
    }

    public override bool Test()
    {
        return invContainer.GetInventory() != null && invContainer.GetInventory().GetCardinality() > requiredSize;
    }
}
