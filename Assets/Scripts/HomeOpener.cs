using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeOpener : MonoBehaviour
{
    public GameObject Panel_HomeScreen;
    public GameObject Panel_Pause;

    public void OpenPanel()
    {

        if (Panel_HomeScreen != null)
        {
            Panel_HomeScreen.SetActive(true);
            Panel_Pause.SetActive(false);
        }

    }
}