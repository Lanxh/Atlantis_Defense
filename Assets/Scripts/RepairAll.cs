using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairAll : MonoBehaviour
{
    public GameObject Povero_Panel;         //pannello di quando non hai abbastanza soldi (da definire nell'inspector)
    public List<GameObject> TorriInScena;   //lista delle torri in campo
    public int totRiparazioni=0;            //costo totale delle riparazioni
    public GameObject torrettaSelezionata;  //ultima torretta selezionata

    public void IndividuaTorretta(GameObject t)
    {
        torrettaSelezionata = t;
        //Debug.Log($"torretta {torrettaSelezionata} selezionata");
    }

    public void MonoRepair()                //ripara una torre
    {
        int moneyPossessed = GetComponent<GoldManager>().money;      //controlla quanti soldi ha il giocatore
        //Debug.Log($"monoRepair di {torrettaSelezionata} in corso");
        float mh = torrettaSelezionata.GetComponent<Turret_HealthBar>().missingHealth; //controlla quanta vita manca alla torretta
        int cr = torrettaSelezionata.GetComponent<Turret_LookAtRobot>().turretStats.costToRepair;//controlla quanto costa riparare un punto vita della torretta

        float totalCost = mh * cr; //costo per riparare la torretta al massimo

        if (moneyPossessed >= totalCost) //se i soldi posseduti sono di più del costo...
        {
            torrettaSelezionata.GetComponent<Turret_HealthBar>().RepairDamage();//ripara la torretta

            //sottrai i soldi
            GetComponent<GoldManager>().ChangeMoney(-Mathf.FloorToInt(totalCost));//trova il GameManager,prendi il component GoldManager e chiama il comando per cambiare i soldi(ChangeMoney)
            //Debug.Log($"{torrettaSelezionata.ToString()} riparata con successo!");
        }
        else                                        //se non hai abbastanza soldi...
        {
            if (!Povero_Panel.activeInHierarchy)    //se il pannello di avviso non è già visibile...
            {
                //Debug.Log("Sei povero");
                Povero_Panel.SetActive(true);       //...fai apparire il pannello di avviso
            }
        }

    }

    public void RepairEverything()
    {
        //trova il costo di riparazione di ogni torretta
        for(int i = 0; i<TorriInScena.Count; i++)  //per ogni elemento nella lista "TorriInScena"...
        {
            float mh = TorriInScena[i].GetComponent<Turret_HealthBar>().missingHealth; //controlla quanta vita manca alla torretta
            int cr = TorriInScena[i].GetComponent<Turret_LookAtRobot>().turretStats.costToRepair;//controlla quanto costa riparare un punto vita della torretta

            float totalCost = mh * cr; //costo per riparare la torretta al massimo
            totRiparazioni += Mathf.FloorToInt(totalCost);//aggiungi il costo alla variabile "costo totale riparazioni"
        }

        int moneyPossessed = GetComponent<GoldManager>().money;      //controlla quanti soldi ha il giocatore

        if (moneyPossessed >= totRiparazioni)       //se hai abbastanza soldi...
        {
            for(int i=0; i<TorriInScena.Count; i++)    //per ogni torretta nella lista...
            {
                TorriInScena[i].GetComponent<Turret_HealthBar>().RepairDamage();//...ripara la torretta
                Debug.Log($"Torretta {TorriInScena[i]} riparata");
            }

            //sottrai i soldi
            GetComponent<GoldManager>().ChangeMoney(-Mathf.FloorToInt(totRiparazioni));//trova il GameManager,prendi il component GoldManager e chiama il comando per cambiare i soldi(ChangeMoney)
            Debug.Log($"tutte le torrette riparate con successo!");
            //totRiparazioni = 0;     //resetta il costo delle riparazioni totali

        }
        else                                        //se non hai abbastanza soldi...
        {
            if (!Povero_Panel.activeInHierarchy)    //se il pannello di avviso non è già visibile...
            {
                //Debug.Log("Sei povero");
                Povero_Panel.SetActive(true);       //...fai apparire il pannello di avviso
            }
        }
    }

    public void addTorretta(GameObject tor)
    {
        TorriInScena.Add(tor);
        Debug.Log($"{tor.ToString()} aggiunta alla lista");
    }

    public void removeTorretta(GameObject tor)
    {
        TorriInScena.Remove(tor);
        Debug.Log($"{tor.ToString()} rimossa dalla lista");
    }

}
