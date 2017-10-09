using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData : MonoBehaviour {

    public float Damage = 5.0f;
    public float killDelay = 1.5f;

    private int team;
    private float startTime = 0.0f;

    public void Start()
    {
        startTime = Time.time;
        team = GetComponent<TargetData>().team;
    }

    public void Update()
    {
        if (Time.time >= startTime + killDelay)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
    }
}
