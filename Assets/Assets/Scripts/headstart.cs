using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headstart : MonoBehaviour
{
    float boost;
    int pause;
    bool forward;
    float z;
    // Start is called before the first frame update
    void Start()
    {
        boost = 0;
        forward = true;
        pause = 50;
        z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (pause == 50)
        {
            if (boost >= 15)
            {
                forward = false;
                pause = 0;
            }
            else if (boost <= 0)
            {
                forward = true;
                pause = 0;
            }
            boost += forward ? (float)0.01 : (float)-0.01;
            transform.position = new Vector3(transform.position.x, transform.position.y, z - boost);
        }
        else
        {
            pause += 1;
        }
    }
}
