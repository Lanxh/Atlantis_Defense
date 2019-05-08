using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_LookAtRobot : MonoBehaviour
{
    [Header("Stat torretta (da impostare)")]           //l'Header crea nell'inspector un'interfaccia più ordinata, dividendo i campi e titolandoli
    public Turret_Stats turretStats;    //serve per recuperare le stat (da definire nell'inspector)

    [Header("Variabili da non toccare")]
    public List<GameObject> robots;     //crea una lista di gameobject, chiamata robots.
    public Transform partToRotate;      //definisce quale è la mesh di questo prefab da ruotare (da definire nell'inspector)
    public Transform _target;           //Il transform del bersaglio, inizialmente nullo.

    public Transform firePoint;         //il punto da cui si crea il proiettile
    private float fireCountdown=0;      //variabile necessaria per il timer tra un colpo e l'altro

    void Start()
    {
        robots = new List<GameObject>();        //inizializza la lista, con niente dentro.
        InvokeRepeating("UpdateTarget", 0f, 0.5f);      //trasforma la funzione UpdateTarget in una simile all'Update, ma solo 2 volte al secondo invece che a ogni frame
    }

    void UpdateTarget()                                 //come Update ma solo due volte al secondo
    {
        if (robots.Count >= 1)                          //se i robot nell'area non sono 0...
        {
            float shortestDistance = Mathf.Infinity;    //definisce la variabile per la distanza più corta e lo setta a infinito
            GameObject nearestEnemy = null;             //definisce la variabile per il nemico più vicino e lo setta su null

            robots.RemoveAll(robot => robot == null);   //pulisce la lista di eventuali robot distrutti

            foreach (GameObject robot in robots)        //per ogni robot nell'area (quindi quelli nella lista)...
            {
                float distanceToRobot = Vector3.Distance(transform.position, robot.transform.position); //...calcola la distanza tra il robot e la torre
                if (distanceToRobot < shortestDistance) //se la distanza è minore delle distanze minori precedenti...    
                {
                    shortestDistance = distanceToRobot; //...settala come nuova distanza minore
                    nearestEnemy = robot;               //e il robot diventa il nemico più vicino
                }
            }

            if (nearestEnemy != null)                   //se il nemico più vicino non è nullo...
            {
                _target = nearestEnemy.transform;       //...settalo come bersaglio
            }
        }
        else
        {
            //Debug.Log("Nessun robot in area");          //altrimenti manda un messaggio di avviso
            _target = null;                             //e rimuovi il target
        }
    }

    void Update()
    {
        if (_target == null)        //se non c'è target...
        {
            return;                 //...non fare nulla
        }

        //la torretta guarda il target
        Vector3 dir = _target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,lookRotation,Time.deltaTime*turretStats.rotationSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f,rotation.y,0f);

        Debug.DrawRay(firePoint.position,dir*0.8f,Color.red);

        //shooting
        if (fireCountdown <= 0f)                //se il timer scende a 0...
        {
            Shoot();                                        //...attiva il comando "Spara"...
            fireCountdown = 1f / turretStats.fireRate;      //... e resetta il timer
        }

        fireCountdown -= Time.deltaTime;        //il timer cala con il tempo
    }

    private void Shoot()                                //comando che instanzia il proiettile
    {
        //istanzia un proiettile(bulletPrefab), alla posizione del punto che abbiamo scelto nell'inspector(firePoint.position), 
        //e alla stessa rotazione di quel punto (firePoint.rotation)

        //la prima parte(GameObject bulletGO) = (GameObject) permette di ottenere l'oggetto instanziato come variabile temporanea (chiamata bulletGO),
        //il che ci permette di usare i dati di questo gameobject per i comandi successivi (è chiamato ObjectCasting)
        GameObject bulletGO = (GameObject) Instantiate(turretStats.bulletPrefab, firePoint.position, firePoint.rotation);

        //Definisce una variabile temporanea di tipo Bullet_Behaviour (deve essere lo stesso nome dello script del proiettile) e 
        //lo setta prendendolo proprio dal proiettile che abbiamo appena instanziato
        Bullet_Behaviour bullet = bulletGO.GetComponent<Bullet_Behaviour>();    

        if (bullet != null)         //Se nell'oggetto c'è lo script che abbiamo chiesto (Bullet_Behaviour)
        {
            bullet.Seek(_target);   //Chiama il comando Seek al suo interno, consegnandogli il transform del bersaglio
            
            //NOTA:questo non sarà necessario una volta personalizzati i proiettili
            bullet.SetDamage(turretStats.damage);   //setta il danno del proiettile a quello di chi lo spara

        }
    }



    private void OnTriggerEnter(Collider nemico)
    {
        if (nemico.gameObject.tag == "Enemy")                   //se l'oggetto dentro l'area ha la tag "Enemy"...
        {
            if (nemico.isTrigger == false)                      //e non è l'area trigger a toccare...
            {
                
                robots.Add(nemico.gameObject);                  //aggiungi il nemico alla lista robots
                //Debug.Log("numero robot in area: " + robots.Count);
            }

        }
    }

    private void OnTriggerExit(Collider nemico)
    {
        if (nemico.gameObject.tag == "Enemy")                   //se l'oggetto uscito dall'area ha la tag "Enemy"...
        {
            if (nemico.isTrigger == false)                      //e non è l'area trigger a toccare...
            {
                robots.Remove(nemico.gameObject);                  //rimuovi il nemico alla lista robots
                //Debug.Log("numero robot in area: " + robots.Count);
            }

        }
    }

    public void removeHimselfFromTarget()
    {
        gameObject.SetActive(false);        //disabilita questo gameobject

        foreach (GameObject robot in robots)        //per ogni robot nell'area (quindi quelli nella lista)...
        {
            Enemy_Behaviour enemyScript = robot.GetComponent<Enemy_Behaviour>();
            GameObject enemyTarget = enemyScript.shootingTarget;
            if (this.gameObject == enemyTarget)
            {
                Debug.Log("rimosso dal target nemico");
                enemyTarget = null;
                enemyScript.SetDestination();
            }
        }

        robots.Clear();     //rimuovi dalla lista tutti i robot salvati
    }

    public int GetPriceToRebuild()
    {
        int repCost = Mathf.FloorToInt(turretStats.priceToBuy * 0.8f); //il costo di riparazione è l'80% del costo della torretta intera, arrotondato per difetto
        return repCost;                         //ritorna il costo appena calcolato
    }
}
