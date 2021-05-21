using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollbg : MonoBehaviour
{
    public float scrollspeed = 0.3f;

    void Start()
    {
        
    }

    void Update()
    {
        gameObject.transform.position = gameObject.transform.position + new Vector3(scrollspeed * Time.deltaTime, 0, 0);
        if (gameObject.transform.position.x >= 24f || gameObject.transform.position.x <= -24f) {
            gameObject.transform.position = new Vector3(0, gameObject.transform.position.y, 0);
        }
    }
}
