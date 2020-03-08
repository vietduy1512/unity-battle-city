using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movement
{
    float horizontal, vertical;
    Rigidbody2D rigidBody;

    WeaponController wc;

    void Start()
    {
        wc = GetComponentInChildren<WeaponController>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (horizontal != 0 && !isMoving)
        {
            StartCoroutine(MoveHorizontal(horizontal, rigidBody));
        }
        else if (vertical != 0 && !isMoving)
        {
            StartCoroutine(MoveVertical(vertical, rigidBody));
        }
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            wc.Fire();
        }
    }
}
