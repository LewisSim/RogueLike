using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
public class ShopManager : MonoBehaviour
{

    //Variables
    public Text shopTabOne, shopTabTwo, shopTabThree, priceTabOne, priceTabTwo, priceTabThree;
    public int tabOnePrice, tabTwoPrice, tabThreePrice;
    public int identifier;
    public const string path = "JsAGDDDatabase";

    public 

    void Start()
    {

        ShopInitialiser i = ShopInitialiser.Load(path);


        foreach (ItemBase Item in i.ShopItems)
        {
            if (Item.uiPaneName == "text1")
            {
                shopTabOne.text = Item.itemName;
                priceTabOne.text = Item.itemDescription;
                tabOnePrice = System.Convert.ToInt32(Item.itemDescription);
                print(tabOnePrice);
            }
            if (Item.uiPaneName == "text2")
            {
                shopTabTwo.text = Item.itemName;
                priceTabTwo.text = Item.itemDescription;
                tabOnePrice = System.Convert.ToInt32(Item.itemDescription);
                print(tabTwoPrice);
            }
            if (Item.uiPaneName == "text3")
            {
                shopTabThree.text = Item.itemName;
                priceTabThree.text = Item.itemDescription;
                tabThreePrice = System.Convert.ToInt32(Item.itemDescription); 
                print(tabThreePrice);
            }
        }

    }

     public void ButtonOne()
    {
        print("This button works");
    }

    public void ButtonTwo()
    {
        print("This button works x2");
    }

    public void ButtonThree()
    {
        print("This button works x3");
    }

}


