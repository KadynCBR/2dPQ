using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBasic : MonoBehaviour
{
    private Animator _anim;
    public float hp = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit(int damageTaken)
    {
        _anim.SetTrigger("Hurt");
        this.hp -= damageTaken;
        if (this.hp <= 0)
            Die();
        Debug.LogFormat("HP: {0}", this.hp);
    }

    void Die()
    {
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(this.gameObject, 1);
    }
}
