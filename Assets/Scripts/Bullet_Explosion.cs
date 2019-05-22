using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Explosion : MonoBehaviour
{
    public int damage = 1;     //definisce il danno del l'esplosione

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && other.isTrigger==false)                              //se è un nemico...
        {
            Enemy_HealthBar HealthBarScript = other.GetComponent<Enemy_HealthBar>();   //...prendi lo script della vita del nemico...
            if (HealthBarScript != null)
            {
                HealthBarScript.TakeDamage(damage);                                         //... e chiama il comando per danneggiarlo 
            }
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void SetDamage(int danno)
    {
        damage = danno;
    }

}
