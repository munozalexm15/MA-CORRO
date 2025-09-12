using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsAnimScript : MonoBehaviour
{
    private bool isSettingsOpen;
    private Animator settingsAnimator;

    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        settingsAnimator = gameManager.OptionsUI.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleSettings()
    {
        if (!isSettingsOpen)
        {
            isSettingsOpen = true;
            gameManager.MainMenuUI.transform.Find("Bottom").gameObject.SetActive(false);
            settingsAnimator.SetFloat("Speed", 1);
            settingsAnimator.Play("OpenCloseSettingsAnim", 0, 0f);
            
            
        }
        else
        {
            isSettingsOpen = false;
            settingsAnimator.SetFloat("Speed", -1);
            settingsAnimator.Play("OpenCloseSettingsAnim", 0, 1f);
            
            
            
            gameManager.MainMenuUI.transform.Find("Bottom").gameObject.SetActive(true);
        }
    }
    
}
