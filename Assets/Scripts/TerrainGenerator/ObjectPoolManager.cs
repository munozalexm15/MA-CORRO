using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> prefabs; // Ahora es una lista de prefabs
    public int poolSize = 10; // Número de objetos en la pool
    private Queue<GameObject> objectPool = new Queue<GameObject>(); // Cola de objetos disponibles
    public List<GameObject> activeObjects = new List<GameObject>(); // Lista de objetos activos

    void Start()
    {
        // Rellenamos la pool con objetos inactivos al inicio
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefabToInstantiate = prefabs[Random.Range(0, prefabs.Count)];
            GameObject obj = Instantiate(prefabToInstantiate);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
            obj.GetComponent<TileTrigger>().objectPool = this;
        }

        // Activamos uno de prueba si quieres
        GameObject activeObj = GetObject();
    }

    // Método para obtener un objeto de la pool
    public GameObject GetObject()
    {
        if (objectPool.Count > 0)
        {
            List<GameObject> objectList = new List<GameObject>(objectPool);
            int randomIndex = Random.Range(0, objectList.Count);
            GameObject obj = objectList[randomIndex];

            objectPool = new Queue<GameObject>(objectPool.Where(item => item != obj));

            obj.SetActive(true);
            obj.transform.position = transform.position;

            if (!activeObjects.Contains(obj))
            {
                activeObjects.Add(obj);
            }

            return obj;
        }
        else
        {
            // Si no hay objetos disponibles, instanciamos uno nuevo aleatorio
            GameObject prefabToInstantiate = prefabs[Random.Range(0, prefabs.Count)];
            GameObject newObj = Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
            newObj.GetComponent<TileTrigger>().objectPool = this;
            activeObjects.Add(newObj);
            return newObj;
        }
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
