using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class BallScript : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI livesText;

    public int points;
    public int lives = 3;

    public BlockPlacer bp;
    private Rigidbody2D rb;
    
    public float ballSpeed;
    public float maxSpeed = 10f;
    public float minSpeed = 2f;

    public AudioSource scoreSound, blip;
    
    
    private int[] dirOptions = {-1, 1};
    private int hDir;
    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        bp = GameObject.Find("Blocks").GetComponent<BlockPlacer>();
        Reset(); 
    }


    // Start the Ball Moving
    public IEnumerator Launch() {
        yield return new WaitForSeconds(1.5f);
        
        // Figure out directions
        hDir = dirOptions[Random.Range(0, dirOptions.Length)];
        
        // Add a horizontal force
        rb.AddForce(transform.right * ballSpeed *.5f * hDir); // Randomly go Left or Right
        // Add a vertical force
        rb.AddForce(transform.up * ballSpeed * -1); // Force it to start going down
    }

    private void Reset() {
        rb.velocity = Vector2.zero;
        ballSpeed = 2;
        if (lives <= 0)
        {
            points = 0;
            lives = 3;
            bp.Reset();
        }

        SetText();
        transform.position = new Vector2(0, -1);
        StartCoroutine("Launch");
    }
    
    // if the ball goes out of bounds
    //or hits a block
    private void OnCollisionEnter2D(Collision2D other)
    {
        // did we hit a wall?
        if (other.gameObject.tag == "wall")
        {
            // make pitch lower
            blip.pitch = 0.75f;
            blip.Play();
            SpeedCheck();
        }

        if (other.gameObject.tag == "block")
        {
            blip.pitch = 1.1f;
            blip.Play();
            points++;
            SetText();
            Destroy(other.collider.gameObject);
            SpeedCheck();
        }
        if (other.gameObject.tag == "block1")
        {
            blip.pitch = 1.2f;
            blip.Play();
            points+=2;
            SetText();
            Destroy(other.collider.gameObject);
            SpeedCheck();
        }
        if (other.gameObject.tag == "block2")
        {
            blip.pitch = 1.3f;
            blip.Play();
            points+=5;
            SetText();
            Destroy(other.collider.gameObject);
            SpeedCheck();
        }

        // did we hit a paddle?
        if (other.gameObject.tag == "paddle")
        {
            // make pitch higher
            blip.pitch = 1f;
            blip.Play();
            SpeedCheck();
        }
        
        // did we hit the floor?
        if (other.gameObject.name == "BottomWall")
        {
            lives--;
            Reset();
        }
    }

    private void SpeedCheck() {
        
        // Prevent ball from going too fast
        if (Mathf.Abs(rb.velocity.x) > maxSpeed) rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y);
        if (Mathf.Abs(rb.velocity.y) > maxSpeed) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.9f);

        if (Mathf.Abs(rb.velocity.x) < minSpeed) rb.velocity = new Vector2(rb.velocity.x * 1.1f, rb.velocity.y);
        if (Mathf.Abs(rb.velocity.y) < minSpeed) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 1.1f);


        // Prevent too shallow of an angle
        if (Mathf.Abs(rb.velocity.x) < minSpeed) {
            // shorthand to check for existing direction
            rb.velocity = new Vector2((rb.velocity.x < 0) ? -minSpeed : minSpeed, rb.velocity.y);
        }

        if (Mathf.Abs(rb.velocity.y) < minSpeed) {
            // shorthand to check for existing direction
            rb.velocity = new Vector2(rb.velocity.x, (rb.velocity.y < 0) ? -minSpeed : minSpeed);
        }
        
        //Debug.Log(rb.velocity);

    }

    private void SetText()
    {
        int numZeroes = 3 - points.ToString().Length;
        string zeroes = "";
        for (int i=0;i<numZeroes;i++)
        {
            zeroes += '0';
        }

        pointsText.text = zeroes+points;
        
        numZeroes = 3 - lives.ToString().Length;
        zeroes = "";
        for (int i=0;i<numZeroes;i++)
        {
            zeroes += '0';
        }

        livesText.text = zeroes+lives;
    }

}
