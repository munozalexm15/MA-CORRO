using System.Collections;
using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    public ObjectPool objectPool;
    public GameObject coinGOContainer;

    private void Start()
    {
        coinGOContainer = transform.Find("CoinsContainer").gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<TileMetadata>().destroyOnComplete)
        {
            StartCoroutine(DelayedReturn(1f)); //Espera de 1 segundo
            return;
        }
        // Cuando el jugador pasa por el trigger, devolvemos el objeto al pool
        if (other.CompareTag("Player"))
        {
            if (coinGOContainer)
            {
                foreach (Transform child in coinGOContainer.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }

            objectPool.ReturnObject(gameObject, false);

            if (!objectPool.activeObjects.Contains(gameObject))
            {
                objectPool.activeObjects.Remove(gameObject);
            }

            // Obtener un objeto del pool
            GameObject obj = objectPool.GetObject();

            if (obj != null)
            {
                // Activar el movimiento hacia el jugador
                obj.GetComponent<MoveTowardsPlayer>().enabled = true;
            }
        }
    }

    void FixedUpdate()
    {
        GetComponent<MoveTowardsPlayer>().speed = objectPool.platformsSpeed;
    }

    private IEnumerator DelayedReturn(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);

    }
}
