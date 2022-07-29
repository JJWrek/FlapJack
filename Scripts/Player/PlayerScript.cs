using UnityEngine;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D myBody;
    public Animator anim;
    public Collider2D floor;

    public AudioClip deathClip, flyClip, scoreClip;

    public float jumpForce;
    public float xSpeed;
    public float rotationUp, rotationDown;

    public int score;

    [HideInInspector]
    public bool hasGameStarted, hasPlayerDied;

    Vector3 playerRotation;

    /// <summary>
    /// Called when the player dies and alerts any listeners
    /// </summary>
    public UnityEvent OnPlayerDeath;

    void Update()
    {
        if (hasGameStarted && !hasPlayerDied)
        {
            FixPlayerRotation();
            XMovement();
        }

        // Check for movement input
        if (hasGameStarted && !hasPlayerDied)
        {
            JumpMovement();
        }
        else
        {
            if (myBody.velocity.y < -1 && !hasPlayerDied)
                myBody.velocity = Vector2.up * jumpForce * .365f;
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
            {
                hasGameStarted = true;
                if (!hasPlayerDied)
                    myBody.velocity = Vector2.up * jumpForce;
            }
        }

        SpeedUp();

    }
    /// <summary>
    /// Controls the speed of the game
    /// </summary>
    void SpeedUp()
    {
        if (score == 20)
            xSpeed = 2.0f;
        else if (score == 100)
            xSpeed = 2.5f;
        else if (score == 300)
            xSpeed = 3.0f;
        else if (score == 500)
            xSpeed = 3.5f;
    }

    /// <summary>
    /// Allows the player to jump
    /// </summary>
    void JumpMovement()
    {
        if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            myBody.velocity = Vector2.up * jumpForce;
            AudioSource.PlayClipAtPoint(flyClip, transform.position);
        }
    }
    /// <summary>
    /// Allows the player to move automatically
    /// </summary>
    void XMovement()
    {
        transform.position += new Vector3(Time.smoothDeltaTime * xSpeed, 0);
    }
    /// <summary>
    /// When the player jumps, the body of the player rotates 30 degrees up and then -90 degrees down
    /// </summary>
    void FixPlayerRotation()
    {
        float degreesToAdd = 0;

        if (myBody.velocity.y <= 0)
            degreesToAdd = rotationDown;
        if (myBody.velocity.y > 0)
            degreesToAdd = rotationUp;

        playerRotation = new Vector3(0, 0, Mathf.Clamp(playerRotation.z + degreesToAdd, -90, 30));
        transform.eulerAngles = playerRotation;
    }

    /// <summary>
    /// Fires when the player had died.
    /// </summary>
    void PlayerDies()
	{
        anim.SetBool("hasPlayerDied", true);
        hasPlayerDied = true;
        AudioSource.PlayClipAtPoint(deathClip, transform.position);

        if (OnPlayerDeath != null)
            OnPlayerDeath.Invoke();
    }

    /// <summary>
    /// When the player hits on of the spikes, the player dies. If the player passes between the 2 spikes, the player earns 1 point;
    /// </summary>
    /// <param name="target"></param>
     void OnTriggerEnter2D(Collider2D target)
    {
        if(target.CompareTag("Spike"))
        {
            PlayerDies();
        }

        if(target.CompareTag("SpikeScore"))
        {
            score++;
            PlayerPrefs.SetInt("Score", score);
            AudioSource.PlayClipAtPoint(scoreClip, transform.position);
        }
    }
    /// <summary>
    /// When the player hits the ground the player dies and falls through the ground
    /// </summary>
    /// <param name="target"></param>
     void OnCollisionEnter2D(Collision2D target)
    {
        if(target.gameObject.CompareTag("Ground"))
        {
            PlayerDies();
        }

        if (target.gameObject.CompareTag("Ground"))
        {
            if (hasPlayerDied)
                floor.enabled = false;
        }
    }
}
