using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour {

    public Wave[] waves;
    public Transform objectToSpawn;

    Wave currentWave;
    int currentWaveNumber;

    int objectsRemainingToSpawn;
    int objectsToBeDestroyed;
    float nextSpawnTime;

    private void Start()
    {
        CallNextWave();
    }

    private void Update()
    {
        if (objectsRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            objectsRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            Transform spawnedObject = Instantiate(objectToSpawn, Vector3.zero, Quaternion.identity);

            if (currentWave.whenToSpawnNextWave == Wave.WhenToSpawnWave.OnWaveClear)        //decide when to call next wave
            {
                LivingEntity isLivingEntity = spawnedObject.GetComponent<LivingEntity>();       //Apply delegate if its a living entity
                if (isLivingEntity)
                {
                    isLivingEntity.OnDeath += OnObjectDestruction;
                }
            }
        }
    }

    void CallNextWave()     //TODO place timer function on this method or to call this method. Create SpawnNow functionality to call this method.
    {
        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            objectsRemainingToSpawn = currentWave.spawnCount;
            objectsToBeDestroyed = objectsRemainingToSpawn;

            if (currentWave.objectToSpawn != null) { objectToSpawn = currentWave.objectToSpawn; }
        }
    }

    void OnObjectDestruction()
    {
        objectsToBeDestroyed--;
        if (objectsToBeDestroyed <= 0)
        {
            CallNextWave();
        }
    }

    [System.Serializable] public class Wave
    {
        public int spawnCount;
        public float timeBetweenSpawns;

        public Transform objectToSpawn;

        public enum WhenToSpawnWave { OnObjectDestruction, OnWaveClear, OnTimer, OnMethodCall };
        public WhenToSpawnWave whenToSpawnNextWave;

    }
}
