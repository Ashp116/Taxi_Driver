using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public Rigidbody controller;
    public float _gravity = -9.8f;

    private Vector3 velocity;
    

    private void FixedUpdate() {
        velocity.y += _gravity * Time.deltaTime;
        controller.AddForce(velocity * Time.deltaTime);
    }
}
