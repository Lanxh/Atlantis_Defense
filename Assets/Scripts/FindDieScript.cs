using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindDieScript : MonoBehaviour
{
    Enemy_HealthBar animscript;
    Enemy_Behaviour shootingScript;
    public bool falling = false;
    public float fallSpeed = 0.05f;
    
    void Start()
    {
        animscript = GetComponentInParent<Enemy_HealthBar>();// trova il component EnemyHealthBar nell'oggetto padre
        shootingScript = GetComponentInParent<Enemy_Behaviour>();// trova il component EnemyHealthBar nell'oggetto padre

    }

    private void Update()
    {
        if (falling)
        {
            transform.position += Vector3.down * fallSpeed;
        }
    }

    public void CallDestroy()
    {
        //Debug.Log("callDestroy chiamato");
        animscript.DestroyObject();
    }

    public void StartFalling()
    {
        //Debug.Log("StartFalling chiamato");

        falling = true;
    }

    public void TriggerShot()
    {
        //Debug.Log("triggerShot chiamato");

        shootingScript.Shoot();
    }
}
