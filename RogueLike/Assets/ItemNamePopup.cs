using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemNamePopup : MonoBehaviour
{
    public string i_Name;
    public Text nameText;
    public GameObject renderObject;
    // Start is called before the first frame update
    void Awake()
    {
        i_Name = transform.parent.GetComponent<ItemStats>().fullName;
        AssignTextToPopUp();
    }
    void AssignTextToPopUp()
    {
        nameText.text = i_Name;
    }

    private void Update()
    {
        if (renderObject.GetComponent<Renderer>().isVisible)
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
