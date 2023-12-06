using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    
    public int maxHealth = 5;
    public int bookCollect = 1;
    public int maxRobots = 2;

    public GameObject projectilePrefab;
    public GameObject LoseEndGame;
    public GameObject WinEndGame;

    public GameObject bookCollectUI;

    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip tripSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    
    public int health { get { return currentHealth; }}
    int currentHealth;
    int fixedRobots;
    public string fixedRobotsString;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    //Javon Part Start
    public AudioClip sheepSound;
    //bool speedBoost;
    float speedTimer;
    float speedTracker;
    //Javon Part End
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    AudioSource audioSource;
    bool isMovementEnabled = true;
    Coroutine stopMovementCoroutine;

    public ParticleSystem healthParticles;
    public ParticleSystem hitParticles;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        LoseEndGame.SetActive(false);
        bookCollectUI.SetActive(false);
        WinEndGame.SetActive(false);
        currentHealth = maxHealth;
        fixedRobots = 0;
        bookCollect = 0;
        
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        fixedRobotsString = fixedRobots.ToString(); 

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
                //SheepKing character2 = hit.collider.GetComponent<SheepKing>();
                if (character != null)
                {
                    if (bookCollect == 0){
                        character.DisplayDialog();  
                    }
                    if (bookCollect == 1){
                        character.DisplayDialogPostBook();  
                    }             
                }
                //if (character2 != null)
                //{
                    //character2.DisplayDialog();             
                //}
            }
        }

        if (currentHealth == 0)
        {
            PlaySound(loseSound);
            GetComponent<Rigidbody2D>().simulated = false;
            animator.SetTrigger("Dead");
            LoseEndGame.SetActive(true);
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }  
        }

        if (bookCollect == 1 && fixedRobots == 2)
        {
            GetComponent<Rigidbody2D>().simulated = false;
            animator.SetTrigger("Dead");
            PlaySound(winSound);
            WinEndGame.SetActive(true);
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }  
        }
    }
    void FixedUpdate()
    {
        if (isMovementEnabled)
        {
            Vector2 position = rigidbody2d.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;

            rigidbody2d.MovePosition(position);
        }
        else
        {
            rigidbody2d.velocity -= rigidbody2d.velocity * 0.01f * Time.deltaTime;
            if (rigidbody2d.velocity.magnitude < 0.01f)
            {
                rigidbody2d.velocity = Vector2.zero;
            }
        }
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

    public void CollectBook(int amount)
    {
        bookCollect = 1;
        bookCollectUI.SetActive(true);
    }

    public void FixRobotCommand(int amount)
    {
        fixedRobots = Mathf.Clamp(fixedRobots + amount, 0, maxRobots);
        Debug.Log(fixedRobots);
    }

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
    
    public void Trip()
    {
        if (isMovementEnabled)
        {
            PlaySound(tripSound);
            isMovementEnabled = false;
            animator.SetTrigger("Trip");
            stopMovementCoroutine = StartCoroutine(StopMovementForSeconds(2f));
        }
    }
    IEnumerator StopMovementForSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);
        isMovementEnabled = true;
    }

    public void PoweredUp(bool amount)
    {
        //speedBoost = true;
        speedTimer = 10.0f;
        speed = 6.0f;
        speedTracker = speedTimer;

        isInvincible = true;
        timeInvincible = 10.0f;
        invincibleTimer = timeInvincible;
    }
}