using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//per far funzionare NavMeshAgent

/*  LEGGIMI DANNAZIONE!
-Sistema di navigazione in via di sviluppo: possibili aggiornamenti in arrivo-
1) fare il baking della NavMesh andando su Windows => AI => Navigation => Bake (andrà fatto ogni volta che si modificherà la mappa)
2) inserire questo script come componente dell'oggetto da muovere.
3) alla casella _targetName scrivere il nome esatto dell'obbiettivo da raggiungere. Finché la destinazione tocca la NavMesh, l'oggetto si muoverà 
verso di lui evitando automaticamente gli ostacoli.

---PER CREARE OSTACOLI LEGGIBILI DALLA NAVMESH---
4) Accedere alla finestra Navigation andando su Windows => AI => Navigation => Object
5) Selezionare l'oggetto che si vuole usare come ostacolo (deve essere static)
6) con l'oggetto selezionato, andare sulla finestra Navigation => Object e settare  Navigation Static su True
7) se non si vuole che il giocatore possa camminare sopra l'oggetto, settare anche la Navigation Area su Not Walkable
8) fare il baking aggiornato della NavMesh (vedi punto 1)
*/


public class Enemy_Behaviour : MonoBehaviour
{
    [Header("Statistiche del Nemico")]
    public Enemy_Stats enemyStats;                      //serve per recuperare le stat

    [Header("Variabili di controllo")]
    [SerializeField]    
    Transform _destination;                             //dichiara la destinazione
    public NavMeshAgent _navMeshAgent;                  //dichiara la navMesh dell'Enemy
    public GameObject _target;                          //dichiara l'oggetto che fungerà da destinazione
    public string _targetName= "Base_Prova";            //dichiara il nome dell'oggetto che fungerà da destinazione (il nome della base nell'Inspector)

    private Vector3 _targetForFlyers;                   //serve per trovare il punto verso cui si muovono i nemici volanti (che è più in alto)
    private int _flyersHeight = 55;                    //dichiara l'altezza su cui stanno tutti i nemici volanti

    public float _rotationSpeed = 1f;                   //velocità con cui si gira verso la torre
    public float _enemySpeed = 15f;                     //velocità di movimento
    private float fireCountdown = 0f;                   //variabile necessaria per il timer tra un colpo e l'altro

    public enum Behaviour { Storming, Sieging };        //un enumerator permette di usare parole come se fossero Int --- In questo caso Storming=0, Sieging=1.
    public Behaviour _myBehaviour = Behaviour.Storming; //dichiara un comportamento default per non causare errori
    public enum TypeOfRobot { Assault, Siege, Flyer };  //Questo Enumerator suddivide i tipi di robot.
    public TypeOfRobot _myTypeOfRobot;                  //Questa variabile, da definire nell'inspector, serve per far capire allo script che tipo di robot è e come si comporta


    private Vector3 targetPos;                          //indica la posizione della torre/base dove verso cui dovrà ruotare.
    public GameObject shootingTarget;                  //indica il Gameobject verso cui dovrà sparare.

    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();//la navMesh dell'Enemy è quella contenuta in QUESTO(this) Gameobject

        //trovare la direzione
        _target = GameObject.Find(_targetName); //trova il target in base al nome che gli hai dato nell'Inspector
        if (_target == null)                    //se non trova niente...
        {
            Debug.LogWarning("l'oggetto "+ _targetName +" non è stato trovato in- " + gameObject.name +" -Controlla se hai scritto bene il _targetName.");  //...manda un avviso.
        }
        _destination = _target.transform;       //setta la destinazione sull'oggetto chiamato su _targetName


        //Comportamento 
        _myBehaviour = Behaviour.Storming;       //imposta il Behaviour di default (così non dà errore)

        if (_myTypeOfRobot == TypeOfRobot.Flyer)        //se è un nemico volante...
        {
            Debug.Log("Vola!");
            _navMeshAgent.enabled = false;
            transform.position= new Vector3(transform.position.x, _flyersHeight, transform.position.z);
            _targetForFlyers = new Vector3(_destination.position.x, _flyersHeight, _destination.position.z);
        }


        if (_myTypeOfRobot != TypeOfRobot.Flyer)        //se non è un nemico volante...
        {
            Debug.Log("Non è volante");

            if (_navMeshAgent == null)              //controlla se ha un navMeshAgent e se non trova niente...
            {
                Debug.LogWarning("il component NavMesh Agent non è stato trovato in" + gameObject.name);  //...manda un avviso.
            }
            else
            {
                SetDestination();                   //altrimenti chiama il metodo SetDestination (definito sotto)
            }
        }


