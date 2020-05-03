using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetUserID : MonoBehaviour
{

    string key = "UserID";
    //get the date in sql format
    string userDataString;
    string urlget = "http://daredicing.com/UserID.php";
    string urlset = "http://daredicing.com/setDate.php";
    string urlplays = "http://daredicing.com/recordPlays.php";
    int userID;
    bool gettingPage;

    // Start is called before the first frame update
    void Start()
    {
        //If we dont have a UserID
        if (!PlayerPrefs.HasKey(key))
        {
            createUserID();
        }
        //we have a UserID so lets update our datetime
        else
        {
            StartCoroutine(UpdateDate());
        }

        StartCoroutine(EnterPlays());
    }

    IEnumerator GetUser()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(urlget))
        {
            gettingPage = true;
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = urlget.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                print(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                userDataString = webRequest.downloadHandler.text;
            }
        }
        gettingPage = false;
    }

    IEnumerator UpdateDate()
    {


        int m_ID = PlayerPrefs.GetInt("UserID");
        print(m_ID);
        WWWForm form = new WWWForm();
        form.AddField("userID", m_ID.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(urlset, form))
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

    IEnumerator EnterPlays()
    {

        int m_ID = PlayerPrefs.GetInt("UserID");
        print(m_ID);
        WWWForm form = new WWWForm();
        form.AddField("userID", m_ID.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(urlplays, form))
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

    IEnumerator wait()
    {
        while (gettingPage)
            yield return new WaitForSeconds(0.1f);
        userID = int.Parse(userDataString);
        PlayerPrefs.SetInt("UserID", userID);
    }

    //create a userID
    void createUserID()
    {
        StartCoroutine(GetUser());
        StartCoroutine(wait()); //Go to this coroutine so we can wait until we have loaded the page until try and set the var
       
    }
}
