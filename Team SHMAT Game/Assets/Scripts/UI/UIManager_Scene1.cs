using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager_Scene1 : MonoBehaviour
{
    public GameObject startText;

    private float textShowTime = 1.2f;
    private float textHideTime = 0.5f;

    private bool animDone = true; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "StartMenu" && animDone)
        {
            StartCoroutine(TextFlash(startText));
            animDone = false;
        }
    }

    private IEnumerator TextFlash(GameObject text)
    {
        Debug.Log("flip this");
        text.SetActive(true);
        yield return new WaitForSeconds(textShowTime);
        text.SetActive(false);
        yield return new WaitForSeconds(textHideTime);
        animDone = true;
    }
}
