using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;

    private Vector2 _moveVector;

    [Range(0, 10f)] public float speed;
    /// <summary>
    /// Used for SmoothDamp in Update(), requires a ref.
    /// </summary>
    private Vector3 _smoothDampVector = Vector3.zero;

    /// <summary>
    /// How much to smooth out the movement
    /// </summary>
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;	
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, _moveVector,
            ref _smoothDampVector, _movementSmoothing);
        Debug.Log(_rigidbody.velocity.magnitude);
        _animator.SetFloat("Speed", _rigidbody.velocity.magnitude);
    }

	public void OnMove(InputAction.CallbackContext context)
	{
        _moveVector = context.ReadValue<Vector2>() * speed;
	}

    
}
