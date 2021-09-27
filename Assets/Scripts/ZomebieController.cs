using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomebieController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Move()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.Translate(Vector3.forward * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
