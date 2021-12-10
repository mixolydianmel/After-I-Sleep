using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables that can be manipulated via ingredients
    [Header("Movement Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;

    [Space(10)]

    [SerializeField] private bool dash;
    [SerializeField] private bool wallJump;

    [Space(10)]

    [SerializeField] private int maxJumps;

    [Header("Player Object Variables")]
    public Transform check; //Transform that is used to check if player is grounded

    LayerMask whatIsGround;

    int numJumps; //Number of jumps the player has made since being grounded
    
    Rigidbody2D playerRB;
    SpriteRenderer playerSR;
    
    bool isGrounded, justDashed, dashRenew;
    // isGrounded: is the player touching the ground
    // justDashed: has the player dashed in the last 0.25 seconds
    // dashRenew: can the player dash again

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();

        whatIsGround = LayerMask.GetMask("Ground");

        justDashed = false; //Only matters if dash is turned on
    }

    void Update()
    {

        isGrounded = Physics2D.OverlapCircle(check.position, 0.3f, whatIsGround);

        if (isGrounded) //renew dash and reset # of jumps upon landing on ground
        {
            dashRenew = true;
            numJumps = 0;

            if (playerRB.velocity.x < -0.05)
                playerRB.velocity = new Vector2(playerRB.velocity.x + acceleration, playerRB.velocity.y);
            
            else if (playerRB.velocity.x > 0.05)
                playerRB.velocity = new Vector2(playerRB.velocity.x - acceleration, playerRB.velocity.y);
            
            else
                playerRB.velocity = new Vector2(0, playerRB.velocity.y);
        }


        if (playerRB.velocity.x < 0) //flip X depending on where player is moving
            playerSR.flipX = true;
        else if (playerRB.velocity.x > 0)
            playerSR.flipX = false;


        if (Input.GetAxisRaw("Horizontal") != 0 && !justDashed) //move left and right only if the player hasn't just dashed
        {
            playerRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, playerRB.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space)) //jump
        {
            numJumps++;
            if (numJumps < maxJumps)
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && dash && dashRenew) //player can dash if their dash is renewed and we enabled dash
        {
            dashRenew = false;
            StartCoroutine("ignoreDirection");

            int dashDirection = 1;
            if (playerSR.flipX)
                dashDirection = -1;

            playerRB.velocity = new Vector2(dashDirection * dashForce, playerRB.velocity.y);
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
