using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Wave_Spawner wave_Spawner;
    public BaseBehaviour baseBehaviour;
    public static bool WaveInCorso = false;

    void Start()
    {

        //Probabilmente non per forza necessari...
        if (!PlayerPrefs.HasKey("actual_Wave"))
        {
            Debug.Log("Contatore wave settato a 0");
            PlayerPrefs.SetInt("actual_Wave", 0);
        }

        if (!PlayerPrefs.HasKey("health"))
        {
            Debug.Log("Contatore health settato a 0");
            PlayerPrefs.SetInt("health", 100);
        }

        /*if (!PlayerPrefs.HasKey("gold"))
        {
            PlayerPrefs.SetInt("gold", 0);
        }*/
    }

    public void SaveLevel()
    {
        //Tutto ciò che deve accadere al passaggio di livello, più...:

        /*PlayerPrefs.SetInt("actual_Wave", wave_Spawner.actual_Wave);
        Debug.Log("contatore Wave caricato a " + wave_Spawner.actual_Wave);

        PlayerPrefs.SetInt("health", baseBehaviour.health);
        Debug.Log("contatore health caricato a " + baseBehaviour.health);
        */

        //PlayerPrefs.SetInt("gold", enemy_HealthBar.gold);
    }

    public void ResetGame() //Da usare magari su un pulsante "Nuova partita" all'interno del menù di gioco, mentre un pulsante "Continua partita" caricherà semplicemente la scena del gioco e si continuerà quindi con i playerprefs salvati
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Salvataggi resettati");
        //SceneManager.LoadScene(0)
    }
}
