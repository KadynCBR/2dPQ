using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace abilities {
    public abstract class CoolDown: MonoBehaviour
    {
        public string Name;
        public bool Available;
        public float cooldown;

        private float _timeStamp;

        public CoolDown()
        {
            Available = true;
        }

        /// <summary>
        /// Will call Action then setup timers for execution
        /// </summary>
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
