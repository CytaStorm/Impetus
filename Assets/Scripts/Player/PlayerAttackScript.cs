using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
	#region GameObject Components
	[SerializeField] private GameObject _sword;
	private Animator _swordAnimator;
	[Space(10)]
	[SerializeField] private AnimationClip _swingAnimationClip;
	[SerializeField] private AnimationClip _thrustAnimationClip;
	[SerializeField] private AnimationClip _slamAnimationClip;
	[Space(20)]
	#endregion

	#region Attack Properties
	//Attack durations
	[Range(0, 2)][SerializeField] private float _swingAttackDuration;
	[Range(0, 2)][SerializeField] private float _thrustAttackDuration;
	[Range(0, 2)][SerializeField] private float _slamAttackDuration;

	//Cooldowns
	[SerializeField] private float _attackCooldownDuration = 2f;
	private bool _swingReady = true;
	private bool _thrustReady = true;
	private bool _slamReady = true;
	#endregion

	// Start is called before the first frame update
	private void Start()
	{
		_swordAnimator = _sword.GetComponent<Animator>();
		SetAttackSpeeds();
	}

	// Update is called once per frame
	void Update()
	{
		//TESTING FOR GOOD ATTACKS.
		SetAttackSpeeds();
	}

	#region Attack Methods
	public void OnSlashInput(InputAction.CallbackContext context)
	{
		if (!context.performed || !_swingReady) return;
		_swordAnimator.SetTrigger("Swing");
		SetAttackDirection();
		StartCoroutine(BeginSwingCooldown());

		//Hitbox

		return;
	}

	public void OnThrustInput(InputAction.CallbackContext context)
	{
		if (!context.performed || !_thrustReady) return;
		_swordAnimator.SetTrigger("Thrust");
		SetAttackDirection();
		StartCoroutine(BeginThrustCooldown());

		//Hitbox
		return;
	}

	public void OnSlamInput(InputAction.CallbackContext context)
	{
		if (!context.performed || !_slamReady) return;
		_swordAnimator.SetTrigger("Slam");
		SetAttackDirection();
		StartCoroutine(BeginSlamCooldown());

		//Hitbox
		return;
	}
	#endregion

	#region Helper Methods
	private void SetAttackSpeeds()
	{
		_swordAnimator.SetFloat(
					"Swing Speed",
					1 / (_swingAttackDuration / _swingAnimationClip.length));
		_swordAnimator.SetFloat(
			"Thrust Speed",
			1 / (_thrustAttackDuration / _thrustAnimationClip.length));
		_swordAnimator.SetFloat(
			"Slam Speed",
			1 / (_slamAttackDuration / _slamAnimationClip.length));
	}

	private IEnumerator BeginSwingCooldown()
	{
		_swingReady = false;
		yield return new WaitForSeconds(_attackCooldownDuration);
		_swingReady = true;
	}

	private IEnumerator BeginThrustCooldown()
	{
		_thrustReady = false;
		yield return new WaitForSeconds(_attackCooldownDuration);
		_thrustReady = true;
	}

	private IEnumerator BeginSlamCooldown()
	{
		_slamReady = false;
		yield return new WaitForSeconds(_attackCooldownDuration);
		_slamReady = true;
	}

	private void SetAttackDirection()
	{
		#region Calculate Attack Direction
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 rawAttackDirection = new Vector2(
			mousePosition.x - _sword.transform.position.x,
			mousePosition.y - _sword.transform.position.y);
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
		#endregion

		//Flip if attack on left to prevent animation rotating.
		if (realAttackDirection == Vector2.left ||
			realAttackDirection == Vector2.left + Vector2.down ||
			realAttackDirection == Vector2.left + Vector2.up)
		{
			if (_sword.transform.localScale.x > 0)
			{
				_sword.transform.localScale = new Vector3(
					_sword.transform.localScale.x * -1,
					_sword.transform.localScale.y,
					_sword.transform.localScale.z);
			}
		}
		else
		{
			_sword.transform.localScale = new Vector3(
				Mathf.Abs(_sword.transform.localScale.x),
				_sword.transform.localScale.y,
				_sword.transform.localScale.z);
		}

		_sword.transform.up = realAttackDirection;
	}
	#endregion
}
