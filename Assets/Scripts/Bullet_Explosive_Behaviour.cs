using UnityEngine;

public class Bullet_Explosive_Behaviour : MonoBehaviour
{
    private Vector3 target;   //il bersaglio del proiettile

    public float bulletSpeed = 70f; //definisce la velocità del proiettile

    public int damage = 20;     //definisce il danno del proiettile

    public GameObject explosionPrefab;  // il prefab dell'esplosione

    //variabili per movimento parabolico
    public float x = 0;
    public float y = 0;
    public float maxDistance;
    private Vector3 startingFlatPos;
    private Vector3 lastTargetPos;

    public void Seek(Transform _target)        //questo comando deve essere chiamato dichiarando sempre anche un Transform (utile perchè puoi chiamarlo da altri script, in questo caso è chiamato dalla torretta)
    {
        //Debug.Log("proiettile esplosivo creato");
        target = _target.position;               //il target di questo proiettile è settato sul punto chiamato dal comando Seek
    }

    private void Start()
    {
        startingFlatPos = new Vector3(transform.position.x,target.y, transform.position.z);   //conserva il punto da cui è partito il proiettile, altezza esclusa
    }

    void Update()
    {
        if (target == null)     //se il proiettile perde il target durante il volo (per esempio se viene distrutto)...
        {
            target = lastTargetPos;    //distruggi questo proiettile
        }

        Vector3 dir = target - transform.position;     //definisce la distanza tra il proiettile e il bersaglio, altezza inclusa
        float distanceThisFrame = bulletSpeed * Time.deltaTime; //definisce la distanza in cui ci muoviamo ogni frame

        //funzione per movimento parabolico        
        maxDistance = Vector3.Distance(target, startingFlatPos);//distanza tra bersaglio e punto di partenza del proiettile, altezza esclusa
        Vector3 flatPos = new Vector3(transform.position.x, target.y, transform.position.z);//posizione, altezza esclusa
        float flatDist = Vector3.Distance(flatPos, target);//ditanza tra bersaglio e il proiettile, altezza esclusa. 
        x = flatDist / maxDistance;                //ritorna la distanza dal bersaglio in un valore compreso tra 0 e 1
        //Debug.Log(x);
        //y = 0;  //solo per debug
        y = (-1*(x*x))+(3*x);                                   //usa quel valore come x nella funzione di moto parabolico, ritornando la y
        Vector3 yParab = new Vector3(0, y, 0);          //ottendo così un vettore sommabile al movimento lineare

        if (dir.magnitude <= distanceThisFrame)                 //se la distanza tra il proiettile e il bersaglio è minore della distanza che sarà percorsa il prossimo frame...
        {
            HitTarget();                                        //chiama il comando di "bersaglio colpito"
            return;
        }

        transform.Translate((dir.normalized * distanceThisFrame)+yParab, Space.World); //sposta il proiettile nella direzione del bersaglio

        lastTargetPos = target;        
    }

    void HitTarget()                                            //quando il bersaglio è colpito...
    {
        GameObject Explosion = (Instantiate(explosionPrefab, transform.position, Quaternion.identity)) as GameObject;//crea l'esplosione
        Explosion.GetComponent<Bullet_Explosion>().SetDamage(damage);//setta il danno dell'esplosione uguale al danno del proiettile

        //Debug.Log("Ho colpito qualcosa!");
        Destroy(gameObject);                                    //Poi distruggi questo proiettile
    }

    public void SetDamage(int dmgAmount)
    {
        damage = dmgAmount;
    }

}
