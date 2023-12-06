using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayableCharacter : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    public GameObject dialogBox2;
    public AudioClip dialogSound;
    AudioSource audioSource;

    float timerDisplay;
    
    void Start()
    {
        dialogBox.SetActive(false);
        dialogBox2.SetActive(false);
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
                dialogBox2.SetActive(false);
            }
        }
    }
    
    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        PlaySound(dialogSound);
    }

    public void DisplayDialogPostBook()
    {
        timerDisplay = displayTime;
        dialogBox2.SetActive(true);
        PlaySound(dialogSound);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}