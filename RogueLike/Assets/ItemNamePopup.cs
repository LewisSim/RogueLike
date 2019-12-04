using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemNamePopup : MonoBehaviour
{
    public Text nameText;
    public GameObject renderObject;
    // Start is called before the first frame update
    void Awake()
    {
        AssignTextToPopUp("Item");
    }
    public void AssignTextToPopUp(string i_Name)
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
