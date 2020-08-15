using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using abilities;

namespace Taya
{
    public class TayaSkills : MonoBehaviour
    {
        public playerUI _playerUI;
        private List<CoolDown> cooldowns = new List<CoolDown>();
        void Start()
        {
            cooldowns.Add(gameObject.AddComponent<FastMove>());
            cooldowns.Add(gameObject.AddComponent<BasicAttack>());
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
        private Animator _anim;
        private TayaController playercontrol;
        public FastMove()
        {
            Name = "Duty's Call";
            cooldown = 6f;
            player_input_string = "Fast Move";
        }

        public override void init(GameObject T, playerUI pui)
        {
            player = T;
            _playerUI = pui;
            _anim = player.GetComponentInChildren<Animator>();
            playercontrol = player.GetComponentInChildren<TayaController>();
            Debug.Log(playercontrol);
        }

        public override void Action()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, vertical, 0f).normalized * distance;
            _anim.speed = 4f;
            playercontrol.default_speed = 17f;
            _playerUI.UseShiftSkill(cooldown);
            // ADD GLOW AND REMOVER GLOW IN DURATION OVER
            StartCoroutine(AnimationSpeedUpDurationOver(4));
        }

        IEnumerator AnimationSpeedUpDurationOver(float bufftime)
        {
            for (float t = 0.0f; t < 1.0f; t+= Time.deltaTime / bufftime)
            {
                yield return null;
            }
            _anim.speed = 1.0f;
            playercontrol.default_speed = 10f;
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
}