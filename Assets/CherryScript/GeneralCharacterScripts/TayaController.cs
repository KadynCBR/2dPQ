using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TayaController : BasicController2D
{
    public override void HandleGravity()
    {
        bool jump = Input.GetButton("Jump");
        if (!_isGrounded && jump && _position.y < 0) {
            _position.y += Physics2D.gravity.y * .15f * 3 * Time.deltaTime;
        } else if (!_isGrounded) {
            _position.y += Physics2D.gravity.y * 3 * Time.deltaTime;
        } else {
            _position.y = 0f;
        }
    }
}
