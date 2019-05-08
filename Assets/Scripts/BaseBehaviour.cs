using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BaseBehaviour : MonoBehaviour
{
    [Header("Parametri base")]
    [Range(0, 100)] public int health;

    [Header("Oggetti UI da collegare")]
    [SerializeField] Text baseHealthBarText;
    [SerializeField] Slider baseHealthBarSlider;
    public GameObject gameover;

    void Start()
    {
        /////////////////////////////////////////////////////////////////// VALERIO /////////////////////////////////////////////////////////////////////////////////////////
        //if (!PlayerPrefs.HasKey("health"))                          //Se non c'è tra i playerprefs una key per health settala a 100...
        //{
        //    health = 100;
        //}
        //else
        //{
        //    health = PlayerPrefs.GetInt("health");                  //...al contrario settala al valore contenuto nel playerprefs
        //}

        health = 100;
        /////////////////////////////////////////////////////////////////// VALERIO /////////////////////////////////////////////////////////////////////////////////////////
        baseHealthBarSlider.value = health;                         //Eguaglia il valore dello Slider della barra della vita a quello di health
        gameover = GameObject.FindGameObjectWithTag("GameOver");
        gameover.SetActive(false);
    }

    
    void Update()
    {
        baseHealthBarText.text = "HP: " + health.ToString();        //Trasforma il valore intero di health in una stringa che viene utilizzata dall'elemento UI relativo
        baseHealthBarSlider.value = health;                         //Eguaglia il valore dello Slider della barra della vita a quello di health
    }

    public void BaseTakeDamage(int damage)
    {
        health -= damage;                                           //Sottrae ad health il valore preso da questo metodo qaundo viene richiamato

        if (health <= 0)                                            //Se la vita scende a 0 richiama la funzione di Game Over
        {
            GameOver();                                             //chiama la funziona del gameover
            health = 0;                                             //resetta la vita a 0
        }
    }

    void GameOver()
    {
        //To defined
        Debug.Log("Hai Perso");
        gameover.SetActive(true);                                   //attiva il pannello del GameOver
    }

   
}
