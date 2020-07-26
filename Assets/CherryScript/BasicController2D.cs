using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using abilities;

public class BasicController2D : MonoBehaviour
{

    private Animator _anim;
    private Vector3 _position;
    private bool _isGrounded;
    public GameObject fireball;

    public float player_speed = 10f;
    public float JumpHeight = 3f;
    private bool falling;
    private bool facing_right;
    private bool _isCrouching;
    private bool _isJumping;

    public CapsuleCollider2D hitbox;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public LayerMask ground_layer;
    private float _ground_check_y_offset = -0.96f;
    private float _groundcheck_distance = 0.08f;
    public float attackskill_cooldown = .5f;
    private float next_attack_time;

    public bool debug = true;
    // Start is called before the first frame update
    void Start()
    {
        _position = transform.position;
        _anim = GetComponentInChildren<Animator>();
        hitbox = GetComponentInChildren<CapsuleCollider2D>();
        attackPoint = transform.GetChild(1);
        facing_right = true;
        _isCrouching = false;
        _isJumping = false;
        next_attack_time = Time.time;
        
    }

    private void DebugOut()
    {
        if (debug) 
        {
            Debug.Log(_isGrounded);
            // Debug.DrawRay(transform.position, Vector2.down, Color.green);
        }
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
        HandleGravity();
        HandleInput();
        MoveCharacter();
        DebugOut();
    }

    private void HandleInput()
    {
        float directional = Input.GetAxis("Horizontal");
        if (directional > 0 && !facing_right) {
            Flip();
        } else if (directional < 0 && facing_right) {
            Flip();
        }
        
        // Crouching
        _isCrouching = Input.GetButton("Crouch");
        if (_isCrouching && _isGrounded)
        {
            hitbox.offset = new Vector2(0.01f, -.44f);
            hitbox.size = new Vector2(1.1f, 1.08f);
            if (player_speed > 0f)
                player_speed -= 5f * Time.deltaTime;
            _anim.SetBool("crouching", true);
        } else {
            hitbox.offset = new Vector2(0.01f, 0f);
            hitbox.size = new Vector2(1.1f, 2.03f);
            player_speed = 10f;
            _anim.SetBool("crouching", false);
        }

        // Position update and speed
        _position.x = directional * player_speed;
        _anim.SetFloat("speed", Mathf.Abs(directional * player_speed));
        
        // Jump
        bool jump = Input.GetButton("Jump");
        if (_isGrounded && jump)
        {
            _isJumping = true;
            _position.y += Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
        } else if (_isGrounded && _isJumping) {
            _isJumping = false;
        }
        _anim.SetBool("Jump", _isJumping);

        // Attacks
        HandleAttacks();
        // HandleFastMove();
    }

    private void MoveCharacter()
    {
        transform.position += _position * Time.deltaTime;
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

    private bool CheckGrounded()
    {
        Vector3 offset_vector = new Vector3(0, _ground_check_y_offset, 0);
        Debug.DrawRay(transform.position + offset_vector, Vector2.down*_groundcheck_distance, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset_vector, Vector2.down, _groundcheck_distance,  ground_layer);
        if (hit.collider != null)
            return true;
        return false;
    }


    private void HandleAttacks()
    {
        if (Time.time >= next_attack_time)
        {
            if (Input.GetButton("Fire1")) // left click basic attack
            {
                _anim.SetTrigger("BasicAttack");
                next_attack_time = Time.time + attackskill_cooldown;
                Checkhit();
            } else if (Input.GetButton("Fire2"))
            {
                next_attack_time = Time.time + attackskill_cooldown;
                _anim.SetTrigger("Skill");
                SpawnProjectile();
            }
        }
    }

    private void Checkhit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<enemyBasic>().OnHit(30);
        }
    }

    private void SpawnProjectile() {
        float rotation = transform.localScale.x;
        Quaternion rotation_vec = Quaternion.Euler(0, 0, 90+90*-rotation);
        GameObject fireball_spawn = Instantiate(fireball, transform.position, Quaternion.identity);
        fireball_spawn.transform.localRotation = rotation_vec;
    }
}
