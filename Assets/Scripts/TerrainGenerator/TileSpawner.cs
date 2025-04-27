using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public TilePoolManager tilePoolManager;
    public string[] availableTiles; // Nombres de los tipos de tiles
    public float tileLength = 10f;
    private Vector3 nextSpawnPoint = Vector3.zero;

    private void Start()
    {
        // Pre-spawn algunos tiles
        for (int i = 0; i < 5; i++)
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        string randomTileName = availableTiles[Random.Range(0, availableTiles.Length)];
        GameObject tile = tilePoolManager.SpawnTile(randomTileName, nextSpawnPoint);
        nextSpawnPoint += Vector3.back * tileLength;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TileEnd"))
        {
            SpawnTile();
            tilePoolManager.ReturnTile(other.transform.parent.gameObject, other.transform.parent.name);
        }
    }
}

