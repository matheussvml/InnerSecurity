using System.Collections.Generic;
using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager Instance;

    public List<Anomaly> anomalies = new List<Anomaly>();
    public float minSpawnTime = 10f;
    public float maxSpawnTime = 25f;

    private float nextSpawnTime;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomAnomaly();
            ScheduleNextSpawn();
        }
    }

    void ScheduleNextSpawn()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

void SpawnRandomAnomaly()
{
    List<Anomaly> inactive = anomalies.FindAll(a => !a.active);

    if (inactive.Count == 0)
        return;

    Anomaly selected = inactive[Random.Range(0, inactive.Count)];
    selected.Activate();

    Debug.Log($"[SPAWN] {selected.type} ativada na câmera {selected.cameraName}");
}


    public bool TryReport(string cameraName, string type)
    {
        Anomaly found = anomalies.Find(a => a.cameraName == cameraName && a.type == type && a.active);

        if (found != null)
        {
            found.Deactivate();
            return true;
        }

        return false;
    }

    public int GetActiveAnomalyCount()
{
    int count = 0;
    foreach (Anomaly a in anomalies)
    {
        if (a.active) count++;
    }
    return count;
}
}
