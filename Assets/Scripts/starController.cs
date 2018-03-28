using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starController : MonoBehaviour
{
    private Rigidbody rb;
    GameObject player;
    GameObject trail;
    float offset = 1f;
    public float maxDistFromNim = 1.5f;
    public Vector3 speed = new Vector3(3, 0, 0);
    public float shootDistance = 5;

    private bool attack = false;
    private float nextFire;


    NimController playerScript;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Nim");
        playerScript = player.GetComponent<NimController>();
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

    void Update()
    {
        Vector3 destination = player.transform.position;
        destination.x = destination.x - offset * playerScript.direction;
        destination.y = destination.y + .2f;
        destination.z = destination.z - .2f;
        float dst = Vector3.Distance(destination, transform.position);
        if (dst > maxDistFromNim && !attack)
        {
            Vector3 vect = destination - transform.position;
            vect = vect.normalized;
            vect *= (dst - maxDistFromNim);
            transform.position += vect;
        }


    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Enemy")
        {
            Destroy(col.gameObject);
        }
        else
        {
            Physics.IgnoreCollision(col.collider, GetComponent<Collider>());
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 destination = player.transform.position;
        if (Input.GetKeyDown(KeyCode.X) && Time.time > nextFire)
        {
            attack = true;
            nextFire = Time.time + .5f;
            transform.position = destination;
        }

        if (attack)
        {
            shoot();
        }
        else
        {
            resetPosition();
        }

    }
    void resetPosition()
    {
        Vector3 destination = player.transform.position;
        destination.x = destination.x - offset * playerScript.direction;
        destination.y = destination.y + .2f;
        destination.z = destination.z - .2f;
        transform.position = Vector3.Lerp(rb.position, destination, 3 * Time.deltaTime);
    }
    public void hardResetPosition()
    {
        Vector3 destination = player.transform.position;
        destination.x = destination.x - offset * playerScript.direction;
        destination.y = destination.y + .2f;
        destination.z = destination.z - .2f;
        transform.position = destination;
    }
    public void shoot()
    {
        Vector3 destination = player.transform.position;
        destination.x = destination.x + shootDistance * playerScript.direction;
        destination.y = destination.y - .5f;
        transform.position = Vector3.Lerp(rb.position, destination, 8 * Time.deltaTime);
        if (Time.time > nextFire)
        {
            attack = false;
        }
    }
}
