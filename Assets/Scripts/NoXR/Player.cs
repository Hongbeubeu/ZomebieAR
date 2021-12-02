using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Health health;
    private bool isMoving;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            isMoving = true;
        }

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        if (h == 0 && v == 0)
        {
            isMoving = false;
        }
        if (!isMoving)
        {
            return;
        }
        var dir = new Vector3(h, 0f, v).normalized;
        transform.Translate(0.2f * dir);
        transform.forward = -dir;
    }
}
