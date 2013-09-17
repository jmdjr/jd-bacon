using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;


public class DynamicText : JDMonoBehavior
{
    // Place this as an instance in a script to get access to any Text element in that script's object.
    // if null, no text mesh exists in hierarchy or failed to get/add DynamicText script.
    public static DynamicText GetTextMesh(JDMonoBehavior source)
    {
        var textObject = source.GetComponentInChildren<TextMesh>();
        if (textObject != null)
        {
            DynamicText textual = textObject.GetComponent<DynamicText>();
            if (textual == null)
            {
                textual = textObject.gameObject.AddComponent<DynamicText>();
            }

            return textual;
        }

        return null;
    }

    public static DynamicText GetTextMesh(GameObject source)
    {
        var textObject = source.GetComponentInChildren<TextMesh>();
        if (textObject != null)
        {
            DynamicText textual = textObject.GetComponent<DynamicText>();
            if (textual == null)
            {
                textual = textObject.gameObject.AddComponent<DynamicText>();
            }

            return textual;
        }

        return null;
    }

    public void SetText(string text)
    {
        TextMesh textual = this.gameObject.GetComponent<TextMesh>();
        textual.text = text;
    }
}