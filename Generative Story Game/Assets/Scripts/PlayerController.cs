using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables that can be manipulated via ingredients
    public float jumpForce, dashForce, moveSpeed;
    public bool dash, wallJump;
    public int maxJumps;

    public Transform check; //Transform that is used to check if player is grounded

    LayerMask whatIsGround;

    int numJumps; //Number of jumps the player has made since being grounded
    
    Rigidbody2D playerRB;
    
    bool isGrounded, justDashed, dashRenew;
    // isGrounded: is the player touching the ground
    // justDashed: has the player dashed in the last 0.25 seconds
    // dashRenew: can the player dash again

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();

        whatIsGround = LayerMask.GetMask("Ground");

        justDashed = false; //Only matters if dash is turned on
    }

    void Update()
    {

        isGrounded = Physics2D.OverlapCircle(check.position, 0.2f, whatIsGround);

        if ((Input.GetKey("left") || Input.GetKey("right")) && !justDashed) //move left and right only if the player hasn't just dashed
        {
            playerRB.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, playerRB.velocity.y);
        }

        if (Input.GetKeyDown("z")) //jump
        {
            numJumps++;
            if (numJumps < maxJumps)
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        }

        
        if(Input.GetKeyDown("x") && dash && dashRenew) //player can dash if their dash is renewed and we enabled dash
        {
            dashRenew = false;
            StartCoroutine("ignoreDirection");
            playerRB.velocity = new Vector2(Mathf.Sign(playerRB.velocity.x) * dashForce, playerRB.velocity.y);
        }

        if (isGrounded) //renew dash and reset # of jumps upon landing on ground
        {
            dashRenew = true;
            numJumps = 0;
        }


    }

    //ignores direction input for 1/4 of a second after the player dashes.
    IEnumerator ignoreDirection()
    {
        justDashed = true;
        yield return new WaitForSeconds(0.25f);
        justDashed = false;

        if(Mathf.Abs(playerRB.velocity.x) > moveSpeed)
            playerRB.velocity = new Vector2(Mathf.Sign(playerRB.velocity.x) * moveSpeed, playerRB.velocity.y);
    }
}
