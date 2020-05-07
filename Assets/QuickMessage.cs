using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuickMessage : MonoBehaviour
{

    static TextMeshProUGUI selfStatic;
    static bool fadingIn = false, fadingOut = false;
    static float count, m_timer;

    bool timing = false;

    private void Awake()
    {
        selfStatic = gameObject.GetComponent<TextMeshProUGUI>();
        selfStatic.alpha = 0f;
    }

    public static void Message(string text, float timer)
    {
        if (count == 0f)
        {
            selfStatic.text = text;
            fadingIn = true;
            m_timer = timer;

            //print("fading in ");
        }
    }

    private void Update()
    {
        if (fadingIn)
        {
            //print("alpha: "+selfStatic.color.a);
            selfStatic.color = Color32.Lerp(selfStatic.color, new Color32(255, 255, 255, 255), Time.deltaTime * 2);
            if (selfStatic.color.a > 0.9f)
            {
                fadingIn = false;
                timing = true;
            }
        }

        if (timing)
        {
            if(count < m_timer)
            {
                count += Time.deltaTime;
            }
            else
            {
                timing = false;
                fadingOut = true;
                count = 0f;
                //print("fading in ");
            }
        }

        if (fadingOut)
        {
            selfStatic.color = Color32.Lerp(selfStatic.color, new Color32(255, 255, 255, 0), Time.deltaTime * 2);
            if(selfStatic.color.a < 0.1f)
            {
                selfStatic.color = new Color32(255, 255, 255, 0);
                fadingOut = false;
            }
        }
    }
}
