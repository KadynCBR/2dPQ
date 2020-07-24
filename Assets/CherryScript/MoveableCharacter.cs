using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableCharacter : MonoBehaviour
{

    private Animator _anim;
    private Vector3 _position;
    private bool _isGrounded;

    public float movement_speed = 10f;
    private bool falling;
    private bool facing_right;

    public LayerMask ground_layer;
    public float _groundcheck_distance = 1f;

    public bool handleGravityHere = true;

    // Start is called before the first frame update
    void Start()
    {
        _position = transform.position;
        _position.x = 0;
        _anim = GetComponent<Animator>();
        facing_right = true;
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = CheckGrounded();
        if (!_isGrounded)
        {
            falling = true;
        } else {
            falling = false;
        }
        _anim.SetBool("Falling", falling);
        if (handleGravityHere)
            HandleGravity();
        // MoveCharacter();
    }

    private void HandleGravity()
    {
        if (!_isGrounded) {
            _position.y += Physics2D.gravity.y * 3 * Time.deltaTime;
        } else {
            _position.y = 0f;
        }
    }

    private void Flip()
    {
        facing_right = !facing_right;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void MoveCharacter()
    {
        transform.position += _position * Time.deltaTime;
    }

    private bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _groundcheck_distance,  ground_layer);
        if (hit.collider != null)
            return true;
        return false;
    }
}
