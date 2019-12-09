using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Xml;


[XmlRoot("PlayerShopCollection")]
public class ShopInitialiser
{

    //Variables
    [XmlArray("ShopItems")]
    [XmlArrayItem("Item")]

    public List<ItemBase> ShopItems = new List<ItemBase>();

    //Methods
    public static ShopInitialiser Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(ShopInitialiser));
        StringReader reader = new StringReader(_xml.text);
        ShopInitialiser ShopItems = serializer.Deserialize(reader) as ShopInitialiser;
        reader.Close();
        return ShopItems;
    }
}
