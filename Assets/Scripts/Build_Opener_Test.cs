using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Build_Opener_Test : MonoBehaviour
{
    public GameObject Panel_Build;
    public GameObject Panel_GameScene;

    public void OpenPanel(GameObject Target)
    {
        if (Panel_Build != null )
        {
            Debug.Log("FUNGE!");

            Panel_Build.SetActive(true);

        }
    }

    public void ClosePanel()
    {


        Panel_Build.SetActive(false);


    }



}