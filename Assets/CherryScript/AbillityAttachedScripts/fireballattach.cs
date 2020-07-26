using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballattach : MonoBehaviour
{
    public float speed = .3f;
    public int damage = 10;
    public float lifetime = 5f;
    private float start_time;

    // Start is called before the first frame update
    void Start()
    {
        start_time = Time.time + lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += transform.right*speed*Time.deltaTime;
        if (Time.time > start_time)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            other.GetComponent<enemyBasic>().OnHit(damage);
            Destroy(this.gameObject); 
        }
    }
}
