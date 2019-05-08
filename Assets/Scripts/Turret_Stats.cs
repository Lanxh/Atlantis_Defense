using UnityEngine;

[System.Serializable]           //indica a Unity che è qualcosa che salva una informazione e che deve essere mostrata nell'inspector
public class Turret_Stats               //Monobehaviour rimosso perchè questo script accumula solo le info della torretta
{
    public float fireRate = 1f;         //definisce il rateo di fuoco della torretta (al secondo)
    public int damage = 10;             //quanto danno fa la torretta per colpo

    public GameObject bulletPrefab;     //il prefab che la torretta istanzierà come proiettile (da definire nell'inspector)

    public float startingHealth = 100f; //dichiara la variabile per la vita totale del nemico

    public int priceToBuy = 50;         //quanto costa cotruire la torretta
    public int costToRepair = 1;        //quanto costa riparare 1 punto vita della torretta

    public GameObject upgradedVersion;  //in quale torretta verrà potenziata (da definire nell'inspector)

    public float rotationSpeed = 180;   //di quanti gradi al secondo ruota quando guarda l'obbiettivo.
}
