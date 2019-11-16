using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class ItemBase 
{
    [XmlAttribute("paneName")]
    public string uiPaneName;
    [XmlElement("itmName")]
    public string itemName;
    [XmlElement("itmDescription")]
    public string itemDescription;
}