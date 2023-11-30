using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    
    public int maxHealth = 5;
    public int fixedRobots = 0;
    public int maxRobots = 2;

    public GameObject projectilePrefab;
    public GameObject LoseEndGame;
    //public GameObject WinEndGame;

    public AudioClip throwSound;
    public AudioClip hitSound;
    
    public int health { get { return currentHealth; }}
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    AudioSource audioSource;

    public ParticleSystem healthParticles;
    public ParticleSystem hitParticles;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        LoseEndGame.SetActive(false);
        //WinEndGame.SetActive(false);
        currentHealth = maxHealth;
        //EnemyController = gameObject.GetComponent<EnemyController>();
        
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
                
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {       
                NonPlayableCharacter character = hit.collider.GetComponent<NonPlayableCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();               
                }
            }
        }

        if (currentHealth == 0)
        {
            GetComponent<Rigidbody2D>().simulated = false;
            animator.SetTrigger("Dead");
            LoseEndGame.SetActive(true);
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }  
        }

        //if (broken = false)
        //{
            //fixedRobots++;
            //if (fixedRobots == maxRobots)
            //{
                //GetComponent<Rigidbody2D>().simulated = false;
                //animator.SetTrigger("Dead");
                //WinEndGame.SetActive(true);
                //if(Input.GetKeyDown(KeyCode.R))
                //{
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //}  
            //}
        //}
    }
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            
            animator.SetTrigger("Hit");
            hitParticles.Play();
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(hitSound);
            
        }
        if (amount > 0)
            {
                healthParticles.Play();
            }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);    
    }

    //public void GameOver()
    //{
      //  if (currentHealth <= 0)
        //{
          //  GamePlayController.instance.RestartGame();
        //}
    //}

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(throwSound);
    }

    public void PlaySound(AudioClip collectedClip)
    {
        audioSource.PlayOneShot(collectedClip);
    }
    
}