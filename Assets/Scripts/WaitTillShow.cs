using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitTillShow : MonoBehaviour
{
    public float waitTime;
    float timer;
    bool allowSpace;
    Text text;
    LevelLoader lvlload;
    // Start is called before the first frame update
    void Start()
    {
        timer = waitTime;
        text = gameObject.GetComponent<Text>();
        text.enabled = false;
        lvlload = gameObject.GetComponent<LevelLoader>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            text.enabled = true;
            allowSpace = true;
        }

        if (allowSpace)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                lvlload.LoadSceneStandard(6);
                allowSpace = false;
            }
        }
    }
}
