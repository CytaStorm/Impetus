using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/**  PLAYER STATS
 *      The purpose of this class is to create ways for enemies or
 *   other items and upgrades etc to interact with the stats of the player
 */

public class PlayerStats : MonoBehaviour
{
	#region Health
	[Header("Health")]
	[SerializeField] private float _health; 
	[SerializeField] private float _maxHealth;
	public float Health
	{
		get => _health;
		set => _health = value;
	}
	public float MaxHealth
	{
		get => _maxHealth;
		set 
		{
			MaxHealthChanged.Invoke(value);
			_maxHealth = value; 
		}
	}
	public bool Alive
	{
		get => Health > 0;
	}
	#endregion

	#region Aether
	[Header("Aether")]
	[SerializeField] private float _aether;
	[SerializeField] private float _maxAether;
	public float Aether
	{
		get => _aether;
		set 
		{
			AetherChanged.Invoke(value);
			_aether = value; 
		}
	}
	public float MaxAether
	{
		get => _maxAether;
		set 
		{
			MaxAetherChanged.Invoke(value);
			_maxAether = value; 
		}
	}
	#endregion

	#region Flow
	[Header("Flow")]
	[SerializeField] private float _flow;
	[SerializeField] private float _maxFlow;
	[SerializeField] [Range(0, 5)] private int _flowState;
	public float Flow
	{
		get => _flow;
		set => _flow = value;
	}
	public float MaxFlow
	{
		get => _maxFlow;
		set => _maxFlow = value;
	}
	public int FlowState
	{
		get => _flowState;
		set => _flowState = value;
	}
	/// <summary>
	/// The index is the level you are dropping from, the element at that index
	/// is how much time it takes to drop from a full flow bar.
	/// </summary>
	[SerializeField] private List<float> _flowStateMaxDropTimesSeconds =
		new List<float> {15, 10, 8, 6, 4, 3};

	#endregion

	#region Events
	public UnityEvent<float> HealthChanged;
	public UnityEvent<float> MaxHealthChanged;

	public UnityEvent<float> AetherChanged;
	public UnityEvent<float> MaxAetherChanged;

	public UnityEvent<float> PlayerDamageChanged;

	public UnityEvent PlayerDied;
	public UnityEvent PlayerRevived;
	public UnityEvent PlayerFullHealed;
	#endregion

	#region Functions to change stats

	public void playerHurt(float amount, float multiplier)
	{
		Health -= (amount * multiplier);
		if (Health <= 0)
		{
			print("Player has died"); 
			PlayerDied.Invoke();
		}
	}

	public void playerInstantDeath()
	{
		Health = 0;
		PlayerDied.Invoke();
	}
	
	public void respawnPlayer()
	{
		Health = MaxHealth;
		PlayerRevived.Invoke();
	}

	public void playerHeal(float amount, float multiplier, bool canHealPastMax)
	{
		float amountRegained = amount * multiplier;
		if (Health + amountRegained > MaxHealth &&
			!canHealPastMax) 
		{
			amountRegained = MaxHealth - MaxHealth;
		}
		MaxHealth += amountRegained;
		AetherChanged.Invoke(amountRegained);
	}

	public void playerFullHeal()
	{
		Health = MaxHealth;
		PlayerFullHealed.Invoke();
	}

	public void playerRegainAether(float amount, float multiplier, bool canRegenPastMax)
	{
		float amountRegained = amount * multiplier;
		if (Aether + amountRegained > MaxAether &&
			!canRegenPastMax) 
		{
			amountRegained = MaxAether - MaxAether;
		}
		MaxAether += amountRegained;
		AetherChanged.Invoke(amountRegained);
	}
	#endregion


	// Start is called before the first frame update
	void Start()
	{
		//Uncomment once finished testing flowstate.
		//_flowState = 0;
	}

	// Update is called once per frame
	void Update()
	{
        //Increase flow state if needed.
        if (Flow > 100)
        {
            if (FlowState >= MaxFlow)
            {
                Flow = 99;
            }
            else
            {
                _flowState++;
                Flow -= 100;
            }

        }


        //Decrease flow state
        if (Flow != 0)
		{
			if (FlowState > 5)
			{
				print(MaxFlow);
				FlowState--;
				Flow = 99;
			}
			Flow -= MaxFlow / _flowStateMaxDropTimesSeconds[_flowState] * Time.deltaTime;
		}
		if (Flow <= 0 && FlowState != 0)
		{
			_flowState--;
			Flow = 100;
		}
		

		
	}
}
