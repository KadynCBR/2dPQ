using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using abilities;
namespace Ninja
{
    public class NinjaSkills : MonoBehaviour
    {
        public playerUI _playerUI;
        private List<CoolDown> cooldowns = new List<CoolDown>();
        void Start()
        {
            cooldowns.Add(gameObject.AddComponent<FastMove>());
            cooldowns.Add(gameObject.AddComponent<BasicAttack>());
            cooldowns.Add(gameObject.AddComponent<Fireball>());
            foreach(CoolDown c in cooldowns)
            {
                c.init(gameObject, _playerUI);
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

    public class FastMove: CoolDown
    {
        public float distance = 6f;
        private GameObject clone;
        public FastMove()
        {
            Name = "Teleport";
            cooldown = 1f;
            player_input_string = "Fast Move";
        }

        public override void Action()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, vertical, 0f).normalized * distance;
            blink_cosmetic();
            player.transform.position += direction;
            _playerUI.UseShiftSkill(cooldown);
        }

        public void blink_cosmetic()
        {
            clone = Instantiate(player, player.transform.position, player.transform.rotation);
            Destroy(clone.GetComponent<NinjaSkills>());
            Destroy(clone.GetComponentInChildren<Animator>());
            Destroy(clone.GetComponent<BasicController2D>());
            Destroy(clone.GetComponent<PlayerStatus>());
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

        public override void init(GameObject T, playerUI pui)
        {
            player = T;
            _playerUI = pui;
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

    public class Fireball : CoolDown
    {
        public GameObject fireball;
        private Animator _anim;
        public Fireball()
        {
            Name = "Fire ball";
            cooldown = 0.3f;
            player_input_string = "Fire2";
        }

        public override void init(GameObject T, playerUI pui)
        {
            player = T;
            _playerUI = pui;
            _anim = T.GetComponentInChildren<Animator>();
            // fireball = T.GetComponentInChildren<BasicController2D>().fireball;
        }

        public override void Action()
        {
            _anim.SetTrigger("Skill");
            SpawnProjectile();
        }

        private void SpawnProjectile()
        {
            float rotation = player.transform.localScale.x;
            Quaternion rotation_vec = Quaternion.Euler(0, 0, 90+90*-rotation);
            GameObject fireball_spawn = Instantiate(fireball, player.transform.position, Quaternion.identity);
            fireball_spawn.transform.localRotation = rotation_vec;
        }
    }
}