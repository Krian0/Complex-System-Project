using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private Vector3 movement;
    public float YVelocity;

    public float gravity = 9.81f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 1.5f;

    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float jumpSpeed;

    public bool use;
    public bool crouch;
    public bool run;
    public bool jump;

    public bool canMove;

	void Start ()
    {
        cc = GetComponent<CharacterController>();
        canMove = true;
	}
	
	private void Update ()
    {
        if (canMove)
        {

            if (cc.isGrounded && YVelocity <= 0)
                YVelocity = 0;

            movement = (transform.TransformDirection(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * getMoveSpeed()) + getInputY();

            use = Input.GetButton("Use");
            crouch = Input.GetButton("Crouch");
            run = Input.GetButton("Run") && !crouch;
            jump = Input.GetButton("Jump");


            if (Input.GetButtonDown("Jump") && cc.isGrounded)
                YVelocity = jumpSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
            cc.Move(movement * Time.fixedDeltaTime);
    }

    private float getMoveSpeed()
    {
        if (run)
            return runSpeed;
        if (crouch)
            return crouchSpeed;

        return walkSpeed;
    }

    private Vector3 getInputY()
    {
        YVelocity -= gravity * Time.deltaTime;

        //Allows "Mario Jumping": Fall faster, jump higher the longer you press the jump key.
        if (YVelocity < 0)
            YVelocity -= gravity * (fallMultiplier - 1) * Time.deltaTime;
        else if (YVelocity > 0 && !jump)
            YVelocity -= gravity * (lowJumpMultiplier - 1) * Time.deltaTime;

        return new Vector3(0, YVelocity, 0);
    }
}
