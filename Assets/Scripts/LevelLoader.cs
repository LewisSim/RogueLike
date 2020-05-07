using System.Collections;
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


	public void LoadLevel (int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

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
    }

    public void GeneratingLevelLoading()
    {
        slider.value = lGen.progress;
        progressText.text = lGen.loadingText;
    }

}
