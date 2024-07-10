using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpellScript : MonoBehaviour
{
	#region Spell Fields
	/**  
	 *	SPELLS EXPLANATION
	 *	
	 *		In order to cast a spell, a specific combination of attacks must happen.
	 *	 This combination is able to happen without waiting for the cooldown on weapons,
	 *   so it's not locked by the _attackReady boolean.
	 *   
	 *   In order to implement this, I will be creating a resettable STRING which will 
	 *   consist of characters based on the inputs:
	 *   
	 *   L - Left click (Slash)
	 *   R - Right click (Thrust)
	 *   S - Spacebar (Slam)
	 *   
	 *   In the update method, I will be constantly checking if the string has a preset
	 *   spell input, and if it does, the spell will be cast and the string reset. 
	 */

	/**  
	 *	SPELL FIELDS
	 *	
	 *		The spell fields will consist of preset strings and the changeable 
	 *		string for the current "cast". 
	 */
	private List<string> _spellList = new List<string>
	{
		"LLL",
		"RRR",
		"SSS",
		"LSL",
		"SLS",
		"LLS",
		"SSL",
		"LRR",
		"RRL"
	};
	private Dictionary<string, string> _spellDictionary =
		new Dictionary<string, string>
		{
			{ "LLL", "Fireball"},
			{ "RRR", "Lightning Stream"},
			{ "SSS", "Ice Hail"},
			{ "LSL", "Water Sword"},
			{ "SLS", "Water Piercing Rain"},
			{ "LLS", "Water Blast"},
			{ "SSL", "Snowstorm"},
			{ "LRR", "Fire Arrow" },
			{ "RRL", "Lightning Bolt"}
		};

	private bool _spellBeingInputted;
	private bool _spellFinished;

	private string _currentSpellCast = "";


	//Cooldown on spells 
	[SerializeField] private float _spellInputDuration = 4f;
	[SerializeField] private float _spellInputTimer;
	private bool _isSpellActive => _spellInputTimer > 0f;
	#endregion

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (_isSpellActive)
		{
			_spellInputTimer -= Time.deltaTime;
			if (!_isSpellActive)
			{
				_spellBeingInputted = false;
				_currentSpellCast = "";
				print("Input not finished in time, spell cancelled.");
			}
		}

		//No spell being cast.
		if (!_spellBeingInputted)
		{
			_currentSpellCast = "";
			return;
		}

		if (_spellFinished)
		{
			_spellBeingInputted = false;
			print(_currentSpellCast);
			//Invalid Spell
			if (_spellList.IndexOf(_currentSpellCast) == -1)
			{
				print("No spell for this input " + _currentSpellCast);
			}
			else
			{
				//Spell cast detected.
				print("Casting " + _spellDictionary[_currentSpellCast] +
					" with input " + _currentSpellCast);
				//cast spell
			}
			_spellFinished = false;
			_currentSpellCast = "";
			_spellInputTimer = 0f;
			return;
		}
	}

	#region Spell Methods
	private bool isFirstInput()
	{
		return _currentSpellCast.Length == 0;
	}

	public void OnBeginSpell(InputAction.CallbackContext context)
	{
		if (!context.performed ||
			!_spellBeingInputted ||
			PlayerScript.Player.OnPedestal) 
		{
			return;
		}

		if (isFirstInput() && !_spellBeingInputted)
		{
			print("Beginning spell");
			_currentSpellCast = "";
			_spellBeingInputted = true;
			_spellInputTimer = _spellInputDuration;
			return;
		}
		if (_spellBeingInputted)
		{
			_spellFinished = true;
			return;
		}
	}

	public void ReadSpellInput(InputAction.CallbackContext context)
	{
		if (!context.performed ||
			!_spellBeingInputted ||
			PlayerScript.Player.OnPedestal) 
		{
			return;
		}

		switch (context.action.name)
		{
			case "Slash":
				_currentSpellCast += "L";
				break;
			case "Thrust":
				_currentSpellCast += "R";
				break;
			case "Slam":
				_currentSpellCast += "S";
				break;
			default:
				print("You shouldn't be here!");
				throw new InvalidOperationException();
		}
		if (isFirstInput())
		{
			//start time frame
		}
	}
	#endregion
}
