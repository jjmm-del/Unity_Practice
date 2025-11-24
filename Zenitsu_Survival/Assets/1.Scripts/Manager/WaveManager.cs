using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyGroup
{
    public GameObject demonPrefab;
    public int count;
}

[System.Serializable]
public class Wave
{
    public string waveName;
    public List<EnemyGroup> enemyGroups;
    public float timeBetweenSpawns = 0.5f;
    public float timeUntilNextWave = 5.0f;
}
public class WaveManager : MonoBehaviour
{
    [Header("웨이브 디자인")]
    [SerializeField] List<Wave> _waves;
    
    [Header("스폰 위치")]
    [SerializeField] Transform[] _spawnPoints;
    
    [Header("현재 스테이지")]
    private int _currentWaveIndex = -1;
    private List<DemonHealth> _activeDemons = new List<DemonHealth>();
    private bool _isSpawning = false;

    private void Start()
    {
        if (_spawnPoints.Length == 0)
        {
            Debug.LogError("[WaveManager] 스폰 포인트 설정이 되지 않았습니다");
            this.enabled = false;
            return;
        }

        StartNextWave();
    }

    private void StartNextWave()
    {
        _currentWaveIndex++;
        if (_currentWaveIndex >= _waves.Count)
        {
            Debug.Log("모든 웨이브를 클리어했습니다.");
            return;
        }
        Wave currentWave = _waves[_currentWaveIndex];
        StartCoroutine(SpawnWaveCoroutine(currentWave));
    }

    private IEnumerator SpawnWaveCoroutine(Wave wave)
    {
        Debug.Log(wave.waveName + "을 시작합니다.");
        _isSpawning = true;

        foreach (EnemyGroup group in wave.enemyGroups)
        {
            for (int i = 0; i < group.count; i++)
            {
                if (group.demonPrefab == null)
                {
                    Debug.LogError(wave.waveName + "에 프리펩이 없습니다.");
                    continue;
                }
                Transform randomPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                GameObject demonObject = Instantiate(group.demonPrefab, randomPoint.position, Quaternion.identity);
                DemonHealth demonHealth = demonObject.GetComponent<DemonHealth>();

                if (demonHealth != null)
                {
                    _activeDemons.Add(demonHealth);
                    demonHealth.OnDeath += HandleDemonDeath;
                }
                yield return new WaitForSeconds(wave.timeBetweenSpawns);
            }
                
        }

        _isSpawning = false;
    }

    private void HandleDemonDeath(DemonHealth deadDemon)
    {
        deadDemon.OnDeath -= HandleDemonDeath;

        _activeDemons.Remove(deadDemon);
        if (!_isSpawning && _activeDemons.Count == 0)
        {
            Wave currentWave = _waves[_currentWaveIndex];
            Debug.Log(currentWave.waveName + "클리어");

            StartCoroutine(NextWaveDelay(currentWave.timeUntilNextWave));
        }
    }

    private IEnumerator NextWaveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNextWave();
    }
    
}