        _enemySpeed = enemyStats.speed;         //imposta la velocità del robot a quella definita nell' inspector
    }

    private void Update()
    {
        switch (_myBehaviour)                    //per ogni frame, a seconda del behaviour attuale, attiva il metodo corrispondente
        {
            case Behaviour.Storming:            //nel caso il behaviour è Swarming...
                SwarmingState();                //...attiva il metodo Swarming (vai verso la base)
                break;
            case Behaviour.Sieging:             //nel caso il behaviour è Sieging...
                SiegingState();                 //...attiva il metodo Sieging (fermati e spara alla torre)
                break;
            default:                            //nel caso nessun behaviour è impostato...
                Debug.Log("Nessun Behaviour");  //...non fare nulla e scrivi un avviso.
                break;
        }
    }


    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Tower_Pos" && col.isTrigger==false && _myTypeOfRobot== TypeOfRobot.Siege && col.gameObject.activeInHierarchy==true)//se l'oggetto dentro l'area è una torre (e non il suo trigger) ed è un nemico da assedio...
        {
            //Debug.Log("torretta trovata");
            targetPos = new Vector3(col.transform.position.x, col.transform.position.y, col.transform.position.z); //definisce la posizione della torre
            shootingTarget = col.gameObject;        //estrae il gameobject del bersaglio
            _myBehaviour = Behaviour.Sieging;     //imposta il comporatmento su "assedio"

        }
        if (col.gameObject.tag == "Base")//se l'oggetto dentro l'area è una torre (e non il suo trigger) ed è un nemico da assedio...
        {
            //Debug.Log("Base trovata");
            targetPos = new Vector3(col.transform.position.x, col.transform.position.y, col.transform.position.z); //definisce la posizione della torre
            shootingTarget = col.gameObject;        //estrae il gameobject del bersaglio
            _myBehaviour = Behaviour.Sieging;     //imposta il comporatmento su "assedio"

        }
    }

    private void OnTriggerExit(Collider target) //se l'oggetto esce dall'area, ritorna a muoverti verso la base
    {
        if (target.gameObject.tag == "Tower_Pos" && target.isTrigger == false && _myTypeOfRobot != TypeOfRobot.Flyer)
        {
            _myBehaviour = Behaviour.Storming;
            _navMeshAgent.speed = _enemySpeed;
        }
    }

    public void SwarmingState()                 //metodo che definisce come si comporta in stato d'assalto
    {
        if (_myTypeOfRobot == TypeOfRobot.Flyer)        //se è un nemico volante...
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetForFlyers, _enemySpeed/50);
        }
    }

    public void SiegingState()                  //metodo che definisce come si comporta in stato d'assedio
    {
        //Debug.Log("I'm sieging!");

        if (shootingTarget != null && shootingTarget.activeInHierarchy==true)
        {
            Vector3 targetDir = targetPos - transform.position;     //definisce la direzione del target

            float step = _rotationSpeed * Time.deltaTime;           //imposta lunghezza degli step per la rotazione

            /*definisce la direzione della rotazione, cioè partire dalla
            direzione frontale (trasform.forward) e andare verso il target (targetDir)*/
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

            //shooting
            if (fireCountdown <= 0f)                //se il timer scende a 0...
            {
                Shoot();                            //...attiva il comando "Spara"...
                fireCountdown = 1f / enemyStats.fireRate; //... e resetta il timer
            }

            fireCountdown -= Time.deltaTime;        //il timer cala con il tempo


            transform.rotation = Quaternion.LookRotation(newDir);   //ruota di uno step nella direzione della torre
            if (_myTypeOfRobot != TypeOfRobot.Flyer)                //se non è un nemico volante...
            {
                _navMeshAgent.speed = 0; // ferma il nemico
            }
        }
        else                            //Se invece non trova più il target(torretta distrutta)
        {
            _myBehaviour = Behaviour.Storming;  //ricomincia a muoverti
        }

    }

    public void SetDestination()               //metodo che definisce all'IA la destinazione da raggiungere (che sarebbe il transform di _destination)
    {
        if (_destination != null)
        {
            Debug.Log("destinazione settata");
            Vector3 targetVector = _destination.transform.position; //estrae una coordinata dal transform di _destination (da definire nell'inspector)
            _navMeshAgent.SetDestination(targetVector);             //setta la destinazione dell'IA su quella coordinata
            _navMeshAgent.speed = enemyStats.speed;         //imposta la velocità del robot a quella definita nell' inspector
        }
    }

    void Shoot()                                //comando che instanzia il proiettile
    {
        //istanzia un proiettile(bulletPrefab), alla posizione del punto che abbiamo scelto nell'inspector(firePoint.position), 
        //e alla stessa rotazione di quel punto (firePoint.rotation)

        //la prima parte(GameObject bulletGO) = (GameObject) permette di ottenere l'oggetto instanziato come variabile temporanea (chiamata bulletGO),
        //il che ci permette di usare i dati di questo gameobject per i comandi successivi (è chiamato ObjectCasting)
        GameObject bulletGO = (GameObject)Instantiate(enemyStats.bulletPrefab, enemyStats.firePoint.position, enemyStats.firePoint.rotation);

        //Definisce una variabile temporanea di tipo Bullet_Behaviour (deve essere lo stesso nome dello script del proiettile) e 
        //lo setta prendendolo proprio dal proiettile che abbiamo appena instanziato
        Bullet_Behaviour bullet = bulletGO.GetComponent<Bullet_Behaviour>();

        if (bullet != null && shootingTarget!=null)         //Se nell'oggetto c'è lo script che abbiamo chiesto (Bullet_Behaviour)
        {
            bullet.Seek(shootingTarget.transform); //Chiama il comando Seek al suo interno, consegnandogli il transform del bersaglio
            bullet.SetDamage(enemyStats.damage);   //setta il danno del proiettile a quello di chi lo spara
        }
    }

}
