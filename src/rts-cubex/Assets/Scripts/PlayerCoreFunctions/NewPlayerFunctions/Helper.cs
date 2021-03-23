using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper 
{
    public GameObject getChildGameObjectByName(GameObject parentObject, string name)
    {
        Transform trans = parentObject.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null) {
            return childTrans.gameObject;
        } else {
            return null;
        }
    }
}
