using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogcontrollerbugfix : MonoBehaviour
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
    private float attackFrequencySeconds;

    [SerializeField]
    private float attackLungeDistance = 5f;

    private Vector3 lungeStart;
    private Vector3 lungeTarget;

    [SerializeField]
    private AnimationCurve jump;
    

    private Vector3 velocity;

    private CharacterController playerController;
    private bool isAttacking;
    private bool isDashing;
    
    private bool isDead;
    private float lungeProgress;
    public float lungeTime = 3f;
    public float lungeSpeed = 1f;
    
    [SerializeField]
    private float lungeHeight;
    private Vector3 lungePos;

    [SerializeField]
    private float dashLength = 2f;

    [SerializeField]
    private float dashCooldown;


    private bool isDashCooldown;
    private Vector3 lungeMotion;
    private bool startLunge;
    private bool startDash;
    private float dashProgress;
    Vector3 desiredVelocity; 

    [SerializeField, Range(0f,1f)]
    private float airResistance;

    private Vector3 dashVector;

    public delegate void AttackPressed(float howLong);
    public static event AttackPressed OnAttackPressed; 

    private void Awake()
    {
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
        
        
        


        if ( Input.GetButtonDown("Fire1") && !isAttacking && playerController.isGrounded && !isDashing)
        {
            AnimationManager.Instance.PlayerAnimator.SetTrigger("isAttack");
            isAttacking = true;
            lungeProgress = 0;
            var playerTransform = transform;
            lungeStart = playerTransform.position;
            if (cameraInputSpace.IsLockedOn)
            {
                Vector3 planarCamDir = Vector3.ProjectOnPlane(cameraInputSpace.transform.forward, Vector3.up);
                Quaternion lockOnRotation = Quaternion.LookRotation(planarCamDir, Vector3.up);
                transform.rotation = lockOnRotation;
            }
            // lungeTarget = lungeStart + playerTransform.forward;
            lungeTarget = lungeStart + playerTransform.forward * attackLungeDistance;
            lungeMotion = transform.forward * maxSpeed * attackLungeDistance;
            OnAttackPressed?.Invoke(lungeTime);
            //StartCoroutine(AttackCooldown(attackFrequencySeconds));
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDashCooldown)
        {
            if (isAttacking)
            {
                isAttacking = false;
            }
            dashVector = desiredVelocity * dashLength;
            dashProgress = 0;
            isDashCooldown = true;
            isDashing = true;
            StartCoroutine(DashCooldown(dashCooldown));
        }
        
        Movement(playerInput);
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
            //forward.Normalize();

            Vector3 right = cameraInputSpace.transform.right;
            right.y = 0f;
            //right.Normalize();

            desiredVelocity =
                (forward * playerInput.y + right * playerInput.x) * maxSpeed;
        }
        else
        {
            Debug.LogError("No camera assigned, defaulting to north/south/west/east movement", this);
            desiredVelocity =
                new Vector3(playerInput.x, 0, playerInput.y) * maxSpeed;
        }
        
        HandleRotation(playerInput, desiredVelocity);

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

    private void FixedUpdate()
    {
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
       // velocity.y -= playerGravity * Time.deltaTime;
       
       velocity.y = -playerGravity;

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
                float t = lungeProgress / lungeTime;
                velocity = Vector3.Lerp(lungeMotion, Vector3.zero, t);
                velocity.y = Mathf.Sin(t * Mathf.PI * 2) * lungeHeight;
                
                    

                //Vector3 moveDelta = lungePos - velocity;

                //velocity += moveDelta * Time.deltaTime;
                
                if (t > 0.8f)
                {
                    isAttacking = false;
                }
            }
            // if (isAttacking)
            // {
            //     lungeProgress += Time.fixedDeltaTime;
            //     float t = lungeProgress/lungeTime;
            //     lungePos = Vector3.Lerp(lungeMotion * attackLungeDistance, Vector3.zero, t)
            //                + Vector3.up * (-4 * Mathf.Pow(t - 0.5f, 2) + 1) * lungeHeight;;
            //
            //     Vector3 moveDelta = lungePos - velocity;
            //
            //     velocity += moveDelta * Time.deltaTime;
            //     
            //     if (lungeProgress > 1)
            //     {
            //         isAttacking = false;
            //     }
            
            // if (isAttacking)
            // {
            //     lungeProgress += Time.fixedDeltaTime;
            //     float t = lungeProgress / lungeTime;
            //     lungePos = Vector3.Lerp(lungeMotion * attackLungeDistance, Vector3.zero, t)
            //                + Vector3.up * (-4 * Mathf.Pow(t - 0.5f, 2) + 1) * lungeHeight;;
            //
            //     Vector3 moveDelta = lungePos - lungeStart;
            //
            //     velocity = moveDelta * Time.deltaTime;
            //     
            //     if (lungeProgress > 1)
            //     {
            //         isAttacking = false;
            //     }
            
            // if (isAttacking)
            // {
            //     lungeProgress += Time.fixedDeltaTime;
            //     float t = lungeProgress / lungeTime;
            //     velocity = Vector3.Lerp(lungeMotion * attackLungeDistance, Vector3.zero, lungeProgress) 
            //                +  Vector3.up * (-4 * Mathf.Pow(lungeProgress - 0.5f, 2) + 1) * lungeHeight;
            //
            //     if (lungeProgress > 1)
            //     {
            //         isAttacking = false;
            //     }
            // }
            
            // else
            // {
            //     if (isAttacking)
            //     {
            //         lungeProgress += airResistance*Time.fixedDeltaTime;
            //         velocity = Vector3.Lerp(lungeMotion * attackLungeDistance, Vector3.zero, lungeProgress) 
            //                    + (Vector3.up * ((lungeProgress-0.5f)*(lungeProgress-0.5f))) * lungeHeight;
            //
            //         if (lungeProgress > 1)
            //         {
            //             isAttacking = false;
            //         }
            //     }
            else
            {
                dashProgress += airResistance*Time.fixedDeltaTime;
                velocity = Vector3.Lerp(dashVector, Vector3.zero, dashProgress);

                if (dashProgress > 0.8f)
                {
                    isDashing = false;
                }
            }
        }

       

        Vector3 displacement = velocity * Time.deltaTime;

        playerController.Move(displacement);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
       velocity -= hit.normal * Vector3.Dot(velocity, hit.normal);
    }


    private void HandleRotation(Vector2 playerInput, Vector3 desiredVelocity)
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
