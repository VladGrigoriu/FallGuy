using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    public int currentHealth;

    public HealthBar healthBar;

    public GameObject gameOver;

    public Animator animator;

    public PlayerController playerController;

    private ChromaticAberrationEffect chromaticEffect;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        animator.SetBool("IsDead", false);
        playerController.movementSpeed = 5;
        chromaticEffect = GameObject.FindGameObjectWithTag("PostProcessing").GetComponent<ChromaticAberrationEffect>();
        
    }

    private void Update()
    {
        if(currentHealth <= (maxHealth/5))
        {
            chromaticEffect.SetLowHealthEffect(true);
        }
        else
        {
            chromaticEffect.SetLowHealthEffect(false);
        }
        
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);
        

        if(currentHealth <= 0)
        {
            //DEAD
            //Play dead animation 
            //Game over screen
            animator.SetBool("IsDead", true);
            playerController.movementSpeed = 0;
        }
        else
        {
            animator.SetBool("IsDamaged", true);
        }
    }

    public void EndDamageAnimation()
    {
        animator.SetBool("IsDamaged", false);
    }

    public void PlayerDead()
    {
        Time.timeScale = 0;
        gameOver.SetActive(true);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        healthBar.SetHealth(currentHealth);

        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
