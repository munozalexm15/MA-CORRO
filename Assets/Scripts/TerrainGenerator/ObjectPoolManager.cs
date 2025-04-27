using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // El prefab del objeto que se va a instanciar
    public int poolSize = 10; // Número de objetos en la pool
    private Queue<GameObject> objectPool = new Queue<GameObject>(); // Cola de objetos disponibles
    public List<GameObject> activeObjects = new List<GameObject>(); //Lista de objetos activos
    void Start()
    {
        // Rellenamos la pool con objetos inactivos al inicio
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false); // Desactivamos los objetos por defecto
            objectPool.Enqueue(obj); // Los añadimos a la cola
            obj.GetComponent<TileTrigger>().objectPool = this;
        }
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

            // 🔥 Solo añadimos a la lista si se activa
            if (!activeObjects.Contains(obj))
            {
                activeObjects.Add(obj);
            }

            return obj;
        }
        else
        {
            return null;
        }
    }


    // Método para devolver un objeto a la pool
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false); // Desactivamos el objeto
        obj.GetComponent<MoveTowardsPlayer>().enabled = false;
        objectPool.Enqueue(obj); // Lo añadimos de nuevo a la cola
    }

    //Metodo que consulta la lista de objectos activos y los detiene
    public void StopMap() {
        foreach(GameObject obj in activeObjects) {
            MoveTowardsPlayer mover = obj.GetComponent<MoveTowardsPlayer>();
            if (mover != null)
            {
                obj.GetComponent<MoveTowardsPlayer>().canMove = false;
                Debug.Log(obj.GetComponent<MoveTowardsPlayer>().canMove);
            }
        }
    }
}
