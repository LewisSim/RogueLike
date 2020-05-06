using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class LearnBehaviour : MonoBehaviour
{

    string urlget = "http://daredicing.com/getUserData.php";
    string urlStats = "http://daredicing.com/getUserStats.php";
    string urlUsePercent = "http://daredicing.com/setWepPercent.php";
    string userDataString, userStatsString;
    int m_UserID;
    bool gettingExistence, existence, checkedexistence, gettingStats, settingPercent, finishedstats;
    float timeToSend;


    // Start is called before the first frame update
    void Start()
    {
        checkedexistence = false;
        gettingExistence = false;
        existence = false;
        gettingStats = false;
        settingPercent = false;
        finishedstats = false;
        getUserID();

        timeToSend = 60;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeToSend -= Time.deltaTime;
        if (timeToSend < 0)
        {
            StartCoroutine(GetExistence());
            StartCoroutine(waitBeforeCheckExistence());
            StartCoroutine(waitBeforeGetStats());
            StartCoroutine(waitBeforeLearning());
            checkedexistence = false;
            gettingExistence = false;
            existence = false;
            gettingStats = false;
            settingPercent = false;
            finishedstats = false;
            timeToSend = 60;
            print("timed");
        }

    }


    void getUserID()
    {
        m_UserID = PlayerPrefs.GetInt("UserID");
    }

    IEnumerator GetExistence()
    {
        gettingExistence = true;
        WWWForm form = new WWWForm();
        form.AddField("userID", m_UserID.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(urlget, form))
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


            string[] pages = urlget.Split('/');
            int page = pages.Length - 1;

            if (www.isNetworkError)
            {
                print(pages[page] + ": Error: " + www.error);
            }
            else
            {
                userDataString = www.downloadHandler.text;
            }

        }

        gettingExistence = false;

    }

    IEnumerator waitBeforeCheckExistence()
    {
        while (gettingExistence)
            yield return new WaitForSeconds(0.1f);
        checkExistence();
    }

    IEnumerator waitBeforeGetStats()
    {
        while (!checkedexistence)
            yield return new WaitForSeconds(0.1f);
        StartCoroutine(GetStats());
    }


    IEnumerator waitBeforeLearning()
    {
        while (!finishedstats)
            yield return new WaitForSeconds(0.1f);
        learnFromStats();
    }


    IEnumerator waitBeforeSetPercentage(float w, string p)
    {
        while (settingPercent)
            yield return new WaitForSeconds(0.1f);
        StartCoroutine(SetPercent(w,p));
    }




    void checkExistence()
    {
        
        int parsedstring = int.Parse(userDataString);
        if (parsedstring == 0)
        {
            existence = false;
        }
        else
        {
            existence = true;
        }
        checkedexistence = true;
    }




    IEnumerator GetStats()
    {
        
        if (existence)
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


                string[] pages = urlget.Split('/');
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
        else
        {

        }


        finishedstats = true;
    }

    void learnFromStats()
    {
        
        if (existence) {

            
            string[] statperWeapon = userStatsString.Split(new[] { ',', ';' });
            float totalWeaponsCount = 0;
            

            //Get the total count
            for (int i = 1; i < statperWeapon.Length; i += 2)
            {
                
                totalWeaponsCount += float.Parse(statperWeapon[i]);
                
            }
                
            for (int i = 1; i < statperWeapon.Length; i += 2)
            {
                
                float curWeaponUse = float.Parse(statperWeapon[i]);
                float weaponUsePercent = (curWeaponUse / totalWeaponsCount) * 100f;
                string curWeapon = statperWeapon[i - 1];
                StartCoroutine(waitBeforeSetPercentage(weaponUsePercent, curWeapon));
                settingPercent = false;
            }
        }

    }

    IEnumerator SetPercent(float gotWeaponUse, string gotWeapon)
    {
        settingPercent = true;
        WWWForm form = new WWWForm();
        form.AddField("userID", m_UserID.ToString());
        form.AddField("curWeapon", gotWeapon);
        form.AddField("weaponUsePercent", gotWeaponUse.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(urlUsePercent, form))
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
            

        }

        
    }


}
