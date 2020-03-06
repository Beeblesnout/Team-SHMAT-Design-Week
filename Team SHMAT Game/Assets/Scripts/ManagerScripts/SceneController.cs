using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{
    public static int winner;

    public static void SwitchSceneTo (string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void SetWinner (int winnerNum)
    {
        winner = winnerNum; 
    }
}
