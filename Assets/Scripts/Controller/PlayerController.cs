using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private FixedJoystick joystick;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    public Animator animator;
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 movementDirection = new Vector2(joystick.Horizontal, joystick.Vertical);
        player.velocity = new Vector2(joystick.Horizontal * movementSpeed, joystick.Vertical * movementSpeed);

        if(joystick.Horizontal != 0 && joystick.Vertical != 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(joystick.Horizontal));
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }



        // if(joystick.Horizontal != 0 && joystick.Vertical != 0)
        // {
        //     Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
        //     transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        // }
        
    }
}
