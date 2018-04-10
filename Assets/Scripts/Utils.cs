using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils  {
    public static void SetLayerRecursively(Transform t, int layer)
    {
        Debug.Log("setting layer for " + t.gameObject.name + " to " + layer);
        t.gameObject.layer = layer;
        foreach (Transform otherT in t.transform)
        {
            SetLayerRecursively(otherT, layer);
        }

    }

}
