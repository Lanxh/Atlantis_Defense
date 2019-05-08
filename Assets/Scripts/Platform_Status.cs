using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Status : MonoBehaviour
{
    public GameObject turretOnTop;  //cosa c'è costruito sopra
    public GameObject turretUpgraded;//con quale torretta upgradarlo
    public GameObject turretDestroyed;//se è una rovina, da quale torre proviene

    public void changePlatformStatus(GameObject turOnTop)
    {       
        turretOnTop = turOnTop;
        if (turOnTop.tag != ("Rubble"))   //se non è stata creata una rovina (quindi non è stata distrutta)...
        {
            if (turretOnTop.GetComponent<Turret_LookAtRobot>().turretStats.upgradedVersion != null)  //... controlla se ha una versione potenziata
            {
                turretUpgraded = turretOnTop.GetComponent<Turret_LookAtRobot>().turretStats.upgradedVersion;    //se la ha, aggiorna la variabile
            }
            else                                    //se non la ha...
            {
                turretUpgraded = null;              //svuota la variabile della torretta potenziata
            }
        }
    }

    public void StoreDestrTurret(GameObject t_destr)
    {
        turretDestroyed = t_destr;
        Debug.Log($"{turretDestroyed}distrutta e salvata in {this.gameObject}");
    }

    public void Clear()
    {
        turretOnTop = null;
        turretUpgraded = null;
        turretDestroyed = null;
    }

}
