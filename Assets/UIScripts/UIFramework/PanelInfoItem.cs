using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PanelInfoItem : ISerializationCallbackReceiver
{
    [NonSerialized]
    public PanelType panelType;
    public string panelTypeString;
    public string panelPath;

    public void OnAfterDeserialize()
    {
        panelType = (PanelType)Enum.Parse(typeof(PanelType), panelTypeString);
    }

    public void OnBeforeSerialize()
    {

    }
}
