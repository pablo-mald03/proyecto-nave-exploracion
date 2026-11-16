using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Configuration")]
    public GameObject prefabOject;
    public Transform[] spawnPoints;

    void Start()
    {
        GenerateRandomObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateRandomObject()
    {
        if (prefabOject == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("You need to assign a prefab and some spawn points");
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedPoint = spawnPoints[randomIndex];

        Instantiate(prefabOject, selectedPoint.position, selectedPoint.rotation);
    }
}
