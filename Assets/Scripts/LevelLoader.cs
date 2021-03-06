﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    public GameObject loadingScreen;
    public GameObject healthUI;
    public GameObject minimapUI;

    public Slider slider;
    public Text progressText;
    public Animator transition;
    public float transitionTime = 1f;

    //Level Gen integration
    public bool isLevelWithGeneration;
    private LevelGen lGen;

    public int sceneAlterIndex;

    [Header("You must enable this if you are using the LoadDepends function")]
    public bool playerPref;



	public void LoadLevel (int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void LoadSceneStandard(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        print("loading...");

        loadingScreen.SetActive(true);

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }

    //Check if level gen scene- get level gen stats
    private void Awake()
    {
        if (isLevelWithGeneration)
        {
            lGen = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGen>();
        }

    }

    /// <summary>
    /// Will load a scene based on an if, you must enable the playerpref bool to have this work at the moment
    /// </summary>
    /// <param name="sceneIndex">The scene to run if the result is true</param>
    /// <param name="sceneAlterIndex">The scene to run if the result is false</param>
    public void LoadDepends(int sceneIndex)
    {
        int sendIndex = 0;
        if (playerPref)
        {
            if (PlayerPrefs.HasKey("Intro"))
            {
                if (PlayerPrefs.GetInt("Intro") == 1)
                {
                    sendIndex = sceneIndex;
                }
                else
                {
                    sendIndex = sceneAlterIndex;
                    PlayerPrefs.SetInt("Intro", 1);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                sendIndex = sceneAlterIndex;
                PlayerPrefs.SetInt("Intro", 1);
                PlayerPrefs.Save();
            }


        }
        else
        {
            sendIndex = sceneIndex;
        }

        LoadLevel(sendIndex);
        
    }

    private void Update()
    {
        if(lGen && isLevelWithGeneration)
        {
            GeneratingLevelLoading();
            if(lGen.progress == 1f)
            {
                isLevelWithGeneration = false;
                loadingScreen.SetActive(false);
                //minimapUI.SetActive(true);
                //healthUI.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerPrefs.SetInt("Intro", 0);
            print("reset intro key");
        }
    }

    public void GeneratingLevelLoading()
    {
        slider.value = lGen.progress;
        progressText.text = lGen.loadingText;
    }

}
