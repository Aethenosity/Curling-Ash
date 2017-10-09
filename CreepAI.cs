using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepAI : MonoBehaviour {

    public bool currentlyAttacking = false;
    public Transform target;
    public List<Transform> targetQueue;

    public float attackDelay = 1.0f;
    public float attackPower = 50.0f;
    public GameObject projectile;

    private float nextFire = 0.0f;
    private bool canAttackBase;
    private bool canAttack;
    private bool targetChanged;
    private bool emptyQueue;

    private GameObject _projectile;
    private TargetData _selfData;
    private CreepMover _selfMover;
    private GameObject _selfObject;
    //private AIPath aiPath;
    //private TargetChoice targetChoice;

    public virtual void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        targetQueue = new List<Transform>();
        canAttackBase = false;
        canAttack = false;
        targetChanged = false;
        emptyQueue = true;
        _selfData = GetComponent<TargetData>();
        _selfObject = gameObject;
        //aiPath = GetComponent<AIPath>();
        //targetChoice = GetComponent<TargetChoice>();
    }

    public void Update()
    {
        if (canAttack)
        {
            if (Time.time >= nextFire)
            {
                nextFire = Time.time + attackDelay;
                FireWeapon();
            }
        }
        else if (emptyQueue)
        {
            ResumeTask();
        }
        if (!_selfObject)
        {
            new GameObject();
        }
    }

    public void DecideCurrentTarget(Collider col)
    {
        tag = col.transform.tag;
        TargetData unkownObject = col.GetComponentInParent<TargetData>();
        //Debug.Log("UnkownObject is in team" + unkownObject.team);
        switch (tag)
        {
            case "Creep":
                //aiPath.canMove = false;
                //Debug.Log("!!Object In Trigger Is Creep");
                if (unkownObject.team != _selfData.team)
                {
                    canAttack = true;
                    if (target != null)
                    {
                        targetQueue.Add(col.transform);
                        emptyQueue = false;
                        break;
                    }
                    else
                    {
                        target = col.transform;
                        targetChanged = true;
                        FireWeapon();
                        emptyQueue = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            case "Base":
                //aiPath.canMove = false;
                canAttack = true;
                if (unkownObject.team != _selfData.team)
                {
                    if (target != null)
                    {
                        canAttackBase = true;
                        break;
                    }
                    else
                    {
                        target = col.transform;
                        targetChanged = true;
                        FireWeapon();
                        break;
                    }
                }
                else
                {
                    break;
                }
            case "Projectile":
                //Debug.Log("!!Object In Trigger Is Projectile");
                ApplyDamage(unkownObject.GetComponent<ProjectileData>().Damage);
                break;
                /*
                if (unkownObject.team != _selfData.team)
                {
                    ApplyDamage(unkownObject.GetComponent<ProjectileData>().Damage);
                    break;
                }
                else
                {
                    break;
                }*/
            default:
            break;
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        //Debug.Log("!Object Entered Trigger");
        DecideCurrentTarget(col);
    }

    public void OnTriggerExit(Collider col)
    {
        Debug.Log("Trigger Exit");
        if (col == target)
        {
            Debug.Log("Target Gone");
            if (targetQueue.Count == 0)
            {
                ResumeTask();
            }
            else if (targetQueue.Count > 0)
            {
                target = targetQueue[0];
                targetQueue.RemoveAt(0);
                if (targetQueue.Count == 0)
                {
                    ResumeTask();
                }
            }
        }/*
        else if (col == enemyBase)
        {
            // THIS IS WHERE GAMEOVER SCRIPT SHOULD BE CALLED
        }*/
    }

    Vector3 CalculateTargetVector()
    {
        if (!gameObject.transform)
        {
            gameObject.AddComponent<Transform>();
        }
        var heading = target.position - gameObject.transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance; // This is now the normalized direction.
        return direction;
    }

    void FireWeapon()
    {
        var direction = CalculateTargetVector();
        currentlyAttacking = true;
        //Debug.Log("!!!FIIIRRRIIIIIING");
        if (!projectile)
        {
            //make a new projectile..... somehow
        }
        // Instantiante projectile at the camera + 1 meter forward with camera rotation
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
        _projectile = newProjectile;
        // if the projectile does not have a rigidbody component, add one
        if (!newProjectile.GetComponent<Rigidbody>())
        {
            newProjectile.AddComponent<Rigidbody>();
        }
        // Apply force to the newProjectile's Rigidbody component if it has one
        newProjectile.GetComponent<Rigidbody>().AddForce(direction * attackPower, ForceMode.VelocityChange);
    }

    public void ApplyDamage(float Damage)
    {
        Debug.Log("!!!!Damage Applied = " + Damage);
        if (_selfData.selfHealth - Damage <= 0)
        {
            Destroy(_selfObject);
        }
        _selfData.selfHealth -= Damage;
    }

    public void ResumeTask()
    {
        currentlyAttacking = false;
        canAttack = false;
        canAttackBase = false;
        target = null;
        emptyQueue = true;
    }
}
