using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SaveableObjectData
{
    public string objectID;
    public bool isActive;
    public TransformData transformData;
    public List<ComponentData> saveableComponents = new List<ComponentData>();
}


[Serializable]
public struct TransformData
{
    public Vector3 position;
    public Quaternion rotation;

    public TransformData(Transform t)
    {
        position = t.localPosition;
        rotation = t.localRotation;
    }
}


[Serializable]
public struct ComponentData
{
    public string componentString;
    public string componentTypeString;
    public bool   isEnable;

    public ComponentData(string json, Type type, bool enabled)
    {
        componentString = json;
        componentTypeString = type.ToString();
        isEnable = enabled;
    }

    public ComponentData(string json, Type type)
    {
        componentString = json;
        componentTypeString = type.ToString();
        isEnable = false;
    }
}
