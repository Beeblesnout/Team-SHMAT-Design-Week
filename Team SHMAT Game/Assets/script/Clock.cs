using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    float currentTime = 0f;
    float startingTime = 300f;

    [SerializeField] TextMeshProUGUI timer;

    void Start ()
    {
        currentTime = startingTime;
    }

    void Update ()
    {
        currentTime -= 1 * Time.deltaTime;
        timer.text = currentTime.ToString("0");
    }
}
