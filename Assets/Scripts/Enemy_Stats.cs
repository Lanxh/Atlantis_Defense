using UnityEngine;

[System.Serializable]           //indica a Unity che è qualcosa che salva una informazione e che deve essere mostrata nell'inspector
public class Enemy_Stats        //Monobehaviour rimosso perchè questo script accumula solo le info delle statistiche del nemico
{
    public int startingHealth;  //con quanta vita parte          
    public int damage;          //quanto danno fa
    public float speed;         //quanto è veloce
    public int reward;          //quanti soldi da' alla sua morte
    public float fireRate=1f;   //velocità di fuoco
    public GameObject bulletPrefab; //il prefab che la torretta istanzierà come proiettile (da definire nell'inspector)
    public Transform firePoint; //il punto da cui si crea il proiettile

}
