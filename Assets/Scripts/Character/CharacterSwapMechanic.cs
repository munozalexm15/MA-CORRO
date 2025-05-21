using UnityEngine;

public class CharacterSwapMechanic : MonoBehaviour
{
    public Transform charactersParent;
    private int currentCharacterIndex = 0;
    private GameObject[] characters;

    void Start()
    {
        int childCount = charactersParent.childCount;
        characters = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            characters[i] = charactersParent.GetChild(i).gameObject;
        }

        ActivateCharacter(currentCharacterIndex);
    }

    public void SwitchCharacter()
    {
        int nextIndex = (currentCharacterIndex + 1) % characters.Length;

        // Mover el nuevo personaje a la posición/rotación del actual
        characters[nextIndex].transform.position = characters[currentCharacterIndex].transform.position;
        characters[nextIndex].transform.rotation = characters[currentCharacterIndex].transform.rotation;
        

        // Activar el nuevo y desactivar el actual
        characters[currentCharacterIndex].SetActive(false);
        characters[nextIndex].SetActive(true);

        //resetear estado, booleanos y todo eso para que cambiar de personajes no de errores
        StateController nextCharController = characters[nextIndex].GetComponent<StateController>();
        nextCharController.currentLane = characters[currentCharacterIndex].GetComponent<StateController>().currentLane;
        if (nextCharController.currentState == nextCharController.fallingState || nextCharController.currentState == nextCharController.jumpingState)
        {
            nextCharController.ChangeState(nextCharController.fallingState);
        }
        else
        {
            nextCharController.ChangeState(nextCharController.runningState);
        }  

        currentCharacterIndex = nextIndex;
    }

    void ActivateCharacter(int index)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == index);
        }
    }
}
