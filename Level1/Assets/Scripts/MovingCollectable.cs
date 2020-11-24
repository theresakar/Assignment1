using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCollectable : MonoBehaviour
{
    public GameObject[] waypoints;
    int current = 0;
    public float speed;
    float wPointRadius = 1;
  
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(waypoints[current].transform.position, transform.position) < wPointRadius)
        {
            current++;
            if(current >= waypoints.Length)
            {
                current = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speed);
    }
}
