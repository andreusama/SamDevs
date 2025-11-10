using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Tools;
using UnityEngine.Playables;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public TextMeshProUGUI text;

    public void Start()
    {
        //if (LevelTraveler.MyTravelInstance.gameData == null)
        //{
        //    MenuBrain.MyMenuBrain.SelectLevel(0);
        //    LoadLevel(1);
        //}
    }
    //0 menu
    //1 level
    public void LoadLevel(int index)
    {
        if (index == 1)
        {
            Debug.Log("SAVING from loading screen" + "GOLD: " + StatController.MyInstance.gameGold + "XP: " + StatController.MyInstance.GetXp());
            //save the game when entering a level in order to not loss progress
            LevelTraveler.MyTravelInstance.SerializeAndSave();
        }
        loadingScreen.SetActive(true);

        StartCoroutine(LoadAsynchronously(index));
    }

    //create a IEnumerator to load the scene asynchronously
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            text.text = progress * 100f + "%";
            yield return null;
        }
    }

    
}
