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
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        player.velocity = new Vector2(joystick.Horizontal * movementSpeed, joystick.Vertical * movementSpeed);
    }
}
