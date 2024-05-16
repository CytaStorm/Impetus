using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float speedMag = _rigidbody.velocity.magnitude;
        _animator.SetFloat("Speed", speedMag);
    }
}
