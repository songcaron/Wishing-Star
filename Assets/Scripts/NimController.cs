using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimController : MonoBehaviour
{
    //variables involving movement speed
    public float speed;
    public float groundSpeed = 80;  //how fast Nim goes when he's on the ground
    public float airSpeed = 15;  //how fast our boi goes in the air
    public float jumpForce = 1000;  //force of jump
    public float maxSpeed = 15;  //maximum movement speed
    public float multiJumpForceMultiplier = 700;

    //directions/getting sprites to work
    public int direction = 1;
    public Quaternion lookLeft = Quaternion.Euler(0, 0, 0);
    public Quaternion lookRight = Quaternion.Euler(0, 180, 0);

    //variables for jumping
    public int maxJumps = 3;
    public int jumpCount;
    public float nextJumpTime;  //need an offset time between jumps
    public float jumpBuffer = .2f;
    public float gravityConstant = -50;

    private Animator animator;
    private Rigidbody rb;
    GameObject star;
    GameObject trail;

    enum states { defaultState, jumping, drop };
    states currentState;
    private bool jumping;

    // Use this for initialization
    void Start()
    {
        star = GameObject.Find("Star");
        trail = GameObject.Find("Trail");

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);  //set maximum velocity

        direction = 1;
        Physics.gravity = new Vector3(0, gravityConstant, 0);

        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        setAnimation();
        if (rb.velocity.y < 0)
        {
            Physics.gravity = new Vector3(0, gravityConstant, 0);
        }
        if (isOnGround())
        {
            Physics.gravity = new Vector3(0, gravityConstant, 0);
            currentState = states.defaultState;
            jumpCount = maxJumps;
        }


        
    }

    //for the physics
    private void FixedUpdate()
    {
        setSpeed();
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround())
        {
            jump();
        }
        

        //determing direction to face
        float mvmtHoriz = Input.GetAxis("Horizontal");
        float mvmtVert = Input.GetAxis("Vertical");
        walk(mvmtHoriz);
        if (Input.GetKeyDown(KeyCode.Z) && !isOnGround() && jumpCount > 0 && Time.time > nextJumpTime)
        {
            multiJump(mvmtHoriz, mvmtVert);
        }
        else if (Time.time < nextJumpTime && !isOnGround())
        {
            Vector3 mvmt = new Vector3(0, 30f, 0);
            rb.AddForce(mvmt);
        }

    }
    private void walk(float mvmtHoriz)
    {
        if (rb.velocity.magnitude > maxSpeed && isOnGround())
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        if (mvmtHoriz < 0 && direction == 1)
        {
            direction = -1;
            transform.rotation = lookRight;

        }
        else if (mvmtHoriz > 0 && direction == -1)
        {
            direction = 1;
            transform.rotation = lookLeft;
        }
        Vector3 movement = new Vector3(mvmtHoriz, 0, 0);
        rb.AddForce(movement * speed);
    }
    private void jump()
    {
        cancelMomentum();
        Debug.Log("jump()");
        float mvmtJump = jumpForce;
        float mvmtHoriz = Input.GetAxis("Horizontal");
        
        Vector3 movement = new Vector3(2f*mvmtHoriz, mvmtJump, 0);
        rb.AddForce(movement);
    }
    private void multiJump(float mvmtHoriz, float mvmtVert)
    {
        Debug.Log("multiJump()");
        currentState = states.jumping;
        cancelMomentum();
        Physics.gravity = new Vector3(0, gravityConstant * .65f, 0);
        star.GetComponent<starController>().hardResetPosition();
        Vector3 mvmt;
        if (mvmtVert > 0)
        {
            Debug.Log("1");
            mvmt = new Vector3(mvmtHoriz * .5f, mvmtVert, 0) - Physics.gravity / multiJumpForceMultiplier;
        }
        else if (mvmtVert == 0)
        {
            Debug.Log("2");
            if (mvmtHoriz == 0)
            {
                Debug.Log("2a");
                Debug.Log(Physics.gravity);
                mvmt = new Vector3(0, 1, 0) - Physics.gravity;
            }
            else
            {
                Debug.Log("2b");
                mvmt = new Vector3(mvmtHoriz * 2f, mvmtVert, 0) - Physics.gravity / multiJumpForceMultiplier;
            }

        }
        else if (mvmtVert < 0)
        {
            Debug.Log("eh");
            mvmt = new Vector3(mvmtHoriz * 2f, -2f, 0);
        }
        else
        {
            Debug.Log("oops");
            mvmt = new Vector3(0, 0, 0);
        }

        rb.AddForce(mvmt * multiJumpForceMultiplier);
        jumpCount--;
        nextJumpTime = Time.time + jumpBuffer;
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "DeathPlane" || col.gameObject.name == "Enemy")
        {
            death();
        }
    }

    public void death()
    {
        cancelMomentum();
        clearTrail();
        transform.position = new Vector3(0, 1, 0);
        star.transform.position = new Vector3(1, 1, 1);
    }

    private void cancelMomentum()
    {
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
    }
    private void setAnimation()
    {
        float mvmtHoriz = Input.GetAxis("Horizontal");
        if (mvmtHoriz != 0)
        {
            animator.SetInteger("State", 1);
        }
        else
        {
            animator.SetInteger("State", 0);
        }

        if (currentState != states.jumping)
        {
            clearTrail();
        }
    }
    bool isOnGround()
    {
        return Physics.Raycast(transform.position, -Vector3.up, .65f);
    }
    void clearTrail()
    {
        TrailRenderer tr = trail.GetComponent<TrailRenderer>();
        tr.Clear();
    }
    void setSpeed()
    {
        if (isOnGround())
        {
            speed = groundSpeed;
        }
        else
        {
            speed = airSpeed;
        }
    }
		
}
