using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TilePoolManager : MonoBehaviour
{
    [System.Serializable]
    public class TileType
    {
        public string name;            // nombre único, p. ej. "Grass" o "Road"
        public GameObject prefab;      // tu prefab de Unity
        public int initialPoolSize = 5;
    }

    public TileType[] tileTypes;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // 1) Crear la cola para cada tipo de tile
        foreach (var tileType in tileTypes)
        {
            var objectPool = new Queue<GameObject>();
            // 2) Rellenar la cola
            for (int i = 0; i < tileType.initialPoolSize; i++)
            {
                var obj = Instantiate(tileType.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            // 3) Añadir la cola al diccionario UNA VEZ
            poolDictionary.Add(tileType.name, objectPool);
        }
    }

    public GameObject SpawnTile(string tileName, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(tileName))
        {
            Debug.LogError($"[TilePoolManager] Tile type «{tileName}» no existe en el pool. Claves disponibles: {string.Join(", ", poolDictionary.Keys)}");
            return null;
        }
        var pool = poolDictionary[tileName];
        GameObject toSpawn = pool.Count > 0 ? pool.Dequeue() : Instantiate(tileTypes.First(t => t.name == tileName).prefab);
        toSpawn.SetActive(true);
        toSpawn.transform.position = position;
        return toSpawn;
    }

    public void ReturnTile(GameObject tile, string tileName)
    {
        if (poolDictionary.ContainsKey(tileName))
        {
            tile.SetActive(false);
            poolDictionary[tileName].Enqueue(tile);
        }
    }
}