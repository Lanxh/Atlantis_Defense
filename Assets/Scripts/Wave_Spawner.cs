using System.Collections;// per le coroutine
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;// per agire sulla UI

/*
 Le ondate (Waves) contengono più gruppi di nemici che spawnano contemporaneamente da punti diversi della mappa.
 Questi gruppi di nemici vengono chiamati "Sprawls".
*/

public class Wave_Spawner : MonoBehaviour
{
    /*dichiara la variabile che conterà quanti nemici vivi ci sono nel livello
    essendo statica, non ha bisogno di essere chiamata da GetComponent per essere modificata da altri script*/
    public static int enemiesAlive = 0;

    public int actual_Wave =-1;             //dichiara la variabile che conta in quale wave ci troviamo
    public int total_Wave = 10;             //dichiara la variabile che conta quante wave ci sono in totale nel livello
    public bool updated_Wave = true;       //dichiara la variabile che controlla se la wave è già stata aggiornata

    public GameObject Next_Wave_Button_GM;  //dichiara il gameobject che contiene il bottone Next Wave
    public Button button_next;              //dichiara il component "bottone" che verrà estratto dal gameObject sopracitato
    public Text text_counter;               //dichiara il component "testo" del bottone (il contatore dei nemici)
    public GameManager game_Man;

    [Header("Ondate da definire")]
    public Wave[] waves;             //lista di tutte le wave (è un array)
    private int waveSprawlIndex=0;   //indicatore dello sprawl attuale   

    void Start()
    {
        
        //bottone NextWave
        Next_Wave_Button_GM = GameObject.FindGameObjectWithTag("Wave_Button");  //trova il gameobject con la tag "Wave_Button" (da assegnare nell'inspector al pannello con il pulsante)
        button_next = Next_Wave_Button_GM.GetComponent<Button>();               //in quel gameobject prendi il component del "bottone"
        button_next.interactable = true;
        text_counter = Next_Wave_Button_GM.GetComponentInChildren<Text>();      //in quel gameobject prendi il primo component "testo" imparentato con lui
        game_Man = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();//trova il gameobject con la tag "GM" (da assegnare nell'inspector al Game Manager)

        //if (!PlayerPrefs.HasKey("actual_Wave"))                          //Se non c'è tra i playerprefs una key per health settala a 100...
        //{
        //    Debug.Log("nessuna ondata trovata");
        //}
        //else
        //{
        //    actual_Wave = (PlayerPrefs.GetInt("actual_Wave"))-1;                  //...al contrario settala al valore contenuto nel playerprefs
        //}

        UpdateWaveCounter();                //chiama il comando che aggiorna il contatore NextWave


    }

    void Update()
    {
        if (enemiesAlive > 0)       //se i nemici vivi sono più di 0...
        {
            button_next.interactable = false;   //...disattiva il bottone next_wave
            GameManager.WaveInCorso = true;     //avvisa che la wave è in corso

            if (updated_Wave == true)           //se la wave era stata aggiornata...
            {
                updated_Wave = false;           //... bisognerà riaggiornarla alla fine della wave
            }
        }
        else                        // se invece sono 0 o meno...
        {
            button_next.interactable = true;    //... riattiva il bottone next_wave
            GameManager.WaveInCorso = false;    //avvisa che non si è più in fase wave

            if (updated_Wave == false)          //se il contatore non è stato ancora aggiornato...
            {
                actual_Wave++;                      //... aumenta la wave attuale a +1 e...
                UpdateWaveCounter();                //... chiama il comando che aggiorna il contatore NextWave
                updated_Wave = true;                //infine dichiara che il contatore è stato aggiornato
                game_Man.SaveLevel();               //salva i dati della wave attuale, della vita della base e (ancora da fare) dei soldi
            }
        }
    }

    void UpdateWaveCounter()                    //comando che, quando chiamato, aggiorna il contatore NextWave
    {
        text_counter.text = (actual_Wave+1) + " / " + (total_Wave); //Aggiorna il contatore sul bottone NextWave
    }

    public void StartSpawnWave()                //per far iniziare la coroutine dello spawning dal bottone
    {
        StartCoroutine(SpawnWave());            //fa partire la coroutine (script che procede separato dal resto dello script)
    }

    IEnumerator SpawnWave()
    {
        Wave wave = waves[actual_Wave];                             //prendi dalla lista di ondate quella di questo round
        WaveSprawl[] waveSprawl = wave.waveSprawls;                 //da quella ondata, prendi la lista degli sprawls

        for (int i = 0; i < waveSprawl.Length; i++)                 //per ogni lista "Sprawl" nella lista "Wave"...
        {
            waveSprawlIndex = i;                                    //...salva l'indice dello sprawl attuale (serve per lo SpawnEnemy())
            SpawnEnemy(waveSprawl[i].enemy);                        //chiama il comando per spawnare il nemico definito nella lista
            yield return new WaitForSeconds(1/waveSprawl[i].rate);  //ripeti il comando dopo "rate" secondi (variabile presa dalla lista della wave attuale)
        }

    }

    void SpawnEnemy(GameObject enemyPrefab)
    {

        Wave wave = waves[actual_Wave];                             //prendi dalla lista di ondate quella di questo round
        WaveSprawl waveSprawl = wave.waveSprawls[waveSprawlIndex];  //prendi l'indice dello sprawl attuale
        for (int i = 0; i < waveSprawl.count; i++)                  //per ogni elemento nella lista...
        {
            float randomZ = Random.Range(0.3f,-0.3f);               //definisce un valore random per randomizzare leggermente i punti di spawn del nemico
            Vector3 randomVector = new Vector3(randomZ, 0, randomZ);//mette il valore random in un vector3 per poterlo usare nell'Instantiate qua sotto
            Instantiate(enemyPrefab, ((waveSprawl.spawnPoint.position)+randomVector), waveSprawl.spawnPoint.rotation);   //...spawna il nemico dichiarato nel punto dichiarato nell'inspector
        }
    }

    public void ResetLevel()
    {
        enemiesAlive = 0;
        SceneManager.LoadScene("Level01");




    }
}
