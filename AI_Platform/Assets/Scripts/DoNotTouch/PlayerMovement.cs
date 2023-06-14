using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*[SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 20f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpHeight = 2f;

    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Get player input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool jump = Input.GetButtonDown("Jump");
        bool sprint = Input.GetButton("Fire3");

        // Calculate movement direction and speed
        Vector3 move = transform.right * x + transform.forward * z;
        float speed = walkSpeed;
        if (sprint && isGrounded)
        {
            speed = sprintSpeed;
        }

        // Apply gravity and move the player
        Vector3 velocity = move.normalized * speed + Vector3.up * rb.velocity.y;
        rb.velocity = velocity;

        // Jump if the player is grounded
        if (jump && isGrounded)
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * gravity), ForceMode.VelocityChange);
        }
    }*/
    /*[SerializeField] private CharacterController controller;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 20f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpHeight = 2f;

    private bool isGrounded;
    private Vector3 velocity;

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get player input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool jump = Input.GetButtonDown("Jump");
        bool sprint = Input.GetButton("Fire3");

        // Calculate movement direction and speed
        Vector3 move = transform.right * x + transform.forward * z;
        float speed = walkSpeed;
        if (sprint && isGrounded)
        {
            speed = sprintSpeed;
        }

        // Apply gravity and move the player
        velocity.y += gravity * Time.deltaTime;
        move = move.normalized * speed * Time.deltaTime;
        controller.Move(move + velocity * Time.deltaTime);

        // Jump if the player is grounded
        if (jump && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }*/

        #region Variables
        public CharacterController controller;    
        float speed = 12f;
        public float walkSpeed = 5f;
        public float sprintSpeed = 20f;
        Vector3 velocity;
        public float gravity = -9.81f;
        public bool isGrounded; 
        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;
        public float jumpHeight = 2f;
        #endregion
        void Start()
        {

        }

        void Update()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        if (Input.GetButtonDown("Horizontal"))
        {
            //Debug.Log("HORIZONTAL PRESSED");
        }
        if(Input.GetButtonDown("Vertical"))
        {
            //Debug.Log("VERTICAL PRESSED");
        }

        if (Input.GetButtonDown("Jump") && isGrounded) //Press Space to jump

            {

                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            if (Input.GetButton("Fire3") && isGrounded) //Press Shift to sprint
            {
                speed = sprintSpeed;
            }
            else
            {
                speed = walkSpeed;
            }
        

    }
}

