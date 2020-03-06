using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonBase<SceneController>
{
    public int winner;

    public void SwitchSceneTo (string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetWinner (int winnerNum)
    {
        winner = winnerNum; 
    }
}
