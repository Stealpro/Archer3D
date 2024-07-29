using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour
{
    public int HP = 100;
    public Animator animator;

    public AudioClip bearBreath;
    public AudioSource audioSource;

    public GameObject m_RightClaw;

    private void Start()
    {
        audioSource.clip = bearBreath;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void AttackSound()
    {
        AudioManager.instance.Play("BearAttack");
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if(HP <= 0)
        {
            AudioManager.instance.Play("BearDeath");
            animator.SetTrigger("die");
            audioSource.Stop();
            GetComponent<Collider>().enabled = false;
            deactivateClaw();
        }
        else
        {
            AudioManager.instance.Play("BearDamage");
            animator.SetTrigger("damage");
        }
    }
    
    public void activateClaw()
    {
        m_RightClaw.GetComponent<Collider>().enabled = true;
    }
    public void deactivateClaw()
    {
        m_RightClaw.GetComponent<Collider>().enabled = false;
    }
}
