using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using abilities;

namespace Taya
{
    public class TayaSkills : MonoBehaviour
    {
        private List<CoolDown> cooldowns = new List<CoolDown>();
        void Start()
        {
            // cooldowns.Add(gameObject.AddComponent<FastMove>());
            cooldowns.Add(gameObject.AddComponent<BasicAttack>());
            foreach(CoolDown c in cooldowns)
            {
                c.init(gameObject);
            }
        }

        void Update()
        {
            foreach(CoolDown c in cooldowns)
            {
                if (c.CheckInput())
                    c.Execute();
                c.Update();
            }
        }
    }

    // public class FastMove: CoolDown
    // {
    //     public float distance = 6f;
    //     private GameObject clone;
    //     public FastMove()
    //     {
    //         Name = "Teleport";
    //         cooldown = 1f;
    //         player_input_string = "Fast Move";
    //     }

    //     public override void Action()
    //     {
    //         float horizontal = Input.GetAxisRaw("Horizontal");
    //         float vertical = Input.GetAxisRaw("Vertical");
    //         Vector3 direction = new Vector3(horizontal, vertical, 0f).normalized * distance;
    //         blink_cosmetic();
    //         player.transform.position += direction;
    //     }

    //     public void blink_cosmetic()
    //     {
    //         clone = Instantiate(player, player.transform.position, player.transform.rotation);
    //         Destroy(clone.GetComponent<NinjaSkills>());
    //         Destroy(clone.GetComponentInChildren<Animator>());
    //         Destroy(clone.GetComponent<BasicController2D>());
    //         StartCoroutine(fadeclone(clone, 1, 0));
    //     }
    // }

    public class BasicAttack : CoolDown 
    {
        private Animator _anim;
        private Transform attackPoint;
        private float attackRange;
        private float damage = 30f;
        private LayerMask enemyLayers;
        public BasicAttack()
        {
            Name = "Basic Attack";
            cooldown = 0.3f;
            player_input_string = "Fire1";
            attackRange = 0.5f;
        }

        public override void init(GameObject T)
        {
            player = T;
            _anim = T.GetComponentInChildren<Animator>();
            enemyLayers = 1 << LayerMask.NameToLayer("Enemy"); // dont use index, must shift to get bits.
            attackPoint = player.transform.Find("attackPoint");
        }

        public override void Action()
        {
            _anim.SetTrigger("BasicAttack");
            CheckHit();
        }

        private void CheckHit()
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<enemyBasic>().OnHit(damage);
            }
        }
    }
}