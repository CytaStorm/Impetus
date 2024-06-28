using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
/**  PLAYER STATS
 *      The purpose of this class is to create ways for enemies or
 *   other items and upgrades etc to interact with the stats of the player
 */

public class PlayerScript : MonoBehaviour, IDamageable
{
	#region GameObject Components
	[SerializeField] private PlayerMovementScript _playerMovementScript;
	#endregion

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

	#region Damage
	[SerializeField] private float _attackDamage;
	public float AttackDamage
	{
		get => _attackDamage;
		set => _attackDamage = value;
	}

	//Decrement each time new room is entered.
	private int _attackBuffRoomsLeft;
    public int AttackBuffRoomsLeft
	{
		get => _attackBuffRoomsLeft;
		set => _attackBuffRoomsLeft = value;
	}
	#endregion

	#region Currency & Shop
	[Header("Currency & Shop")]
	[SerializeField] private float _gold;
	public float Gold
	{
		get => _gold;
		set => _gold = value;
	}

	private bool _onPedestal = false;
	public bool OnPedestal
	{
		get => _onPedestal;
		set => _onPedestal = value;
	}
	#endregion

	#region Events
	public UnityEvent<float> HealthChanged;
	public UnityEvent<float> MaxHealthChanged;

	public UnityEvent<float> AetherChanged;
	public UnityEvent<float> MaxAetherChanged;

	public UnityEvent<float> PlayerDamageChanged;
	public UnityEvent<PlayerScript, float> BuyItem;

	public UnityEvent PlayerDied;
	public UnityEvent PlayerRevived;
	public UnityEvent PlayerFullHealed;
	#endregion

	#region Functions to change stats

	public void TakeDamage(float amount, float multiplier)
	{
		Health -= (amount * multiplier);
		print(Health);
		if (Health <= 0)
		{
			print("Player has died"); 
			PlayerDied.Invoke();
		}
	}
	public void Die()
	{
		Health = 0;
		PlayerDied.Invoke();
	}
	public void RespawnPlayer()
	{
		print("respawn");
		Health = MaxHealth;
		PlayerRevived.Invoke();
	}

	public void Heal(float amount, float multiplier, bool canHealPastMax)
	{
		print("healed");
		float amountRegained = amount * multiplier;
		if (Health + amountRegained > MaxHealth &&
			!canHealPastMax) 
		{
			amountRegained = MaxHealth - MaxHealth;
		}
		Health += amountRegained;
		HealthChanged.Invoke(amountRegained);
	}

	public void PlayerFullHeal()
	{
		print("full healed");
		Health = MaxHealth;
		PlayerFullHealed.Invoke();
	}

	public void PlayerRegainAether(float amount, float multiplier, bool canRegenPastMax)
	{
		print("regained aether");
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
		_attackDamage = 25;
	}

	// Update is called once per frame
	void Update()
	{
		if (Health <= 0)
		{
			PlayerDied.Invoke();
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

	#region World Interactions
	public void OnBuy(InputAction.CallbackContext context)
	{
		if (!OnPedestal || !context.performed)
		{
			return;
		}
		BuyItem.Invoke(this, Gold);
	}
	#endregion
}
