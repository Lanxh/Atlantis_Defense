using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  LEGGIMI DANNAZIONE!

1) i nemici devono avere il tag "Enemy"
*/

public class Turret_Behaviour : MonoBehaviour
{

    private Transform _target;          //Il transform del bersaglio, inizialmente nullo.
    public float _range=15f;
    public int shootRate = 10;          // quante volte controlla al secondo, se non sta bersagliando nessuno.
    public float rotationSpeed = 45;    // di quanti gradi al secondo ruota quando guarda l'obbiettivo.
    float timeToShoot;

    void Start()
    {
        timeToShoot = 1 / shootRate;    //imposta il tempo per sparare
        _target = null;                 //per evitare errori, all'inizio imposto il target su null
    }

    void Update()
    {
        if (_target == null)                                                                    //Se non c'è un target...
        {
            transform.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);       //...ruota nella posizione 0 
        }
        else                                                                                    //Se invece c'è un target...
        {
            transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));    //... guarda verso di lui.
        }
    }

    void FixedUpdate()
    {
        timeToShoot -= Time.fixedDeltaTime;         //timer per il tempo prima di sparare
        if (_target != null && timeToShoot <= 0)    //a tempo scaduto...
        {
            timeToShoot = 1 / shootRate;            //... resetta il timer e...
            Shoot();                                //... chiama la funzione per sparare.
        }
    }

    void Shoot()
    {
        RaycastHit hit;                             //Definisce una variabile che prende informazioni dal raycast

        /*Physics.Raycast spara un raggio che parte dalla torre(transform.position), nella direzione "dritta" dell'oggetto (vector3.forward), 
        della stessa lunghezza del raggio d'azione della torre (_range). 
        Infine, le informazioni ottenute (out) vengono conservate nella variabile scelta (hit).*/
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, _range))
        {
            if (hit.transform.tag == "Enemy")   //se l'oggetto colpito ha la tag "Enemy"...
            {
                _target = hit.transform;        //...ottiene il transform del bersaglio e lo conserva in "_target"
                Debug.Log("BANG!");
            }
        }
                     
    }

        private void OnDrawGizmosSelected()     //designa i Gizmo che appaiono quando selezioni l'oggetto (in questo caso il range della torretta)
    {
        Gizmos.color = Color.yellow;            //il colore del gizmo è giallo 
        Gizmos.DrawWireSphere(transform.position, _range);  //disegna una sfera di raggio "_range" con il centro sul transform dell'oggetto (la torretta)
    }

    private void OnTriggerEnter(Collider robot)
    {
        if (robot.gameObject.tag == "Enemy")                   //se l'oggetto dentro l'area è un nemico...
        {
            Debug.Log("nemico entra in area");

        }
    }
}
