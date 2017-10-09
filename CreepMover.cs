using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepMover : MonoBehaviour
{

    public float speed;
    public Transform target;

    private bool canMove = true;

    private CreepAI _selfAI;

    private void Awake()
    {
        _selfAI = GetComponent<CreepAI>();
        canMove = true;
        if (speed <= 0.0f)
        {
            speed = 3.0f;
        }
    }

    void Update()
    {
        if (_selfAI.currentlyAttacking) return;
        if (canMove)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
    }

    public void StopMovement()
    {
        canMove = false;
    }

    public void ResumeMovement()
    {
        canMove = true;
    }
}
