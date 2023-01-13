using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    /// <summary>
    /// Enemy Wave Structure
    /// </summary>

    [System.Serializable]
    public class Wave
    {
        // Delay
        public float timeBeforeWave;
        public List<GameObject> enemies;
    }

    // Enemy will get different speed in each wave
    public float speedRandomizer = 0.2f;

    // Enemy spawn delay
    public float unitSpawnDelay = 0.5f;

    // List of waves
    public List<Wave> waves;

    // Pathway enemies follow
    private Pathway pathway;

    // Nearest wave
    private Wave nextWave;

    // Delay counter
    private float counter;

    // Wave started
    private bool waveInProgress;

    // List for random enemy generation
    private List<GameObject> enemyPrefabs;

    // Buffer with active spawned enemies
    private List<GameObject> activeEnemies = new List<GameObject>();

    // If enemy not set, this folder will use to get random enemy
    public string enemiesResourceFolder = "Prefabs/Enemies";

    /// <summary>
    /// Awake the instance
    /// </summary>
    void Awake()
    {
        pathway = GetComponentInParent<Pathway>();
        enemyPrefabs = Resources.LoadAll<GameObject>(enemiesResourceFolder).ToList();
        // Debug.Log(enemyPrefabs.Count());
    }

    void OnEnable()
    {
        EventManager.StartListening("UnitDie", UnitDie);
    }

    void OnDisable()
    {
        EventManager.StopListening("UnitDie", UnitDie);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (waves.Count > 0)
        {
            // Start from first wave
            nextWave = waves[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Wait for next wave
        if ((nextWave != null) && (waveInProgress == false))
        {
            counter += Time.deltaTime;
            if (counter >= nextWave.timeBeforeWave)
            {
                counter = 0f;
                // Start new wave
                StartCoroutine(RunWave());
            }
        }
        // If all spawned enemies are dead
        if ((nextWave == null) && (activeEnemies.Count <= 0))
        {
            EventManager.TriggerEvent("AllEnemiesAreDead", null, null);
            // Turn off spawner
            enabled = false;
        }
    }

    private void GetNextWave()
    {
        int idx = waves.IndexOf(nextWave) + 1;
        if (idx < waves.Count)
        {
            nextWave = waves[idx];
        }
        else
        {
            nextWave = null;
        }
    }

    private IEnumerator RunWave()
    {
        waveInProgress = true;
        foreach (GameObject enemy in nextWave.enemies)
        {
            GameObject prefab = null;
            prefab = enemy;
            // If enemy prefab not specified - get random enemy
            if (prefab == null)
            {
                prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            }
            // Create enemy
            GameObject newEnemy = Instantiate(prefab, transform.position, transform.rotation);
            // Set pathway
            newEnemy.GetComponent<AIStatePatrol>().path = pathway;
            NavAgent agent = newEnemy.GetComponent<NavAgent>();
            // Set speed offset
            agent.speed = Random.Range(agent.speed * (1f - speedRandomizer), agent.speed * (1f + speedRandomizer));
            // Add enemy to list
            activeEnemies.Add(newEnemy);
            // Wait for delay before next enemy run
            yield return new WaitForSeconds(unitSpawnDelay);
        }
        GetNextWave();
        waveInProgress = false;
    }

    private void UnitDie(GameObject obj, string param)
    {
        // If this is active enemy
        if (activeEnemies.Contains(obj) == true)
        {
            // Remove it from buffer
            activeEnemies.Remove(obj);
        }
    }
}
