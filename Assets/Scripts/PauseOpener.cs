using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOpener : MonoBehaviour
{
    public GameObject Panel_Pause;
    public GameObject Panel_GameScene;

    public void OpenPanel()
    {
        if(Panel_Pause != null)
        {
            Panel_Pause.SetActive(true);
            Panel_GameScene.SetActive(false);
        }
    }

    public void ClosePanel()
    {
        if(Panel_Pause != null)
        {
            Panel_Pause.SetActive(false);
            Panel_GameScene.SetActive(true);
        }
    }

    
}

