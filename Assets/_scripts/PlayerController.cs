using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	
	[HideInInspector]   //boolean used to determine if the player is facing right
	public bool isFacingRight = true;
	
	[HideInInspector]   //boolean used to determine if the player is touching the ground
	public bool isGrounded = false;
	
    [HideInInspector]   //force value that is applied when player jumps
	public float jumpForce = 650.0f;

    [HideInInspector]   //the max speed the player is allowed to move
    public float maxSpeed = 7.0f;

    [HideInInspector]   //boolean used to determine if double jump was used
    public bool doubleJump = false;

    [HideInInspector]   //boolean used to determine if the player is flying
    public bool isFlying = false; 
	
	public Transform groundCheck;
	public LayerMask groundLayers;
	
	private float groundCheckRadius = 0.2f;

    private Animator anim;
	
	void Awake()
	{
        anim = this.GetComponent<Animator>();
	}
	
	void Update()
	{
		if(Input.GetButtonDown("Jump")) //Jump and Double Jump feature
		{
            //when player is touching the ground
			if(isGrounded == true)
			{
                doubleJump = false; //resets double jump by setting it to false
				this.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,0);
				this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
                this.anim.SetTrigger("Jump");
			}
            //if the player is airborn and they haven't used their double jump
            else if (isGrounded == false && doubleJump == false)
            {
                doubleJump = true;  //set's double jumped as used by setting it's value to true
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce/2));
            }
		}
	}
	
	void FixedUpdate()
	{
		isGrounded = Physics2D.OverlapCircle
			(groundCheck.position, groundCheckRadius, groundLayers);
		try
		{
            float fly = Input.GetAxis("Vertical");
            float move = Input.GetAxis("Horizontal");

            //checks to see if player is attempting to fly by pressing the up/down key
            if (Input.GetButtonDown("Vertical"))
            {
                //isFlying is set to true to toggle to Flying controls
                isFlying = true;
            }
            // when the up/down key is released and the player is touching the ground
            if (Input.GetButtonUp("Vertical"))
            {
                if (isGrounded == true)
                {
                    //isFlying is set to false to toggle back to ground controls
                    isFlying = false;
                }
            }
            //ground Controls for whenever player isn't flying
            if (isFlying == false)
            {
                    this.GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
                this.anim.SetFloat("Speed", Mathf.Abs(move));
            }
            else //Flying Controls
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(move * (maxSpeed * 2), fly * maxSpeed);
            }

            //determines which way the player is facing and if they should get flipped
            if ((move > 0.0f && isFacingRight == false) || (move < 0.0f && isFacingRight == true))
			{
				Flip ();
			}
		}
		catch(UnityException error)
		{
			Debug.LogError(error.ToString());
		}
	}

    
    //flips character
	void Flip()
	{
		isFacingRight = !isFacingRight;						
		Vector3 playerScale = transform.localScale;			
		playerScale.x = playerScale.x * -1;					
		transform.localScale = playerScale;				    
	}
}