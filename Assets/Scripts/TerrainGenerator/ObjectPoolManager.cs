using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> prefabs; // Ahora es una lista de prefabs

    private Dictionary<TileMetadata.TileType, List<GameObject>> prefabsByType = new Dictionary<TileMetadata.TileType, List<GameObject>>();
    private TileMetadata.TileType currentType;
    private int tilesRemainingInGroup = 0;
    public int minGroupSize = 3;
    public int maxGroupSize = 6;
    
    public int poolSize = 10; // Número de objetos en la pool
    private Queue<GameObject> objectPool = new Queue<GameObject>(); // Cola de objetos disponibles
    public List<GameObject> activeObjects = new List<GameObject>(); // Lista de objetos activos

   void Start()
    {
        // Agrupar prefabs por tipo
        foreach (GameObject prefab in prefabs)
        {
            TileMetadata meta = prefab.GetComponent<TileMetadata>();
            if (meta == null) continue;

            if (!prefabsByType.ContainsKey(meta.tileType))
            {
                prefabsByType[meta.tileType] = new List<GameObject>();
            }
            prefabsByType[meta.tileType].Add(prefab);
        }

        // Seleccionamos un tipo inicial
        currentType = GetRandomTileType();
        tilesRemainingInGroup = Random.Range(minGroupSize, maxGroupSize + 1);

        // Inicializar pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefabToInstantiate = GetRandomPrefabOfType(currentType);
            GameObject obj = Instantiate(prefabToInstantiate);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
            obj.GetComponent<TileTrigger>().objectPool = this;
        }

        // Activar uno de prueba
        GameObject activeObj = GetObject();
    }


    // Método para obtener un objeto de la pool
    public GameObject GetObject()
    {
        // Ver si cambiamos de tipo
        if (tilesRemainingInGroup <= 0)
        {
            currentType = GetRandomTileType();
            tilesRemainingInGroup = Random.Range(minGroupSize, maxGroupSize + 1);
        }

        GameObject obj = null;

        // Buscar uno en la pool que sea del tipo actual
        foreach (GameObject pooled in objectPool)
        {
            TileMetadata meta = pooled.GetComponent<TileMetadata>();
            if (meta != null && meta.tileType == currentType)
            {
                obj = pooled;
                break;
            }
        }

        if (obj != null)
        {
            objectPool = new Queue<GameObject>(objectPool.Where(item => item != obj));
        }
        else
        {
            // Instanciar uno nuevo del tipo actual
            GameObject prefabToInstantiate = GetRandomPrefabOfType(currentType);
            obj = Instantiate(prefabToInstantiate);
            obj.GetComponent<TileTrigger>().objectPool = this;
        }

        obj.SetActive(true);
        obj.transform.position = transform.position;

        if (!activeObjects.Contains(obj))
            activeObjects.Add(obj);

        tilesRemainingInGroup--;

        return obj;
    }

    private TileMetadata.TileType GetRandomTileType()
    {
        TileMetadata.TileType[] types = prefabsByType.Keys.ToArray();
        return types[Random.Range(0, types.Length)];
    }

    private GameObject GetRandomPrefabOfType(TileMetadata.TileType type)
    {
        List<GameObject> list = prefabsByType[type];
        return list[Random.Range(0, list.Count)];
    }



    // Método para devolver un objeto a la pool
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.GetComponent<MoveTowardsPlayer>().enabled = false;
        objectPool.Enqueue(obj);
    }

    // Método que consulta la lista de objetos activos y los detiene
    public void StopMap()
    {
        foreach (GameObject obj in activeObjects)
        {
            MoveTowardsPlayer mover = obj.GetComponent<MoveTowardsPlayer>();
            if (mover != null)
            {
                mover.canMove = false;
                Debug.Log(mover.canMove);
            }
        }
    }
}
