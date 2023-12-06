using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {  
            controller.Trip();
            Destroy(gameObject);
        }
    }
}
