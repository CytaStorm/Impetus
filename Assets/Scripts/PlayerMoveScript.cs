using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
	#region GameObject Components
    [Header("Components")]
	[SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    #endregion


    #region Walking
    [Header("Walking Speed Settings")]
    [Range(0, 10f)] public float Speed = 5;
    /// <summary>
    /// How much to smooth out the movement
    /// </summary>
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;
	private Vector2 _moveVector;
    /// <summary>
    /// Used for SmoothDamp in Update(), requires a ref.
    /// </summary>
    private Vector3 _smoothDampVector = Vector3.zero;
    #endregion

	#region Dashing
    [Header("Dash Settings")]
    public float DashDistance = 1000;
    public float DashCooldownTime = 1;
    private float _dashCooldownTimer;
    private bool _dashReady
    {
        get { return _dashCooldownTimer == 0f; }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {

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
                Mathf.Clamp(_dashCooldownTimer - Time.deltaTime, 0f, DashCooldownTime);
        }
    }

	public void OnMoveInput(InputAction.CallbackContext context)
	{
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

        _dashCooldownTimer = DashCooldownTime;

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
            _rigidbody.AddForce(_moveVector.normalized *  DashDistance);
            return;
        }
    }

    
}
