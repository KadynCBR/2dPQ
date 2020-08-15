using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace abilities {
    public abstract class CoolDown: MonoBehaviour
    {
        public string Name;
        public bool Available;
        public float cooldown;
        public string player_input_string;
        protected playerUI _playerUI;
        protected GameObject player;

        private float _timeStamp;

        public CoolDown()
        {
            Available = true;
        }

        public virtual void init(GameObject T, playerUI pui) {
            player = T;
            _playerUI = pui;
        }

        public virtual bool CheckInput() {
            return Input.GetButton(player_input_string);
        }

        public bool Execute()
        {
            if (Available)
            {
                Action();
                Executed();
                return true;
            } else {
                return false;
            }
        }

        public abstract void Action();

        public void Executed()
        {
            Available = false;
            _timeStamp = Time.time + cooldown;
        }

        public void Update()
        {
            if (_timeStamp < Time.time && !Available)
                Available = true;
        }
    }
}
