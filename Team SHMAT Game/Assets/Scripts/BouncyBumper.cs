using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBumper : MonoBehaviour
{
    private GameManager manager;
    private AudioManager audioManagerScript;
    [SerializeField]
    private ParticleSystem particleEffect;
    public int particleCount = 30;
    public AnimationCurve bumpCurve;
    public float bumpDuration;
    public Vector3 bumpAmount;

    private float bumperForce = 7f;
    //private int comboStageHits = 5; //how many bounces are required for combo to advance to the next point-awarding stage

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        audioManagerScript = FindObjectOfType<AudioManager>();
        particleEffect = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            //other.rigidbody.AddExplosionForce(bumperForce, transform.position, 2, 0, ForceMode.Impulse);
            //other.rigidbody.AddForce(other.contacts[0].normal * bumperForce, ForceMode.Impulse);

            Vector3 dir = other.rigidbody.velocity.normalized;
            float mag = other.rigidbody.velocity.magnitude;

            dir = Vector3.Reflect(dir, other.contacts[0].normal);
            mag += bumperForce; 

            other.rigidbody.velocity = dir * mag;

            //award points to the player who shot the ball
            BallAttach ballScript = other.transform.GetChild(0).GetComponent<BallAttach>();
            GameObject playerAwarded = ballScript.lastHost; //find the ball's last host 
            PlayerMovement playerScript = playerAwarded.GetComponent<PlayerMovement>();
            int playerNum = playerScript.playerNum; //access the host's number

            manager.IncreaseCombo(1); 
            int points = DetermineScore();
            manager.AwardPointsToPlayer(points, playerNum); //award points to player for each bumper hit 

            DoEffects();
        }
    }

    [ContextMenu("Play Effects")]
    [ExecuteAlways]
    private void DoEffects()
    {
        StopAllCoroutines();
        StartCoroutine(BumpAnim());
        if (particleEffect) particleEffect.Emit(30);
    }

    [ExecuteAlways]
    IEnumerator BumpAnim()
    {
        float startTime = Time.time;
        Vector3 startScale = transform.localScale;
        float prog = 0;
        while (prog < 1)
        {
            prog = (Time.time - startTime) / bumpDuration;
            transform.localScale = Vector3.Scale(startScale, Vector3.one + (bumpCurve.Evaluate(prog) * bumpAmount));
            transform.localScale.Set(transform.localScale.x, startScale.y, transform.localScale.z);
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = startScale;
    }

    private int DetermineScore() //decided how many points to give out depending on combo count 
    {
        int combo = manager.comboCount;
        Debug.Log(combo);
        //Debug.Log(combo + "COMBO!");
        if (combo < 8) 
        {
            audioManagerScript.PlaySound("Bumper1");
            return 1; 
        }

        //if (combo < comboStageHits * 2) //alternative combo stage calculation
        if (combo < 16)
        {
            audioManagerScript.PlaySound("Bumper2");
            return 2;
        }

        if (combo < 23)
        {
            audioManagerScript.PlaySound("Bumper3");
            return 3;
        }

        if (combo < 30)
        {
            audioManagerScript.PlaySound("Bumper4");
            return 4;
        }

        audioManagerScript.PlaySound("Bumper5");
        return 5;
    }
}
