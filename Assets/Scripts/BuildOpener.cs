using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildOpener : MonoBehaviour
{
    public GameObject Panel_Build;
    public GameObject Panel_Upgrade;


    public void OpenPanel()

    {

        Debug.Log("Funziono!");
        if (Panel_Build != null)
        {
            Debug.Log("FUNGE!");

            Panel_Build.SetActive(true);

            if(Panel_Upgrade == true)
            {
                
                Panel_Upgrade.SetActive(false);

            }


        }
    }
    
    public void ClosePanel()
    {
        
     
     Panel_Build.SetActive(false);
        
   
    }



 }
    
        
      
       

     



