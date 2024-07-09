using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour, IMoveable
{
	#region GameObject Components
	[Header("GameObject Components")]
	[SerializeField] private Animator _animator;
	[SerializeField] private Rigidbody2D _rigidbody;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	#endregion

	#region General
	[Header("General")]
	private bool _moveable;
	public bool Moveable
	{
		get => _moveable;
		set => _moveable = value;
	}

	[Range(0, 10f)] [SerializeField] private float _speed = 5;
	public float Speed
	{
		get => _speed;
		set { _speed = value; }
	}

	private Vector2 _moveVector;

	/// <summary>
	/// How much to smooth out the movement
	/// </summary>
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;

	/// <summary>
	/// Used for SmoothDamp in Update(), requires a ref.
	/// </summary>
	private Vector3 _smoothDampVector = Vector3.zero;
	#endregion

	#region Dashing
	[Header("Dash Settings")]
	[SerializeField] private float _dashDistance;
	[SerializeField] private float _dashCooldownSeconds;
	public float DashDistance
	{
		get => _dashDistance;
		set => _dashDistance = value;
	}
	public float DashCooldownSeconds
	{
		get => _dashCooldownSeconds;
		set => _dashCooldownSeconds = value;	
	}

	private float _dashCooldownTimer;
	private bool _dashReady
	{
		get { return _dashCooldownTimer == 0f; }
	}
	#endregion

	// Start is called before the first frame update
	void Start()
	{
		Moveable = true;
		PlayerScript.Player.PlayerDied.AddListener(CannotMove);
	}

	// Update is called once per frame
	void Update()
	{
		_rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, _moveVector,
			ref _smoothDampVector, _movementSmoothing);

		_animator.SetFloat("Speed", _rigidbody.velocity.magnitude);

		if (!_dashReady)
		{
			_dashCooldownTimer = 
				Mathf.Clamp(_dashCooldownTimer - Time.deltaTime, 0f, DashCooldownSeconds);
		}
	}

	public void OnMoveInput(InputAction.CallbackContext context)
	{
		if (!Moveable)
		{
			return;
		}
		_moveVector = context.ReadValue<Vector2>() * Speed;
	}

	public void OnDashInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		if (!_dashReady)
		{
			Debug.Log("Dash on cooldown!");
			return;
		}

		_dashCooldownTimer = DashCooldownSeconds;

		//Dash towards cursor if player is still.
		if (_moveVector.magnitude == 0)
		{
			Debug.Log("Cursor Dash");
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 dashDirection = new Vector2(
				mousePosition.x - transform.position.x,
				mousePosition.y - transform.position.y).normalized;

			_rigidbody.AddForce(dashDirection * DashDistance);
			return;
		}
		//Dash in direction of movement if player is moving.
		else 
		{
			Debug.Log("Movement Dash");
			_rigidbody.AddForce(_moveVector.normalized * DashDistance);
			return;
		}
	}

	private void CannotMove()
	{
		Moveable = false;
	}

	public void Knockback(Vector2 force)
	{
		_rigidbody.AddForce(force);
	}
}
