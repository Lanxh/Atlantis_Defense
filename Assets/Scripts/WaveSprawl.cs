using UnityEngine;

[System.Serializable]           //indica a Unity che è qualcosa che salva una informazione e che deve essere mostrata nell'inspector
public class WaveSprawl         //Monobehaviour rimosso perchè questo script accumula solo le info delle wave
{
    public GameObject enemy;    //quale nemico spawna
    public int count;           //quanti ne spawna
    public float rate=10f;      //il rateo di spawn tra l'uno e l'altro
    public Transform spawnPoint;//dove vengono spawnati
}
