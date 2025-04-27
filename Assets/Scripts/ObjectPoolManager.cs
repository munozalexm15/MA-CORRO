using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // El prefab del objeto que se va a instanciar
    public int poolSize = 10; // Número de objetos en la pool
    private Queue<GameObject> objectPool = new Queue<GameObject>(); // Cola de objetos disponibles

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
        // Convertimos la cola a una lista para seleccionar un objeto aleatorio
        List<GameObject> objectList = new List<GameObject>(objectPool);
        
        // Seleccionamos un objeto aleatorio de la lista
        int randomIndex = Random.Range(0, objectList.Count);
        GameObject obj = objectList[randomIndex];
        // Eliminamos el objeto seleccionado de la cola
        objectPool = new Queue<GameObject>(objectPool.Where(item => item != obj));

        obj.SetActive(true); // Activamos el objeto
        return obj;
    }
    else
    {
        // Si no hay objetos disponibles, podemos instanciar uno nuevo o devolver null
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

}
