using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public AudioClip collectedClip;

    public float timeInvincible = 10.0f;
    bool isInvincible;
    float invincibleTimer;

void Update()
{
    if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
}

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {  
            //if(controller.health < controller.maxHealth)
            //{
            controller.PlaySound(collectedClip);
            controller.PoweredUp(true);
	        Destroy(gameObject);
            //}
        }
    }
}
