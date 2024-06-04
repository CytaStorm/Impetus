using UnityEngine;
using UnityEngine.Events;
/**  PLAYER STATS
 *      The purpose of this class is to create ways for enemies or
 *   other items and upgrades etc to interact with the stats of the player
 */

public class PlayerStats : MonoBehaviour
{
    #region Fields
    [SerializeField] private float _playerHealth; 
    [SerializeField] private float _playerMaxHealth;

    [SerializeField] private float _playerDamage;

    [SerializeField] private float _playerMana;
    [SerializeField] private float _playerMaxMana;

    private float _flow;
    #endregion

    #region Properties
    public float PlayerHealth
    {
        get => _playerHealth;
        set => _playerHealth = value;
    }
    public float PlayerMaxHealth
    {
        get => _playerMaxHealth;
        set 
        {
            PlayerMaxHealthChanged.Invoke(value);
            _playerMaxHealth = value; 
        }
    }
    public bool PlayerAlive
    {
        get => PlayerHealth > 0;
    }
    public float PlayerDamage
    {
        get => _playerDamage;
        set 
        { 
            PlayerDamageChanged.Invoke(value);
            _playerMana = value; 
        }
    }
    public float PlayerMana
    {
        get => _playerMana;
        set 
        {
            PlayerManaChanged.Invoke(value);
            _playerMana = value; 
        }
    }
    public float PlayerMaxMana
    {
        get => _playerMaxMana;
        set 
        {
            PlayerMaxManaChanged.Invoke(value);
            _playerMaxMana = value; 
        }
    }
    public float Flow
    {
        get => _flow;
        set => _flow = value;
    }
    public int FlowState
    {
        get
        {
            if (Flow == 0) return 0;
            if (Flow > 0 && Flow < 25) return 1;
            if (Flow >= 25 && Flow < 50) return 2;
            if (Flow >= 50 && Flow < 75) return 3;
            return 4;
        }
    }
    #endregion

    #region Events
    public UnityEvent<float> PlayerHealthChanged;
    public UnityEvent<float> PlayerMaxHealthChanged;

    public UnityEvent<float> PlayerManaChanged;
    public UnityEvent<float> PlayerMaxManaChanged;

    public UnityEvent<float> PlayerDamageChanged;

    public UnityEvent PlayerDied;
    public UnityEvent PlayerRevived;
    public UnityEvent PlayerFullHealed;
    #endregion

    #region Functions to change stats

    public void playerHurt(float amount, float multiplier)
    {
        PlayerHealth -= (amount * multiplier);
        if (PlayerHealth <= 0)
        {
            print("Player has died"); 
            PlayerDied.Invoke();
        }
    }

    public void playerInstantDeath()
    {
        PlayerHealth = 0;
        PlayerDied.Invoke();
    }
    
    public void respawnPlayer()
    {
        PlayerHealth = PlayerMaxHealth;
        PlayerRevived.Invoke();
    }

    public void playerHeal(float amount, float multiplier, bool canHealPastMax)
    {
        float amountRegained = amount * multiplier;
        if (PlayerHealth + amountRegained > PlayerMaxHealth &&
            !canHealPastMax) 
        {
            amountRegained = PlayerMaxHealth - PlayerMaxHealth;
        }
        PlayerMaxHealth += amountRegained;
        PlayerManaChanged.Invoke(amountRegained);
    }

    public void playerFullHeal()
    {
        PlayerHealth = PlayerMaxHealth;
        PlayerFullHealed.Invoke();
    }

    public void playerRegainMana(float amount, float multiplier, bool canRegenPastMax)
    {
        float amountRegained = amount * multiplier;
        if (PlayerMana + amountRegained > PlayerMaxMana &&
            !canRegenPastMax) 
        {
            amountRegained = PlayerMaxMana - PlayerMaxMana;
        }
        PlayerMaxMana += amountRegained;
        PlayerManaChanged.Invoke(amountRegained);
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        PlayerDamage = 10; // starting damage
        PlayerHealth = 100; // starting health
        PlayerMaxHealth = 100; //starting max health
        PlayerMana = 100; //starting mana
        PlayerMaxMana = 100; //starting max mana
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
