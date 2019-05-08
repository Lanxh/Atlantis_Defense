using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public int money;
    private Text goldText;
    private string Moneystring;

    void Start()
    {
        goldText = GameObject.Find("Gold_Counter").GetComponent<Text>(); // trova il GameObject chiamato Gold_Counter e ne estrae il componente "testo".
        UpdateMoney();
    }

    void UpdateMoney()
    {
        Moneystring = "GOLD: " + money.ToString();
        goldText.text = Moneystring;
    }

    public void ChangeMoney(int deltaMoney)
    {
        money += deltaMoney;    //aggiungi o sottrai il valore inserito ai soldi del giocatore (money)
        UpdateMoney();          //aggiorna la UI in modo che rifletta i cambiamenti effettuati
    }

    public void SaveMoney()
    {
        //salva i soldi nei playerpref
    }
}