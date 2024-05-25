using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
	#region Attack Cooldown Fields
	public float AttackCooldownTime;

    private float _attackCooldownTimer;

    private bool _attackReady 
    {
        get { return _attackCooldownTimer == 0f; }
    }
	#endregion

	#region Sword Drawing
	private GameObject _swordControlPoint;
    private Vector3 _mousePosition;

    /// <summary>
    /// Used to determine which side of the player sword
    /// should be drawn on.
    /// </summary>
    private byte _swingOffset;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region Sword initialization
        _swordControlPoint = this.gameObject.transform.GetChild(0).gameObject;
        _swingOffset = 0;
        #endregion

        #region Spell initialization
        currentSpellCast = "";
		spellList.Add(fireballSpell);
		spellList.Add(lightningStreamSpell);
        spellList.Add(iceHailRainSpell);
        spellList.Add(waterSwordSpell);
        spellList.Add(waterPiercingRainSpell);
        spellList.Add(waterBlastSpell);
        spellList.Add(snowStormSpell);
        spellList.Add(fireArrowSpell);
        spellList.Add(lightningBoltSpell);
        #endregion

    }

	// Update is called once per frame
	void Update()
	{
		#region Sword update
		//Point sword in direction of mouse
		_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Vector2 direction = new Vector2(
			_mousePosition.x - _swordControlPoint.transform.position.x,
			_mousePosition.y - _swordControlPoint.transform.position.y);

		_swordControlPoint.transform.up = direction;

		if (_swingOffset == 0)
		{
			_swordControlPoint.transform.Rotate(
				new Vector3(0, 0, 100));
		}
		else
		{
			_swordControlPoint.transform.Rotate(
				new Vector3(0, 0, -100));
		}

		if (!_attackReady)
		{
			_attackCooldownTimer =
				Mathf.Clamp(_attackCooldownTimer - Time.deltaTime, 0f, AttackCooldownTime);
		}
        #endregion

        #region Spell Update

		/**
		 * Checks if the length of the spell input 
		 * string is 3 letters long and casts a spell
		 */

        if (currentSpellCast.Length == 3) 
		{
			print("Spell input desired length");
			print(currentSpellCast);
			Boolean isSpell = false;
			foreach (string spell in spellList)
			{
				
				if (spell == currentSpellCast)
				{
					if(spellDictionary.TryGetValue(currentSpellCast, out string spellName))
					{
						print(spellName + " Casted!");
                        //cast spell
                        isSpell = true;
                    }
					
			
					

					
				}
			}
			currentSpellCast = "";
			if (!isSpell)
			{
				print("Invalid Spell Input!");
			}
		}

        #endregion
    }



    #region Attack Methods

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

    // Basic spells - L: Fire attribute, R: Lightning attribute, S: Ice attribute.

    public string fireballSpell = "LLL";  
	public string lightningStreamSpell = "RRR"; 
	public string iceHailRainSpell = "SSS";

	//By mixing these attributes, you can create other forms of magic.

	public string waterSwordSpell = "LSL";
	public string waterPiercingRainSpell = "SLS";
	public string waterBlastSpell = "LLS";

	public string snowStormSpell = "SSL";

	public string fireArrowSpell = "LLR";
	public string lightningBoltSpell = "RRL";

	public List<string> spellList = new List<string>();
    public Dictionary<string, string> spellDictionary = new Dictionary<string, string>
    {
        { "LLL", "fireballSpell" },
        { "RRR", "lightningStreamSpell" },
        { "SSS", "iceHailRainSpell" },
        { "LSL", "waterSwordSpell" },
        { "SLS", "waterPiercingRainSpell" },
        { "LLS", "waterBlastSpell" },
        { "SSL", "snowStormSpell" },
        { "LLR", "fireArrowSpell" },
        { "RRL", "lightningBoltSpell" }
    };


    //I'll add more of these later on, for now I'm going to leave it at this and probably not even
    //code most of them until i know we can actually like get more than just a few done.

    public string currentSpellCast;

    public void OnSlash(InputAction.CallbackContext context)
	{
		Boolean wasSpellInput = false;
		if (!context.performed)
		{
			return;
		}
		if (!_attackReady)
		{
            currentSpellCast = currentSpellCast + "L";
			wasSpellInput = true;
            Debug.Log("attack on cooldown!");
			return;
		}
		if (!wasSpellInput)
		{
            currentSpellCast = currentSpellCast + "L";

        }
		

		Debug.Log("slash");
		_attackCooldownTimer = AttackCooldownTime;
		ChangeSwingOffset();
		return;
	}

	

	public void OnThrust(InputAction.CallbackContext context)
	{
		Boolean wasSpellInput = false;
		if (!context.performed)
		{
			return;
		}
		if (!_attackReady)
		{
            currentSpellCast = currentSpellCast + "R";
			wasSpellInput = true;
            Debug.Log("attack on cooldown!");
			return;
		}
		if (!wasSpellInput)
		{
			currentSpellCast = currentSpellCast + "R";
		}

        

        Debug.Log("thrust");
		_attackCooldownTimer = AttackCooldownTime;
		ChangeSwingOffset();
		return;
	}

	public void OnSlam(InputAction.CallbackContext context)
	{
		Boolean wasSpellInput = false;
		if (!context.performed)
		{
			return;
		}
		if (!_attackReady)
		{
            currentSpellCast = currentSpellCast + "S";
			wasSpellInput = true;
            Debug.Log("attack on cooldown!");
			return;
		}
		if (!wasSpellInput)
		{
            currentSpellCast = currentSpellCast + "S";
        }
        

        Debug.Log("slam");
		_attackCooldownTimer = AttackCooldownTime;
		ChangeSwingOffset();
		return;
	}
	#endregion

	private void ChangeSwingOffset()
	{
		if (_swingOffset == 0)
		{
			_swingOffset = 1;
		}
		else
		{
			_swingOffset = 0;
		}
	}

}
