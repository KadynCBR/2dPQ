using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gravity
{
    public class Gravity : MonoBehaviour
    {
        
        public bool _isGrounded;
        public LayerMask ground_layer;
        private float _ground_check_y_offset = -0.96f;
        private float _groundcheck_distance = 0.08f;
        public Vector3 _position;
        // Start is called before the first frame update
        void Start()
        {
            _position = new Vector3(0,0,0);
        }

        // Update is called once per frame
        void Update()
        {
            _isGrounded = CheckGrounded();
            HandleGravity();
        }

        private void HandleGravity()
        {
            if (!_isGrounded) {
                _position.y += Physics2D.gravity.y * 3 * Time.deltaTime;
            } else {
                _position.y = 0f;
            }
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
}
