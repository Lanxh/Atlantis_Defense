using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Interactivity : MonoBehaviour
{

    private string nome; //nome del bottone
    private Button button;  //component bottone

    private void Start()
    {
        nome = name;
    }

    private void Update()
    {
        switch (nome)
        {
            case "RepairAll_Button":            //nel caso il nome dell'oggetto è questo...
                //Debug.Log("xxxxxx");
                button = GetComponent<Button>();    //estrai il componente bottone
                BtnInteractivityRA();                //...attiva il metodo BtnInteractivity (scritto qui sotto)
                break;
            case "Repair_Button":            //nel caso il nome dell'oggetto è questo...
                button = GetComponent<Button>();    //estrai il componente bottone
                BtnInteractivityR();                //...attiva il metodo BtnInteractivity (scritto qui sotto)
                break;

        }
    }

    private void BtnInteractivityRA()
    {
        if (GameManager.WaveInCorso == true)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    private void BtnInteractivityR()
    {
        //da aggiungere: "e se il valore di riparazione non è zero"
        if (GameManager.WaveInCorso == true)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }


}
