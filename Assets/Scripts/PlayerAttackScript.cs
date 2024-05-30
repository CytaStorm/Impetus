using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
	#region Attack Fields
    [Header("Attack Properties")]
	//Attack duration
	public float AttackDuration = 0.5f;
	private float _currentAttackTime;
	private bool _attacking;
	private float _attackCompletion
	{
		get { return _currentAttackTime / AttackDuration; }
	}
	private Vector2 _mostRecentAttackDirection;

	//Attack Cooldown
	public float AttackCooldownDuration = 2f;
    private float _currentAttackCooldownTime;
    private bool _attackReady 
    {
        get { return _currentAttackCooldownTime == 0f; }
    }
	#endregion

	#region Sword Drawing
	[Range(0f, 360f)] public int SlashArc;
	private GameObject _swordControlPoint;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _swordControlPoint = this.gameObject.transform.GetChild(0).gameObject;
		_swordControlPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
	{
		//Tick attack cooldown if possible and not attacking.
		if (!_attacking)
		{
			if (!_attackReady)
			{
				_currentAttackCooldownTime =
					Mathf.Clamp(_currentAttackCooldownTime - Time.deltaTime, 0f, AttackCooldownDuration);
			}
			return;
		}

		//Is attacking, tick up attack time and check if still attacking
		_currentAttackTime = Mathf.Clamp(_currentAttackTime + Time.deltaTime, 0f,
			AttackDuration);


		if (_currentAttackTime == AttackDuration)
		{
			_attacking = false;
			_swordControlPoint.SetActive(false);
		}
	}

	

	#region Attack Methods
	public void OnSlashInput(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		HandleBasicAttack();
		return;
	}

	public void OnThrustInput(InputAction.CallbackContext context)
	{
		if (!context.performed)	return;
		HandleBasicAttack();
		return;
	}

	public void OnSlamInput(InputAction.CallbackContext context)
	{
		if (!context.performed)	return;
		HandleBasicAttack();
		return;
	}
	#endregion

	#region Helper Methods
	/// <summary>
	/// Sets up common elements between different basic attacks
	/// like setting attack cooldowns and attack duration.
	/// </summary>
	private void HandleBasicAttack()
	{
		if (!_attackReady)
		{
			Debug.Log("attack on cooldown!");
			return;
		}
		_swordControlPoint.SetActive(true);		
		//Reset attack cooldown and attack duration
		_currentAttackCooldownTime = AttackCooldownDuration;
		_attacking = true;
		_currentAttackTime = 0f;

		//Lock attack direction to cardinal/diagonal
		_mostRecentAttackDirection = GetAttackDirection();
		_swordControlPoint.transform.up = _mostRecentAttackDirection;
		_swordControlPoint.transform.Rotate(
			new Vector3(0, 0, SlashArc / 2));
		print(_mostRecentAttackDirection);
	}

	private Vector2 GetAttackDirection()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 rawAttackDirection = new Vector2(
			mousePosition.x - _swordControlPoint.transform.position.x,
			mousePosition.y - _swordControlPoint.transform.position.y);
		float rawAngle = Vector2.SignedAngle(Vector2.right, rawAttackDirection);
		Vector2 realAttackDirection;
		//EAST
		if (Mathf.Abs(rawAngle) < 22.5)
		{
			realAttackDirection = Vector2.right;
		}
		//NORTHEAST
		else if (rawAngle >= 22.5 && rawAngle < 67.5)
		{
			realAttackDirection = Vector2.right + Vector2.up;
		}
		//NORTH
		else if (rawAngle >= 67.5 && rawAngle < 112.5)
		{
			realAttackDirection = Vector2.up;
		}
		//NORTHWEST
		else if (rawAngle >= 112.5 && rawAngle < 157.5)
		{
			realAttackDirection = Vector2.up + Vector2.left;
		}
		//WEST
		else if (Mathf.Abs(rawAngle) > 157.5)
		{
			realAttackDirection = Vector2.left;
		}
		//SOUTHWEST
		else if (rawAngle >= -157.5 && rawAngle < -112.5)
		{
			realAttackDirection = Vector2.down + Vector2.left;
		}
		//SOUTH
		else if (rawAngle >= -112.5 && rawAngle < -67.5)
		{
			realAttackDirection = Vector2.down;
		}
		//SOUTHEAST
		else
		{
			realAttackDirection = Vector2.down + Vector2.right;
		}
		return realAttackDirection;
	}
	#endregion
}
