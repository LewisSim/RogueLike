using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaTransition : MonoBehaviour
{
    public int sceneTarget;
    private TriggerHandler trigger;
    private GameObject levelGenerator;
    public bool DelveDeeper = false;
    public bool LoadBoss = false;

    private void Awake()
    {
        trigger = GetComponent<TriggerHandler>();
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator");
    }
    // Update is called once per frame
    void Update()
    {
        if(trigger.isTriggered)
        {
            if (Input.GetButton("Fire1") && trigger.triggeredBy.gameObject.tag == "Player")
            {
                if (levelGenerator && DelveDeeper)
                {
                    DontDestroyOnLoad(levelGenerator);
                    AdjustGenerator();
                    if (LoadBoss)
                    {
                        levelGenerator.GetComponent<LevelGen>().loadBossRoom = true;
                    }
                }
                SceneManager.LoadScene(sceneTarget);
                gameObject.GetComponent<SoundAtSource>().TriggerSoundAtUI();
            }
        }
    }

    private void AdjustGenerator()
    {
        LevelGen lGen = levelGenerator.GetComponent<LevelGen>();
        lGen.seed += Random.Range(1, 10);

    }

    private void RecordGeneratorSettings()
    {
        //Save gen settings for loading at later time
    }
}
