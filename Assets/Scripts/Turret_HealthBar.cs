using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;// per far funzionare la UI (la healthBar appunto)

public class Turret_HealthBar : MonoBehaviour
{
    [Header("Variabili da riempire")]
    public GameObject rovine;    //dichiara il prefab da usare per le rovine (da definire nell'inspector)
    public GameObject barPrefab;    //dichiara il prefab da usare per la barra della vita (da definire nell'inspector)

    [Header("Variabili autoriempienti")]
    private Turret_Stats sentryStats;   //statistiche della torretta
    private float startHealth = 100f;   //dichiara la variabile per la vita totale della torre
    public float health = 100f;         //dichiara la variabile per la vita attuale della torre
    public float missingHealth=0;       //variabile che altri script prendono per capire quanto riparare della torretta
    private int repairCost = 1;         //dichiara la variabile per il costo di riparazione
    private Camera mainCamera;          //telecamera di riferimento (quella principale)
    protected Image bar;            //dichiara l'immagine da usare per la barra della vita (protected così non appare nell'inspector)
    protected Image barFilled;      //dichiara l'immagine da usare per il riempimento della barra della vita
    private bool isDying = false;         //variabile di controllo per evitare bug (Die() chiamato più volte per frame)
    private GameObject standingPlatform;  //la piattaforma su cui poggia la torretta



    void Start()
    {
        mainCamera = Camera.main;       //trova la camera principale (che è anche l'unica)
        

        SetHealth();                    //funzione che setta la vita e la barra della vita della torretta

    }

    public void TakeDamage(int damage)  //questo comando deve essere chiamato dichiarando sempre anche un valore int (utile perchè puoi chiamarlo da altri script, in questo caso è chiamato dal proiettile che tocca il bersaglio)
    {
        health -= damage;           //sottrae alla vita il danno specificato dal comando TakeDamage

        bar.fillAmount = 1;                          //farà apparire la barra della vita
        barFilled.fillAmount = health / startHealth; //farà in modo che la barra della vita rifletta l'effettiva vita del nemico

        missingHealth = startHealth - health;       //calcola quanta vita manca

        if ((health <= 0) && (isDying == false))          //se la vita scende a 0 e non è morto...
        {
            Die();                  //parte il comando di morte
            isDying = true;         //ora sta morendo, quindi non richiamerà questo script
        }
    }

    public void RepairDamage()          //comando che ricarica la vita al massimo e nasconde di nuovo la barra
    {
        health = startHealth;           //ripara la torretta al max
        missingHealth = 0;              //resetta la vita mancante a 0

        bar.fillAmount = 0f;                //nasconde la barra finchè non prende danni
        barFilled.fillAmount = 0f;          //nasconde la barra finchè non prende danni

    }

    void Die()                      //comando di morte (chiamato su TakeDamage() )
    {
        /*DA AGGIUNGERE QUI:
        1)Animazione di morte
        */
        Tower_Choice turretBuildScript = GameObject.Find("GameManager").GetComponent<Tower_Choice>();
        turretBuildScript.CreateRubble(standingPlatform);//crea una rovina sulla piattaforma su cui è stata creata
        standingPlatform.GetComponent<Platform_Status>().StoreDestrTurret(this.gameObject);

        Turret_LookAtRobot turretBehaviourScript = GetComponent<Turret_LookAtRobot>();

        
        turretBehaviourScript.removeHimselfFromTarget();
        Destroy(bar);               //distruggi la barra
        //Destroy(gameObject);
        return;
    }


    private void Update()
    {
        //WorldToScreenPoint trasforma un punto nel mondo in un punto sullo schermo (quindi perfetto per un canvas)
        bar.transform.position = mainCamera.WorldToScreenPoint(transform.position + new Vector3(0, 3f, 0)); //posiziona la barra sotto la torretta

    }

    public void SetPlatform(GameObject ptf)
    {
        standingPlatform = ptf;     //salva la piattaforma su cui viene creata
    }

    public void SetHealth()
    {
        isDying = false;                //non sta morendo

        sentryStats = GetComponent<Turret_LookAtRobot>().turretStats;   //estrae le statistiche della torretta
        startHealth = sentryStats.startingHealth;   //estrae dallo script Enemy behaviour la variabile della vita
        health = startHealth;                       //per far in modo che anche cambiando il valore della vita, la barra mostri la percentuale di vita correttamente

        /*istanzia il prefab della barra al transform della canvas (siccome ce ne deve essere solo una findObject va bene). 
        Poi di quel prefab prendi l'immagine (GetComponent<Image>()) e assegnala alla variabile "bar" */
        bar = Instantiate(barPrefab, FindObjectOfType<Canvas>().transform).GetComponent<Image>();

        /*il riempimento della barra è assegnato creando una lista di tutti i componenti "Image" nella variabile "bar" e nei suoi figli che comprendono quindi 2
        immagini: il background e il fill. A noi ci serve solo il fill, quindi in quella lista prendiamo solo
        "quella che non appartiene a bar" (il comando è Find(img => img != bar))  */
        barFilled = new List<Image>(bar.GetComponentsInChildren<Image>()).Find(img => img != bar);

        bar.fillAmount = 0f;                //nasconde la barra finchè non prende danni
        barFilled.fillAmount = 0f;          //nasconde la barra finchè non prende danni

    }
}
