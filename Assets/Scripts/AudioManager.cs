using UnityEngine.Audio;//importante
using System;
using UnityEngine;
/*LEGGIMI DANNAZIONE!
 Per far partire un audio seguire i seguenti step:
 1)Nell'inspector dell'AudioManager aprire la sezione "Sounds" => "size" e digitare il numero di suoni presenti nella scena.
 2)Si dovrebbe creare sotto una serie di elementi vuoti pari al numero nella size.
 3)In ognun elemento inserire la clip che si vuole eseguire e settarne le opzioni (volume, pitch, ecc).
 4)Scegliere un nome per ogni elemento. Non bisogna sbagliarlo perchè lo script cercherà quello per farlo partire.
 5)Per farlo partire da uno script esterno all'AudioManager, il comando è:
 --->  AudioManager.instance.Play("nome della clip");  <---
 
 Se invece lo si fa partire dall'AudioManager (per esempio per il tema principale del gioco) basta il comando
 --->  Play("nome della clip");  <--
   
     */

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;  //lista di tutti i suoni (è un Array)

    public static AudioManager instance; //sarà utilizzata per capire se c'è già un AudioManager nella scena

    private void Awake()    //prima di start, per ogni suono nella lista "sounds" crea un AudioSource
    {
        if (instance == null)           //se non c'è un AudioManager in questa scena...
        {
            instance = this;            //... ora c'è, ed è questo AudioManager!
        }
        else                            //se invece già c'è un AudioManager...
        {
            Destroy(gameObject);        //distruggilo!
            return;                     //impedisce che venga chiamato altro codice prima che l'oggetto venga distrutto
        }

        DontDestroyOnLoad(gameObject); //non distrugge l'AudioManager al cambio scena, permettendo di mantenere un audio fluido da una scena all'altra

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();  //crea l'AudioSource
            s.source.clip = s.clip;                             //definisce la clip

            s.source.volume = s.volume;                         //definisce il volume
            s.source.pitch = s.pitch;                           //definisce l'altezza
            s.source.loop = s.loop;                             //setta il loop a vero o falso
        }
    }

    public void Play(string name)                               //fa partire l'audio con il suo nome (vedi sotto)
    {
        //Nella variabile "s" inserisce il suono che noi abbiamo chiamato:
        //cerca all'interno dell'array "sounds" il sound DOVE (detto "=>" ) il nome del suono (sound.name) è uguale (==) al
        //nome chiamato dalla funzione (name).
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)                                          //se non trova audio con quel nome non ti da errore ma ti avvisa che non ha trovato nulla
        {
            Debug.LogWarning("Sound: " + name + " not found! Controlla di aver scritto bene il nome del suono nell'inspector.");
            return;
        }

        s.source.Play();                                        //esegue l'audio trovato con Array.Find
    } 

  
}
