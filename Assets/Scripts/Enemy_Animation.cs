using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;// per far funzionare le animazioni

public class Enemy_Animation : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();      //prende il primo animator che trova fra i suoi figli
        if (animator == null)
        {
            Debug.LogError($"Nessun animator trovato tra i figli di {this}");
        }
    }

    public void AttackAnimation(bool request)
    {
        animator.SetBool("Is_Attacking", request);          //imposta l'animator in fase di attacco o di idle
    }

    public void DeathAnimation()
    {
        Debug.Log("DeathAnimation chiamato");
        animator.SetBool("Is_Dead", true);                  //fa partire l'animazione di morte
    }

    public bool IsInAtkAnim()
    {
        bool ItIs = animator.GetBool("Is_Attacking");
        //Debug.Log($"Animator controllato, fase di attacco: {ItIs}.");
        return ItIs;
    }

  
}
