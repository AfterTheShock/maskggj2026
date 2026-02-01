using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmeySpawnerManager: MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public int baseEnemyCount = 5;
    public float multiplier = 1.7f;
    public float timeBetweenSpawns = 1.5f;

    public int currentWave = 1;
    public List<GameObject> activeEnemies = new List<GameObject>();

    private bool isSpawning = false;

    [SerializeField] float timeBetweenWaves = 4;

    [Header("Audio")]
    [SerializeField] private AudioClip[] clips;

    void Update()
    {
        activeEnemies.RemoveAll(item => item == null);

        if (!isSpawning && activeEnemies.Count == 0)
        {
            if (currentWave != 1)
            {
                UpgradesManager.Instance.ShowUpgradeScreen();
                
                AudioClip[] selectedClips = {  clips[1] };
                AudioManager.Instance.PlaySfx(selectedClips, transform, 1f, false, false, AudioReverbPreset.Psychotic);
            }
            StartCoroutine(PrepareNextWave());
        }
    }

    IEnumerator PrepareNextWave()
    {
        isSpawning = true;

        //if (currentWave != 1) UpgradesManager.Instance.ShowUpgradeScreen();

        if(currentWave != 1) yield return new WaitForSeconds(timeBetweenWaves);

        WaveUIManager.Instance?.ShowWave(currentWave);
        AudioClip[] selectedClips = {  clips[0] };
        AudioManager.Instance.PlaySfx(selectedClips, transform, 1f, false, false, AudioReverbPreset.Psychotic, 3f);

        int enemiesToSpawn = Mathf.RoundToInt(baseEnemyCount * Mathf.Pow(multiplier, currentWave - 1));

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        
        currentWave++;
        isSpawning = false;
    }

    [Header("Configuraci�n de Radio")]
    public float spawnRadius = 3f;

    void SpawnEnemy()
    {
        /*
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Vector3 centerPoint = spawnPoints[randomIndex].position;

        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPosition = centerPoint + new Vector3(randomCircle.x, 0, randomCircle.y);

        UnityEngine.AI.NavMeshHit hit;

        if (UnityEngine.AI.NavMesh.SamplePosition(randomPosition, out hit, spawnRadius, UnityEngine.AI.NavMesh.AllAreas))
        {
            randomPosition = hit.position;
        }*/

        Vector3 randomPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

        GameObject newEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

        newEnemy.transform.localScale = Vector3.one * Random.Range(0.95f,1.25f);

        activeEnemies.Add(newEnemy);

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        
        if (enemyScript != null)
        {
            enemyScript.enemyHealth *= (1 + ((currentWave-1) * 0.025f));
            enemyScript.enemyDamage *= (1 + ((currentWave-1) * 0.15f));
            enemyScript.enemySpeed *= (1 + ((currentWave-1) * 0.085f));
        }
    }

    void OnDrawGizmosSelected()
    {
        if (spawnPoints == null) return;
        Gizmos.color = Color.yellow;
        foreach (var sp in spawnPoints)
        {
            if (sp != null) Gizmos.DrawWireSphere(sp.position, spawnRadius);
        }
    }
}