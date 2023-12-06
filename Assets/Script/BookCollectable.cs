using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCollectable : MonoBehaviour
{
    public AudioClip collectedClip;
    public ParticleSystem magicEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller.bookCollect == 0)
        {
            controller.CollectBook(1);
            magicEffect.Stop();
	        Destroy(gameObject);
            controller.PlaySound(collectedClip);
        }
    }
}
