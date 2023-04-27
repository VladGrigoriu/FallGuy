using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private FixedJoystick joystick;

    [SerializeField] private float movementSpeed;
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        player.velocity = new Vector2(joystick.Horizontal * movementSpeed, joystick.Vertical * movementSpeed);
    }
}
