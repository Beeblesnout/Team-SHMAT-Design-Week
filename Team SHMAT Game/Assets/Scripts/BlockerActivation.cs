using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerActivation : MonoBehaviour
{
    private AudioManager audioManagerScript;

    private GameObject panel;
    private bool blockerInCoolDown = false;
    private float panelActiveTime = 0.75f; //how long blocker panel stays up for 
    private float coolDownTime = 1.5f; //how long before blocker can be activated again 
    public bool player2;

    // Start is called before the first frame update
    void Start()
    {
        audioManagerScript = FindObjectOfType<AudioManager>();
        panel = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        GetInput(); 
    }

    private void GetInput()
    {
        if(!player2)
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                ActivateBlocker();
            }
        }
        else 
        {
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.J))
            {
                ActivateBlocker();
            }
        }
    }

    public void ActivateBlocker()
    {
        if (blockerInCoolDown) return;

        audioManagerScript.PlaySound("PadAction");
        blockerInCoolDown = true; 
        panel.SetActive(true);
        StartCoroutine(TimePanelRetrieval());
        StartCoroutine(TimeCoolDown());
    }

    private IEnumerator TimePanelRetrieval()
    {
        yield return new WaitForSeconds(panelActiveTime);
        audioManagerScript.PlaySound("PadHit");
        panel.SetActive(false);
    }

    private IEnumerator TimeCoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        blockerInCoolDown = false;
    }
}
