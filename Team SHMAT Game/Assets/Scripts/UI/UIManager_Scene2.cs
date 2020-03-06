using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager_Scene2 : MonoBehaviour
{
    private SceneController sceneManager;
    public GameObject restartText;
    public Text winnerText; 

    private float textShowTime = 1.2f;
    private float textHideTime = 0.5f;

    private bool animDone = true;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = FindObjectOfType<SceneController>();

        if(SceneController.Instance.winner == 1)
        {
            winnerText.text = "BLUE WINS!";
        }
        else
        {
            winnerText.text = "RED WINS!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "WinScreen")
        {
            if (animDone)
            {
                StartCoroutine(TextFlash(restartText));
                animDone = false;
            }

            if (Input.anyKeyDown)
            {
                SceneController.Instance.SwitchSceneTo("StartMenu");
            }
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
