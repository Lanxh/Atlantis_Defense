using UnityEngine;

[System.Serializable]           //indica a Unity che è qualcosa che salva una informazione e che deve essere mostrata nell'inspector
public class Wave               //Monobehaviour rimosso perchè questo script accumula solo le info delle wave
{
    public WaveSprawl[] waveSprawls;   //contiene la lista di tutti i tipi di nemici che devono spawnare in questa ondata 
}
