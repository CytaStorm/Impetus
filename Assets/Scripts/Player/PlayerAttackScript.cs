using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
	#region GameObject Components
	[Header("GameObject Components")]
	[SerializeField] private GameObject _swordPivotPoint;
	[SerializeField] private GameObject _sword;
	private Animator _swordAnimator;
	#endregion

	#region Attack Properties
	[Header("Attack Properties")]
	//Attack duration
	[SerializeField] private float _attackDuration = 0.5f;
	private float _currentAttackTime;
	private bool _attacking;
	private float _attackCompletion
	{
		get { return _currentAttackTime / _attackDuration; }
	}
	private Vector2 _mostRecentAttackDirection;

	//Attack Cooldown
	[SerializeField] private float _attackCooldownDuration = 2f;
	private float _currentAttackCooldownTime;
	private bool _attackReady
	{
		get { return _currentAttackCooldownTime == 0f; }
	}
	#endregion

	#region Sword Drawing
	[SerializeField][Range(0f, 360f)] private int _slashArc;
	private Vector3 _slashArcOffset
	{
		get { return new Vector3(0, 0, _slashArc / 2); }
	}
	private Vector3 _slashArcBegin;
	private Vector3 _slashArcEnd;

	#endregion

	// Start is called before the first frame update
	void Start()
	{
		_swordPivotPoint.SetActive(false);
		_swordAnimator = _sword.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		#region Sword update
		//Tick attack cooldown if possible and not attacking.
		if (!_attacking)
		{
			if (!_attackReady)
			{
				_currentAttackCooldownTime =
					Mathf.Clamp(_currentAttackCooldownTime - Time.deltaTime, 0f, _attackCooldownDuration);
			}
		}
		//Is attacking, tick up attack time and check if still attacking
		_currentAttackTime = Mathf.Clamp(_currentAttackTime + Time.deltaTime, 0f,
			_attackDuration);
		_swordPivotPoint.transform.eulerAngles = Vector3.Lerp(
			_slashArcBegin, _slashArcEnd, _attackCompletion);
		//print(_swordPivotPoint.transform.eulerAngles);

		if (_currentAttackTime == _attackDuration)
		{
			_attacking = false;
			_swordPivotPoint.SetActive(false);
		}
		#endregion
	}

	#region Attack Methods
	public void OnSlashInput(InputAction.CallbackContext context)
	{
		if (!context.performed || !_attackReady) return;
		_swordAnimator.SetTrigger("Swing");
		HandleBasicAttack(context);
		return;
	}

	public void OnThrustInput(InputAction.CallbackContext context)
	{	
		if (!context.performed || !_attackReady) return;
		_swordAnimator.SetTrigger("Thrust");
		HandleBasicAttack(context);
		return;
	}

	public void OnSlamInput(InputAction.CallbackContext context)
	{
		if (!context.performed || !_attackReady) return;
		_swordAnimator.SetTrigger("Slam");
		HandleBasicAttack(context);
		return;
	}
	#endregion

	#region Helper Methods

	/// <summary>
	/// Sets up common elements between different basic attacks
	/// like setting attack cooldowns and attack duration.
	/// </summary>
	private void HandleBasicAttack(InputAction.CallbackContext context)
	{
		_swordPivotPoint.SetActive(true);
		//Reset attack cooldown and attack duration
		_currentAttackCooldownTime = _attackCooldownDuration;
		_attacking = true;
		_currentAttackTime = 0f;

		//Lock attack direction to cardinal/diagonal
		_mostRecentAttackDirection = GetAttackDirection();
		_swordPivotPoint.transform.right = GetAttackDirection();
		_slashArcBegin = _swordPivotPoint.transform.eulerAngles +
			_slashArcOffset;
		_slashArcEnd = _swordPivotPoint.transform.eulerAngles -
			_slashArcOffset;
	}

	private Vector2 GetAttackDirection()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 rawAttackDirection = new Vector2(
			mousePosition.x - _swordPivotPoint.transform.position.x,
			mousePosition.y - _swordPivotPoint.transform.position.y);
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
