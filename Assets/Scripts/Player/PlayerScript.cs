using System.Collections.Generic;
using System.Security.Cryptography;
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
	#region Singleton
	public static PlayerScript Player 
	{
		get; private set; 
	}
	#endregion

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
		set => _maxHealth = value; 
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
		set => _aether = value; 
	}
	public float MaxAether
	{
		get => _maxAether;
		set => _maxAether = value; 
	}
	#endregion

	#region Flow
	[Header("Flow")]
	public float Flow;
	public float MaxFlow;
	public float FlowPercent
	{
		get => 100 * Flow / MaxFlow;
	}
	[Range(1, 5)] public int FlowState;
	
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

	public UnityEvent<PlayerScript, float> BuyItem;
	public UnityEvent<int, int> UpgradeFlowState;
	public UnityEvent<int, int> DegradeFlowState;

	public UnityEvent PlayerDied;
	#endregion

	#region Functions to change stats
	//Health
	public void Heal(float amount, float multiplier, bool canHealPastMax)
	{
		float amountRegained = amount * multiplier;
		if (Health + amountRegained > MaxHealth &&
			!canHealPastMax) 
		{
			amountRegained = MaxHealth - MaxHealth;
		}
		Health += amountRegained;
	}
	public void FullHeal()
	{
		Health = MaxHealth;
	}
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
		FullHeal();
	}

	//Aether
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
	}
	#endregion

	private void Awake()
    {
        if (Player != null &&
			Player != this)
        {
            Destroy(gameObject);
        }
        else Player = this;
    }

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (Health <= 0)
		{
			PlayerDied.Invoke();
		}

		//Decrease flow state over time.
		Flow -= MaxFlow / _flowStateMaxDropTimesSeconds[FlowState] *
			Time.deltaTime;

		//Upgrade flow if necessary.
		if (Flow > 99)
		{
			FlowState++;
			UpgradeFlowState.Invoke(FlowState - 1, FlowState);
			Flow -= 100;
		}

		//Drop flowstate if necessary
		if (Flow <= 0 && FlowState != 1)
		{
			FlowState--;
			DegradeFlowState.Invoke(FlowState + 1, FlowState);
			Flow = 100;
		}

		//Keep flowstate from going over the max in level or value.
		FlowState = Mathf.Clamp(FlowState, 1, 5);
		Flow = Mathf.Clamp(Flow, 0, MaxFlow);
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
