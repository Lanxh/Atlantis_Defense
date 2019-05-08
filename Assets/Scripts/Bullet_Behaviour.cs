using UnityEngine;

public class Bullet_Behaviour : MonoBehaviour
{
    private Transform target;   //il bersaglio del proiettile

    public float bulletSpeed = 70f; //definisce la velocità del proiettile

    public int damage = 20;     //definisce il danno del proiettile

    public void Seek (Transform _target)        //questo comando deve essere chiamato dichiarando sempre anche un Transform (utile perchè puoi chiamarlo da altri script, in questo caso è chiamato dalla torretta)
    {
        target = _target;               //il target di questo proiettile è settato sul punto chiamato dal comando Seek
    }

    void Update()
    {
        if (target == null)     //se il proiettile perde il target durante il volo (per esempio se viene distrutto)...
        {
            Destroy(gameObject);    //distruggi questo proiettile
            return;                 //poiché Destroy è un comando lento, return assicura che il codice non vada avanti prima che il comando non sia terminato
        }

        Vector3 dir = target.position - transform.position;     //definisce la distanza tra il proiettile e il bersaglio
        float distanceThisFrame = bulletSpeed * Time.deltaTime; //definisce la distanza in cui ci muoviamo ogni frame

        if (dir.magnitude <= distanceThisFrame)                 //se la distanza tra il proiettile e il bersaglio è minore della distanza che sarà percorsa il prossimo frame...
        {
            HitTarget();                                        //chiama il comando di "bersaglio colpito"
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World); //sposta il proiettile nella direzione del bersaglio
    }

    void HitTarget()                                            //quando il bersaglio è colpito...
    {
        if (target.tag == "Enemy")                              //se è un nemico...
        {
            Enemy_HealthBar HealthBarScript = target.GetComponent<Enemy_HealthBar>();   //...prendi lo script della vita del nemico...
            if (HealthBarScript != null)
            {
                HealthBarScript.TakeDamage(damage);                                         //... e chiama il comando per danneggiarlo 
            }
        }

        if (target.tag == "Tower_Pos")                          //se è una torretta...
        {
            //Prendi lo script della healthbar della torre
            //danneggia la torre
            Turret_HealthBar HealthBarScript = target.GetComponent<Turret_HealthBar>();//...prendi lo script della vita della torretta...
            if (HealthBarScript != null)
            {
                HealthBarScript.TakeDamage(damage);                                         //... e chiama il comando per danneggiarla 
            }
            //Debug.Log("Torre Danneggiata");
        }

        if (target.tag == "Base")                               //se è una base...
        {
            BaseBehaviour BaseHealtScript = target.GetComponent<BaseBehaviour>();//prendi lo script della heathbar della base
            if (BaseHealtScript != null)
            {

                BaseHealtScript.BaseTakeDamage(damage); //danneggia la base

            }
            //Debug.Log("Base Danneggiata");
        }
        //Debug.Log("Ho colpito qualcosa!");
        Destroy(gameObject);                                    //Poi distruggi questo proiettile
    }

    public void SetDamage(int dmgAmount)
    {
        damage = dmgAmount;
    }
}
