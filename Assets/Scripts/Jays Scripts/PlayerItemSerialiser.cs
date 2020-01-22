using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

public class PlayerItemSerialiser : MonoBehaviour
{
    public const string path = "JsItemGenDatabase";

    void Start()
    {
       
    }        

    //void Update()
    //{

    //    PItemBase LeatherArmour = new PItemBase();
    //    LeatherArmour.Dog = "Dog tester";
    //    LeatherArmour.Dog2 = "Dog tester2";

    //    //Serialiser
    //    XmlSerializer serializer = new XmlSerializer(typeof(PItemBase));
    //    StreamWriter writer = new StreamWriter(path);
    //    serializer.Serialize(writer.BaseStream, LeatherArmour);
    //    writer.Close();
    //    print("XMLS - Run");
    //}
}
