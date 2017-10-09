using UnityEngine;
using System.Collections;
using System;

public class SpawnGameObjects : MonoBehaviour
{
    public bool firstSpawn = false;

    // public variables
    public GameObject spawnObject; // what prefabs to spawn
    public Transform spawnPoint; // where to spawn creeps

    private GameObject _spawnObject;

    //spawn timing
    public float spawnDelay = 30.0f;
    private float timeSinceLastSpawn = 0.0f;

    //battle settings
    public int team = 0;

    private void Start()
    {
        spawnCreepWave();
    }

    // Update is called once per frame
    void Update()
    {
        // if time to spawn a new game object
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnDelay)
        {
            timeSinceLastSpawn = 0.0f;
            spawnCreepWave();
        }
    }

    void spawnCreepWave()
    {
        GameObject clone = Instantiate(spawnObject, spawnPoint.transform) as GameObject;
        _spawnObject = clone;
        clone.gameObject.GetComponent<TargetData>().team = team;
    }
}
