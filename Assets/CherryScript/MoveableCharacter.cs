using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableCharacter : MonoBehaviour
{

    private Animator _anim;
    private Vector3 _position;
    private bool _isGrounded;
    private bool _isJumping;
    private Vector3 directional_input;

    public float movement_speed = 10f;
    private bool falling;
    private bool facing_right;

    public LayerMask ground_layer;
    private float _ground_check_y_offset = -0.96f;
    private float _groundcheck_distance = 0.08f;

    public float character_speed = 10f;
    public float JumpHeight = 7.5f;

    public bool handleGravityHere = true;
    public EnemyAI ai;

    // Start is called before the first frame update
    void Start()
    {
        _position = new Vector3(0,0,0);
        _position.x = 0;
        _anim = GetComponentInChildren<Animator>();
        ai = GetComponent<EnemyAI>();
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
        if (directional_input.x == 0)
            _position.x = 0;
        HandleAIInput(ai.direction);
        MoveCharacter();
    }

    public void HandleAIInput(Vector3 inputs)
    {
        directional_input = inputs;
        if (directional_input.x > .3f && !facing_right) {
            Flip();
        } else if (directional_input.x < -.3f && facing_right) {
            Flip();
        }
        // Position update and speed
        _position.x = directional_input.x * character_speed;
        _anim.SetFloat("speed", Mathf.Abs(directional_input.x * character_speed));
        HandleJumping((directional_input.y >= .9f));
    }    
    public virtual void HandleJumping(bool jump)
    {
        if (_isGrounded && jump)
        {
            _isJumping = true;
            _position.y += Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
        } else if (_isGrounded && _isJumping) {
            _isJumping = false;
        }
        _anim.SetBool("Jump", _isJumping);
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
        Vector3 offset_vector = new Vector3(0, _ground_check_y_offset, 0);
        Debug.DrawRay(transform.position + offset_vector, Vector2.down*_groundcheck_distance, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset_vector, Vector2.down, _groundcheck_distance,  ground_layer);
        if (hit.collider != null)
            return true;
        return false;
    }
}
