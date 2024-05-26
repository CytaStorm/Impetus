using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/**  PLAYER STATS
 *      The purpose of this class is to create ways for enemies or
 *   other items and upgrades etc to interact with the stats of the player
 */

public class PlayerStats : MonoBehaviour
{
    public float playerHealth; 
    public float playerMaxHealth;

    public float playerDamage;

    public float playerMana;
    public float playerMaxMana;

    public event Action<float> playerHealthChanged;
    public event Action<float> playerMaxHealthChanged;

    public event Action<float> playerManaChanged;
    public event Action<float> playerMaxManaChanged;

    public event Action<float> playerDamageChanged;

    public UnityEvent playerDied;
    public UnityEvent playerRevived;
    public UnityEvent playerFullHealed;


    #region Getters
    public float getPlayerHealth()
    {
        return this.playerHealth;
    }
    public float getPlayerMaxHealth()
    {
        return this.playerMaxHealth;
    }
    public float getPlayerMana()
    {
        return this.playerMana;
    }
    public float getPlayerMaxMana()
    {
        return this.playerMaxMana;
    }
    public float getPlayerDamage()
    {
        return this.playerDamage;
    }
    public Boolean isPlayerAlive()
    {
        if (playerHealth <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion

    #region Setters

    public void setPlayerHealth(float value)
    {
        this.playerHealth = value;
        playerHealthChanged.Invoke(playerHealth);
    }
    public void setPlayerMaxHealth(float value)
    {
        this.playerMaxHealth = value;
        playerMaxHealthChanged.Invoke(playerMaxHealth);
    }
    public void setPlayerDamage(float value)
    {
        this.playerDamage = value; 
        playerDamageChanged.Invoke(playerDamage);
    }
    public void setPlayerMana(float value)
    {
        this.playerMana = value;
        playerManaChanged.Invoke(playerMana);
    }
    public void setPlayerMaxMana(float value)
    {
        playerMaxMana = value;
        playerMaxManaChanged.Invoke(playerMaxMana);
    }
    #endregion

    #region Functions to change stats

    public void playerHurt(float amount,float multiplier)
    {
        playerHealth -= (amount * multiplier);
        if(playerHealth < 0)
        {
            print("Player has died"); 
            playerDied.Invoke();
        }
    }

    public void playerInstantDeath()
    {
        playerHealth = 0;
        playerDied.Invoke();
    }
    
    public void respawnPlayer()
    {
        playerHealth = playerMaxHealth;
        playerRevived.Invoke();
    }

    public void playerHeal(float amount, float multiplier, Boolean canHealPastMax)
    {
        playerHealth += (amount * multiplier);
        if (playerHealth > playerMaxHealth)
        {
            if (!canHealPastMax)
            {
                playerHealth = playerMaxHealth;
            }
        }
        playerHealthChanged.Invoke(this.playerHealth);
        
    }

    public void playerFullHeal()
    {
        playerHealth = playerMaxHealth;
        playerFullHealed.Invoke();
    }

    public void playerRegainMana(float amount, float multiplier, Boolean canRegenPastMax)
    {
        playerMana += (amount * multiplier);
        if( playerMana > playerMaxMana)
        {
            if (!canRegenPastMax)
            {
                playerMana = playerMaxMana;
            }
        }
        playerManaChanged.Invoke(this.playerMana); 
    }

    public void playerIncreaseDamage(float amount)
    {
        playerDamage += amount;
        playerDamageChanged.Invoke(this.playerDamage);
    }
    public void playerDecreaseDamage(float amount)
    {
        playerDamage -= amount;
        playerDamageChanged.Invoke(this.playerDamage);
    }
   
    public void playerIncreaseMaxHealth(float amount)
    {
        playerMaxHealth += amount;
        playerMaxHealthChanged.Invoke(this.playerMaxHealth);
    }
    public void playerDecreaseMaxHealth(float amount)
    {
        playerMaxHealth -= amount;
        playerMaxHealthChanged.Invoke(this.playerMaxHealth);
    }
    
    public void playerIncreaseMaxMana(float amount)
    {
        playerMaxMana += amount;
        playerMaxManaChanged.Invoke(this.playerMaxMana);
    }

    public void playerDecreaseMaxMana(float amount)
    {
        playerMaxMana -= amount;
        playerMaxManaChanged.Invoke(this.playerMaxMana);
    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        this.playerDamage = 10; // starting damage
        this.playerHealth = 100; // starting health
        this.playerMaxHealth = 100; //starting max health
        this.playerMana = 100; //starting mana
        this.playerMaxMana = 100; //starting max mana
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
