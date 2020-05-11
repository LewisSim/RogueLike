using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetWeaponPercent : MonoBehaviour
{
    string userDataString;
    [System.NonSerialized]
    public string userStatsString;
    float timeToSend;
    int m_UserID;
    string urlStats = "http://daredicing.com/getWeaponPercent.php";
    // Start is called before the first frame update
    void Start()
    {
        getUserID();
        timeToSend = 60f;
        StartCoroutine(WeaponPercent());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeToSend -= Time.deltaTime;
        
        if (timeToSend < 0)
        {
            StartCoroutine(WeaponPercent());
            timeToSend = 60f;
        }
    }

    void getUserID()
    {
        m_UserID = PlayerPrefs.GetInt("UserID");
    }


    IEnumerator WeaponPercent()
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", m_UserID.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(urlStats, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                print(www.error);
            }
            else
            {
                print("Form upload complete!");
            }


            string[] pages = urlStats.Split('/');
            int page = pages.Length - 1;

            if (www.isNetworkError)
            {
                print(pages[page] + ": Error: " + www.error);
            }
            else
            {
                userStatsString = www.downloadHandler.text;
            }


        }


    }
}
