using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepKing : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;
    public AudioClip dialogSound;

    AudioSource audioSource;

    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            

            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        PlaySound(dialogSound);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
