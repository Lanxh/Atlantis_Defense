using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//per far funzionare NavMeshAgent

/*  LEGGIMI DANNAZIONE!
-Sistema di navigazione in via di sviluppo: possibili aggiornamenti in arrivo-
1) fare il baking della NavMesh andando su Windows => AI => Navigation => Bake (andrà fatto ogni volta che si modificherà la mappa)
2) inserire questo script come componente dell'oggetto da muovere.
3) alla casella Destination inserire l'obbiettivo da raggiungere. Finché la destinazione tocca la NavMesh, l'oggetto si muoverà 
verso di lui evitando automaticamente gli ostacoli.

---PER CREARE OSTACOLI LEGGIBILI DALLA NAVMESH---
4) Accedere alla finestra Navigation andando su Windows => AI => Navigation => Object
5) Selezionare l'oggetto che si vuole usare come ostacolo (deve essere static)
6) con l'oggetto selezionato, andare sulla finestra Navigation => Object e settare  Navigation Static su True
7) se non si vuole che il giocatore possa camminare sopra l'oggetto, settare anche la Navigation Area su Not Walkable
8) fare il baking aggiornato della NavMesh (vedi punto 1)
*/


public class Enemy_Movement : MonoBehaviour
{


    [SerializeField]
    Transform _destination=null;                     //dichiara la destinazione

    NavMeshAgent _navMeshAgent;                 //dichiara la navMesh dell'Enemy

    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();// la navMesh dell'Enemy è quella contenuta in QUESTO(this) Gameobject

        if (_navMeshAgent == null)              //se non trova niente...
        {
            Debug.LogError("il component NavMesh Agent non è stato trovato in" + gameObject.name);  //...manda un avviso.
        }
        else
        {
            SetDestination();                   //altrimenti chiama il metodo SetDestination (definito sotto)
        }

    }

    private void Update()
    {
        if (_navMeshAgent == null)              //se non trova niente...
        {
            Debug.LogError("il component NavMesh Agent non è stato trovato in" + gameObject.name);  //...manda un avviso.
        }
        else
        {
            SetDestination();                   //altrimenti chiama il metodo SetDestination (definito sotto)
        }
    }

    private void SetDestination()               //metodo che definisce all'IA la destinazione da raggiungere (che sarebbe il transform di _destination)
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position; //estrae una coordinata dal transform di _destination (da definire nell'inspector)
            _navMeshAgent.SetDestination(targetVector);             //setta la destinazione dell'IA su quella coordinata
        }
    }
}
