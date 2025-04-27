using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    public GameManager gameManager; // Referencia al GameManager

    private void OnTriggerEnter(Collider other)
    {
        // Cuando el jugador entra en el trigger, activamos el objeto
        gameManager.ActivateObjectInTrigger(other);
    }
}
