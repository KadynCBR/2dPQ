using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using abilities;

public class Skills : MonoBehaviour
{
    private FastMove fast_move;
    // Start is called before the first frame update
    void Start()
    {
        fast_move = gameObject.AddComponent<FastMove>();
        fast_move.init(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fast Move"))
            fast_move.Execute();

        fast_move.Update();
    }
    
}

public class FastMove: CoolDown
{
    public float distance = 6f;
    private GameObject player;
    private GameObject clone;
    public FastMove()
    {
        Name = "Teleport";
        cooldown = 1f;
    }
    public void init(GameObject T)
    {
        player = T;
    }

    public override void Action()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, vertical, 0f).normalized * distance;
        blink_cosmetic();
        player.transform.position += direction;
    }

    public void blink_cosmetic()
    {
        clone = Instantiate(player, player.transform.position, player.transform.rotation);
        Destroy(clone.GetComponent<Skills>());
        Destroy(clone.GetComponentInChildren<Animator>());
        Destroy(clone.GetComponent<BasicController2D>());
        StartCoroutine(fadeclone(clone, 1, 0));
    }

    IEnumerator fadeclone(GameObject clone, float aTime, float aValue)
    {
        SpriteRenderer sr = clone.GetComponentInChildren<SpriteRenderer>();
        float alpha = sr.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
            sr.material.color = newColor;
            yield return null;
        }
        Destroy(clone);
    }

}

public class BasicAttack : CoolDown 
{
    private GameObject player;
    public BasicAttack()
    {
        Name = "Basic Attack";
        cooldown = 0.3f;
    }

    public void init(GameObject T)
    {
        player = T;
    }


    public override void Action()
    {

    }
}
