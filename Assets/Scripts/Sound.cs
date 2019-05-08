using UnityEngine.Audio;//importante
using UnityEngine;

[System.Serializable]//quando si crea una custom class, non apparirà nell'inspector a meno che non la marchi come "Serializable"
public class Sound
{
    public string name;    //il nome da cercare negli altri script per ottenere questo suono specifico(modificato da volume e pitch)

    public AudioClip clip; //nome della clip da eseguire(non modificata)

    [Range(0f,1f)]         //crea uno slider per settare il volume
    public float volume;   //volume della clip
    [Range(0.1f, 3f)]      //crea uno slider per settare l'altezza
    public float pitch;    //altezza della clip

    public bool loop;      //decide se loopare o no l'audio

    [HideInInspector]      //anche se la variabile è pubblica, non mostrarla nell'inspector
    public AudioSource source;//la fonte della clip
}
