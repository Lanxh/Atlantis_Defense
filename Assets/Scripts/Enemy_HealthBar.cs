using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;// per far funzionare la UI (la healthBar appunto)

public class Enemy_HealthBar : MonoBehaviour
{
    private Enemy_Animation _animScript;                //script dell'animazione

    private float startHealth = 100f;   //dichiara la variabile per la vita totale del nemico
    public float health = 100f;         //dichiara la variabile per la vita attuale del nemico
    private int prize = 1;              //dichiara la variabile per la taglia in denarao del nemico

    //single Canvas Healthbar
    public GameObject barPrefab;    //dichiara il prefab da usare per la barra della vita (da definire nell'inspector)
    public Image bar;            //dichiara l'immagine da usare per la barra della vita (protected così non appare nell'inspector)
    public Image barFilled;      //dichiara l'immagine da usare per il riempimento della barra della vita
    private Camera mainCamera;      //dichiara la camera di riferimento (quella principale)


    private bool isDying=false;         //variabile di controllo per evitare bug (Die() chiamato più volte per frame)

    void Start()
    {
        _animScript= GetComponent<Enemy_Animation>();//lo script di animazione contenuto in questo gameobject

        Wave_Spawner.enemiesAlive++;    //aumenta di uno (++ significa "aggiungi un +1") il contatore di nemici che sta nello script Wave_Spawner

        mainCamera = Camera.main;       //trova la camera principale (che è anche l'unica)

        startHealth = GetComponent<Enemy_Behaviour>().enemyStats.startingHealth;    //estrae dallo script Enemy behaviour la variabile della vita
        prize = GetComponent<Enemy_Behaviour>().enemyStats.reward;    //estrae dallo script Enemy behaviour la variabile della taglia in denaro del nemico

        // Debug.Log(Wave_Spawner.enemiesAlive + " Nemici vivi"); //stampa i nemici vivi (per debug)
        isDying = false;                //non sta morendo

        /*istanzia il prefab della barra al transform della canvas (siccome ce ne deve essere solo una findObject va bene). 
        Poi di quel prefab prendi l'immagine (GetComponent<Image>()) e assegnala alla variabile "bar" */
        bar = Instantiate(barPrefab, FindObjectOfType<Canvas>().transform).GetComponent<Image>();

        /*il riempimento della barra è assegnato creando una lista di tutti i componenti "Image" nella variabile "bar" e nei suoi figli che comprendono quindi 2
          immagini: il background e il fill. A noi ci serve solo il fill, quindi in quella lista prendiamo solo
          "quella che non appartiene a bar" (il comando è Find(img => img != bar))  */
        barFilled = new List<Image>(bar.GetComponentsInChildren<Image>()).Find(img => img != bar);

        health = startHealth;       //per far in modo che anche cambiando il valore della vita, la barra mostri la percentuale di vita correttamente

        bar.fillAmount = 0f;                //nasconde la barra finchè non prende danni
        barFilled.fillAmount = 0f;          //nasconde la barra finchè non prende danni

    }

    public void TakeDamage(int damage)  //questo comando deve essere chiamato dichiarando sempre anche un valore int (utile perchè puoi chiamarlo da altri script, in questo caso è chiamato dal proiettile che tocca il bersaglio)
    {
        health -= damage;           //sottrae alla vita il danno specificato dal comando TakeDamage
        
        bar.fillAmount = 1;                          //farà apparire la barra della vita
        barFilled.fillAmount = health / startHealth; //farà in modo che la barra della vita rifletta l'effettiva vita del nemico
        

        if ((health <= 0) &&  (isDying==false))          //se la vita scende a 0 e non è morto...
        {
            Die();                  //parte il comando di morte
            isDying = true;         //ora sta morendo, quindi non richiamerà questo script
        }
    }

    private void Die()                      //comando di morte (chiamato su TakeDamage() )
    {
        Wave_Spawner.enemiesAlive--;//Sottrai di uno (-- significa "sottrai di uno") il contatore di nemici che sta nello script Wave_Spawner 

        CapsuleCollider CapCollider = GetComponent<CapsuleCollider>();      //prende il capsule collider dell'oggetto
        CapCollider.enabled = false;                            //disabilita il collider

        //Debug.Log(Wave_Spawner.enemiesAlive + " Nemici vivi"); //stampa i nemici vivi (per debug)


        //Animazione di morte
        _animScript.DeathAnimation();
        
        GameObject.Find("GameManager").GetComponent<GoldManager>().ChangeMoney(prize);//trova il GameManager,prendi il component GoldManager e chiama il comando per cambiare i soldi(ChangeMoney)
        Destroy(bar);               //distruggi la barra
        //la distruzione dell'oggetto avviene alla fine dell'animazione
    }

    private void Update()
    {
        //WorldToScreenPoint trasforma un punto nel mondo in un punto sullo schermo (quindi perfetto per un canvas)
        if (bar != null)
        {
            bar.transform.position = mainCamera.WorldToScreenPoint(transform.position+new Vector3(0,3f,0));
            //Debug.Log($"fill amount:{bar.fillAmount}, {barFilled.fillAmount}");
        }

    }

    public void DestroyObject()
    {
        Debug.Log("oggetto distrutto");
        Destroy(gameObject);        //distruggi questo gameobject
    }



}
