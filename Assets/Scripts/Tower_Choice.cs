using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Choice : MonoBehaviour
{
    public GameObject Build_Panel;          //pannello Build (da definire nell'inspector)
    public GameObject Upgrade_Panel;        //pannello Upgrade (da definire nell'inspector)
    public GameObject Rebuild_Panel;         //pannello Rebuild (da definire nell'inspector)
    public GameObject Povero_Panel;         //pannello di quando non hai abbastanza soldi (da definire nell'inspector)
    public GameObject rovine;               //Rovine (da definire nell'inspector)
    public GameObject[] Torri;              //lista delle torri spawnabili (da definire nell'inspector)
    public RepairAll repairScript;          //script di riparazione (da definire nell'inspector)

    private GameObject TempTarget;          //base selezionata al momento
    private Transform SpawnPoint;           //punto in cui dovrebbe spawnare la torretta
    private Platform_Status pStatus;        //se lo status della piattaforma è libero o occupato

    public int DivisionePrezzo = 2; //divisione che modifica i soldi in entrata in caso di vendita

    public void TowerSpawn (GameObject Target)
    {
        if (Target.tag == "TowerPos")
        {
            TempTarget = Target;
            pStatus = Target.GetComponent<Platform_Status>();
            SpawnPoint =Target.transform.Find("SpawnPoint");   //trova il transform del figlio della piattaforma, purchè sia chiamato "SpawnPoint"
            if (pStatus.turretOnTop == null)        //se non c'è una torretta sopra...
            {
                Build_Panel.SetActive(true);                        //...fai apparire il pannello Build
                Upgrade_Panel.SetActive(false);
            }
            if (pStatus.turretOnTop != null)        //se invece c'è una torretta sopra...
            {
                if (pStatus.turretOnTop.tag != "Rubble") //e se non è una rovina
                {
                    Upgrade_Panel.SetActive(true);                      //...fai apparire il pannello Upgrade
                    Build_Panel.SetActive(false);
                    repairScript.IndividuaTorretta(pStatus.turretOnTop);
                    if (pStatus.turretUpgraded == null)
                    {
                        Debug.Log("torretta Maxata");
                        //disabilita la possibilità di upgrade

                    }
                }
                else                    //se invece sopra ci sono rovine...
                {
                    Build_Panel.SetActive(false);
                    Upgrade_Panel.SetActive(false);
                    Rebuild_Panel.SetActive(true);

                    //Debug.Log("apertura pannello ricostruzione");
                }
            }
        }
    }


    public void TowerChoice(int NumeroTorre)
    {
        int price = Torri[NumeroTorre].GetComponent<Turret_LookAtRobot>().turretStats.priceToBuy;   //estrae il costo della torretta che si vuole costruire
        int moneyPossessed = GameObject.Find("GameManager").GetComponent<GoldManager>().money;      //controlla quanti soldi ha il giocatore

        if (moneyPossessed >= price)    //se il giocatore ha abbastanza soldi per poter comprare...
        {
            if (pStatus.turretOnTop == false)//controlla che la piattaforma non sia occupata
            {
                GameObject NewTower = (Instantiate(Torri[NumeroTorre], SpawnPoint.position, Quaternion.identity)) as GameObject;//crea la torre scelta nello spawnPoint che hai cliccato
                Build_Panel.SetActive(false);               //chiudi il pannello Build
                pStatus.changePlatformStatus(NewTower);     //imposta lo stato della piattaforma su "occupato"
                repairScript.addTorretta(NewTower);         //aggiungi la torretta alla lista delle torrette nel livello
                NewTower.GetComponent<Turret_HealthBar>().SetPlatform(TempTarget); //assegna la base di provenienza alla torretta


                //sottrai i soldi
                GameObject.Find("GameManager").GetComponent<GoldManager>().ChangeMoney(-price);//trova il GameManager,prendi il component GoldManager e chiama il comando per cambiare i soldi(ChangeMoney)
            }
        }
        else
        {
            if (!Povero_Panel.activeInHierarchy)    //se il pannello di avviso non è già visibile...
            {
                //Debug.Log("Sei povero");
                Povero_Panel.SetActive(true);       //...fai apparire il pannello di avviso
            }
        }
    }

    public void TowerUpgrade()
    {
        //Debug.Log("tower upgrade chiamato");
        int price = pStatus.turretUpgraded.GetComponent<Turret_LookAtRobot>().turretStats.priceToBuy; //estrae il costo della torretta che si vuole costruire
        int moneyPossessed = GameObject.Find("GameManager").GetComponent<GoldManager>().money;  //controlla quanti soldi ha il giocatore

        if (moneyPossessed >= price)    //se il giocatore ha abbastanza soldi per poter comprare...
        {
            //sostituzione della torretta con la versione potenziata
            repairScript.removeTorretta(pStatus.turretOnTop);   //rimuovi la torretta attuale alla lista delle torrette nel livello
            pStatus.turretOnTop.GetComponent<Turret_HealthBar>().RepairDamage();//resetta la barra della vita della torretta
            Destroy(pStatus.turretOnTop);                       //distruggi quella che c'è al momento
            GameObject NewUpTower = (Instantiate(pStatus.turretUpgraded, SpawnPoint.position, Quaternion.identity)) as GameObject;  //spawna la versione potenziata
            pStatus.changePlatformStatus(NewUpTower);          //aggiorna la piattaforma
            repairScript.addTorretta(NewUpTower);               //aggiungi la torretta alla lista delle torrette nel livello
            NewUpTower.GetComponent<Turret_HealthBar>().SetPlatform(TempTarget); //assegna la base di provenienza alla torretta
            Upgrade_Panel.SetActive(false);                      //fai sparire il pannello Upgrade

            //sottrai i soldi
            GameObject.Find("GameManager").GetComponent<GoldManager>().ChangeMoney(-price);//trova il GameManager,prendi il component GoldManager e chiama il comando per cambiare i soldi(ChangeMoney)
        }
        else
        {
            if (!Povero_Panel.activeInHierarchy)    //se il pannello di avviso non è già visibile...
            {
                //Debug.Log("Sei povero");
                Povero_Panel.SetActive(true);       //...fai apparire il pannello di avviso
            }
        }
    }

    public void CreateRubble(GameObject piattaforma)
    {
        pStatus = piattaforma.GetComponent<Platform_Status>();
        SpawnPoint = piattaforma.transform.Find("SpawnPoint");   //trova il transform del figlio della piattaforma, purchè sia chiamato "SpawnPoint"


        repairScript.removeTorretta(pStatus.turretOnTop);   //rimuovi la torretta attuale alla lista delle torrette nel livello
        GameObject NewRubble = (Instantiate(rovine, SpawnPoint.transform.position, Quaternion.identity))as GameObject;
        pStatus.changePlatformStatus(NewRubble); //aggiorna la piattaforma
        //DA AGGIUNGERE: inserimento dati della torre distrutta all'interno della rovina
        Debug.Log("Rovina creata");
    }

    public void RebuildRubble()
    {
        GameObject TwrDestroyed = pStatus.turretDestroyed;              //trova qual'è la torretta distrutta
        GoldManager goldMG = GameObject.Find("GameManager").GetComponent<GoldManager>();        //trova il GoldManager
        int price = TwrDestroyed.GetComponent<Turret_LookAtRobot>().GetPriceToRebuild();        //costo di ricostruzione
        int moneyPossessed = goldMG.money;      //controlla quanti soldi ha il giocatore
       

        if (moneyPossessed >= price)    //se il giocatore ha abbastanza soldi per poter comprare...
        {
            Turret_HealthBar NT_HealthBar = TwrDestroyed.GetComponent<Turret_HealthBar>();
            Destroy(pStatus.turretOnTop);               //distruggi le rovine

            TwrDestroyed.SetActive(true);                   //ricrea la torre scelta nello spawnPoint che hai cliccato
            NT_HealthBar.SetHealth();                   //resetta la sua vita
            Rebuild_Panel.SetActive(false);             //chiudi il pannello Build
            pStatus.changePlatformStatus(TwrDestroyed);     //imposta lo stato della piattaforma su "occupato"
            repairScript.addTorretta(TwrDestroyed);         //aggiungi la torretta alla lista delle torrette nel livello
            NT_HealthBar.SetPlatform(TempTarget);       //assegna la base di provenienza alla torretta

            //sottrai i soldi
            goldMG.ChangeMoney(-price);//trova il GameManager,prendi il component GoldManager e chiama il comando per cambiare i soldi(ChangeMoney)

        }
        else
        {
            if (!Povero_Panel.activeInHierarchy)    //se il pannello di avviso non è già visibile...
            {
                //Debug.Log("Sei povero");
                Povero_Panel.SetActive(true);       //...fai apparire il pannello di avviso
            }
        }
    }

    public void SellTower()
    {
        int price = pStatus.turretUpgraded.GetComponent<Turret_LookAtRobot>().turretStats.priceToBuy; //estrae il costo della torretta che si vuole costruire
        int moneyPossessed = GameObject.Find("GameManager").GetComponent<GoldManager>().money;
        GameObject TwrToSell = pStatus.turretOnTop;         //quale torretta è da vendere, salvata sul Platform_Status della piattaforma cliccata

        if (TwrToSell.tag != "Rubble")                  //se non stiamo vendendo una rovina...
        {
            repairScript.removeTorretta(TwrToSell);   //rimuovi la torretta attuale alla lista delle torrette nel livello
            Upgrade_Panel.SetActive(false);         //fai sparire il pannello Upgrade
        }
        else                                        //se invece è una rovina...
        {
            price = price / (DivisionePrezzo*2);    //setta il prezzo alla metà della vendita normale
            Rebuild_Panel.SetActive(false);         //fai sparire il pannello Upgrade
        }

        Destroy(TwrToSell);                     //distruggi quello che c'è sopra
        pStatus.Clear();                        //pulisci tutte le info conservate nella piattaforma
        GameObject.Find("GameManager").GetComponent<GoldManager>().ChangeMoney(+price);//aggiungi i soldi alla cassa del giocatore
        
    }
}

