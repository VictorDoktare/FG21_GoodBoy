using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DogControllerRedux : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Range(0f, 100f)]
    private float maxSpeed = 10f;
    
    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 10f;
    
    [SerializeField]
    private float playerGravity = 9.81f;

    [SerializeField, Range(20, 80)]
    private float minTurnSpeed = 20f, maxTurnSpeed = 60;
    
    [SerializeField]
    private SimpleOrbitalCamera cameraInputSpace = default;

    [SerializeField]
    private Animator playerAnimator;
    
    [SerializeField]
    private float attackFrequencySeconds;

    [SerializeField]
    private float attackLungeDistance = 5f;
    
    [SerializeField]
    private float dashLength = 2f;

    [SerializeField]
    private float dashCooldown;
    
    [SerializeField, Range(0f,2f)]
    private float airResistance;
    
    
    [SerializeField]
    private ParticleSystem dashParticles;

    [SerializeField]
    private DogStats playerStats;
    
    [SerializeField]
    private float damageFlashDuration;

    [SerializeField]
    private Renderer dogRenderer;
    
    private Material dogMaterial;
    private Color originalColor;
    [ColorUsageAttribute(true,true)]
    public Color damageFlashColor;

    
    public AudioSource attackSFX;
    public AudioSource footstepSFX;
    public AudioSource hurtSFX;


    private Vector3 velocity;

    private CharacterController playerController;
    private bool isAttacking;
    private bool isDashing;
    
    private bool isDead;
    private float lungeProgress;
    public float lungeTime = 3f;

    private bool isDashCooldown;
    private Vector3 lungeMotion;
    private float dashProgress;
    Vector3 desiredVelocity;

    private Vector3 dashVector;
    private bool isFootStepping;


    public delegate void AttackPressed(float howLong);
    public static event AttackPressed OnAttackPressed;

    private void OnEnable()
    {
        EnemyAI.OnTryAttackPlayer += TryDamagePlayer;
    }

    private void OnDisable()
    {
        EnemyAI.OnTryAttackPlayer -= TryDamagePlayer;
    }

    private void TryDamagePlayer()
    {
        if (!isDashing)
        {
            playerStats.TakeDamage(1);
            if (!(playerStats.CurrentHealth <= 0) && transform.gameObject.activeInHierarchy)
            {
                print("hurt");
                hurtSFX.Play();
                StartCoroutine(DamageFlash(damageFlashDuration));
            }
        }
    }

    private IEnumerator DamageFlash(float duration)
    {
        dogMaterial.SetColor("_Emission", damageFlashColor);
        yield return new WaitForSeconds(duration);
        dogMaterial.SetColor("_Emission", Color.black);
    }

    private void Awake()
    {
        dogMaterial = dogRenderer.material;
        originalColor = dogMaterial.color;
        playerController = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PlayerDied()
    {
        isDead = true;
    }

    private void Update()
    {
        if (isDead) return;
        
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        if ((playerInput.x != 0 || playerInput.y != 0) && !isAttacking)
        {
            playerAnimator.SetBool("isRun", true);
            if (!isFootStepping)
            {
                isFootStepping = true;
                StartCoroutine(StartFootStepping());
            }
        }
        else
        {
            StopCoroutine(StartFootStepping());
            footstepSFX.Stop();
            playerAnimator.SetBool("isRun", false);

        }


        if ( Input.GetButtonDown("Fire1") && !isAttacking && playerController.isGrounded && !isDashing)
        {
            playerAnimator.SetTrigger("isAttack");
            isAttacking = true;
            lungeProgress = 0;
            if (cameraInputSpace.IsLockedOn)
            {
                Vector3 planarCamDir = Vector3.ProjectOnPlane(cameraInputSpace.transform.forward, Vector3.up);
                Quaternion lockOnRotation = Quaternion.LookRotation(planarCamDir, Vector3.up);
                transform.rotation = lockOnRotation;
            }
            
            lungeMotion = transform.forward * maxSpeed * attackLungeDistance;
            OnAttackPressed?.Invoke(attackLungeDistance);
            //StartCoroutine(AttackCooldown(attackFrequencySeconds));
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDashCooldown)
        {
            if (isAttacking)
            {
                isAttacking = false;
            }
            
            playerAnimator.SetTrigger("isDash");
            
            dashVector = desiredVelocity * dashLength;
            dashProgress = 0;
            isDashCooldown = true;
            isDashing = true;
            dashParticles.Play();
            StartCoroutine(DashCooldown(dashCooldown));
        }
        
        Movement(playerInput);
    }

    private IEnumerator StartFootStepping()
    {
        footstepSFX.pitch = Random.Range(0.3f, 0.5f);
        footstepSFX.Play();
        yield return new WaitForSeconds(footstepSFX.clip.length + 0.23f);
        StartCoroutine(StartFootStepping());
    }

    IEnumerator AttackCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }

    IEnumerator DashCooldown(float dashCooldownTime)
    {
        yield return new WaitForSeconds(dashCooldownTime);
        isDashCooldown = false;
    }
    
    private void Movement(Vector2 playerInput)
    {
        if (cameraInputSpace)
        {
            Vector3 forward = cameraInputSpace.transform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = cameraInputSpace.transform.right;
            right.y = 0f;
            right.Normalize();

            desiredVelocity =
                (forward * playerInput.y + right * playerInput.x) * maxSpeed;
            
        }
        else
        {
            Debug.LogError("No camera assigned, defaulting to north/south/west/east movement", this);
            desiredVelocity =
                new Vector3(playerInput.x, 0, playerInput.y) * maxSpeed;
        }
        
        HandleRotation(playerInput);

        
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        

        if (!isDashing && !isAttacking)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
            velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
        }
        else
        {
            if (isAttacking)
            {
                lungeProgress += airResistance*Time.deltaTime;
                Vector3 lunge = Vector3.Lerp(lungeMotion, Vector3.zero, lungeProgress);

                velocity.x = lunge.x;
                velocity.z = lunge.z;
                
                
                if (lungeProgress > 1)
                {
                    isAttacking = false;
                }
            }
            
            else
            {
                dashProgress += airResistance*Time.deltaTime;
                velocity = Vector3.Lerp(dashVector, Vector3.zero, dashProgress);

                if (dashProgress > 1)
                {
                    isDashing = false;
                }
            }
        }

        
        velocity.y = -playerGravity;
       

        Vector3 displacement = velocity * Time.deltaTime;

        playerController.Move(displacement);
        
        // if (isDashing)
        // {
        //     dashProgress += Time.deltaTime;
        //     float t = lungeProgress / lungeTime;
        //     
        //     if (t > 1f)
        //     {
        //         isAttacking = false;
        //     }
        // }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
       velocity -= hit.normal * Vector3.Dot(velocity, hit.normal);
    }


    private void HandleRotation(Vector2 playerInput)
    {
        float currentTurnSpeed = velocity.magnitude / 5;
        float playerTurnSpeed = Mathf.Lerp(maxTurnSpeed, minTurnSpeed, currentTurnSpeed);

        if (cameraInputSpace.IsLockedOn && isAttacking)
        {
            Vector3 planarCamDir = Vector3.ProjectOnPlane(cameraInputSpace.transform.forward, Vector3.up);
            Quaternion lockOnRotation = Quaternion.LookRotation(planarCamDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lockOnRotation, playerTurnSpeed * Time.deltaTime);
        }
        else
        {
            // Only rotate if player has moved
            if (playerInput.sqrMagnitude > 0)
            {
                Quaternion faceRotation =
                    Quaternion.LookRotation(new Vector3(desiredVelocity.x, 0f, desiredVelocity.z));
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, faceRotation, playerTurnSpeed * Time.deltaTime);
                // if player is at a stand-still, instantly rotate to wards the given direction 
                // LIKE IN MARIO
                // if (velocity.sqrMagnitude <= 0.01f)
                // {
                //     Quaternion snapRotation =
                //         Quaternion.LookRotation(new Vector3(playerInput.x, 0f, playerInput.y));
                //     transform.rotation =
                //         Quaternion.Slerp(transform.rotation, snapRotation, playerTurnSpeed * Time.deltaTime);
                // }
                // else
                // {
                //     Quaternion faceRotation =
                //         Quaternion.LookRotation(new Vector3(desiredVelocity.x, 0f, desiredVelocity.z));
                //     transform.rotation =
                //         Quaternion.Slerp(transform.rotation, faceRotation, playerTurnSpeed * Time.deltaTime);
                // }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 displacement = velocity * Time.deltaTime;
        Gizmos.DrawRay(transform.position, displacement);
    }
}
