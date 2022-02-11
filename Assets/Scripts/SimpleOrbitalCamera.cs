using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

[RequireComponent(typeof(Camera))]
public class SimpleOrbitalCamera : MonoBehaviour
{
    [SerializeField]
    private Transform focus = default;

    [SerializeField]
    private Transform player = default;

    [SerializeField, Min(0f)]
    private float focusRadius = 1f;

    [SerializeField, Range(0f, 1f)]
    private float focusCentering = 0.5f;

    [SerializeField, Range(1f, 20f)]
    private float distance = 5f;

    [SerializeField, Range(1f, 360f)]
    private float rotationSpeed = 90f;

    [SerializeField, Range(-89f, 89f)]
    private float minVerticalAngle = -30f, maxVerticalAngle = 60f;

    [SerializeField, Min(0f)]
    private float alignDelay = 5f;

    [SerializeField, Range(0f, 90f)]
    private float alignSmoothRange = 45f;
    
    [SerializeField]
    private LayerMask obstructionMask = -1;

    [Header("Lock-on")]

    [SerializeField]
    private float lockOnDistance = 20f;
    
    [SerializeField, Range(1f, 10f), Min(0f)]
    private float lockOnHeight = 3f;

    public LayerMask validLockOnTargets;
    

    private Collider[] nearbyTargets = new Collider[60];
    
    private List<ITargetable> lockOnTargets = new List<ITargetable>();
    
    private bool lockedOn;
    public bool IsLockedOn => lockedOn;

    private float lastManualRotationTime;
    private Vector3 focusPoint, previousFocusPoint;

    private Vector2 orbitAngles = new Vector2(45f, 0f);

    private Camera regularCamera;

    private ITargetable lockOnTarget;
    private RaycastHit[] targetObfuscators = new RaycastHit[1];


    private void OnValidate()
    {
        if (maxVerticalAngle < minVerticalAngle)
        {
            maxVerticalAngle = minVerticalAngle;
        }
    }

    private void Awake()
    {
        regularCamera = GetComponent<Camera>();
        focusPoint = focus.position;
        transform.localRotation = Quaternion.Euler(orbitAngles);
    }

    private Vector3 CameraHalfExtends
    {
        get
        {
            Vector3 halfExtends;
            halfExtends.y = regularCamera.nearClipPlane * Mathf.Tan(0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);
            halfExtends.x = halfExtends.y * regularCamera.aspect;
            halfExtends.z = 0f;
            return halfExtends;
        }
    }

    private void ToggleLockOn(bool toggle)
    {
        if (toggle == lockedOn) return;
        
        lockedOn = !lockedOn;
        
        if (lockedOn)
        {

            lockOnTargets = new List<ITargetable>();
            lockOnTargets.Clear();

            int hits = Physics.OverlapSphereNonAlloc(player.position, lockOnDistance, nearbyTargets, validLockOnTargets);
            
            for (int i = 0; i < hits; i++)
            {
                ITargetable target = nearbyTargets[i].GetComponent<ITargetable>();
                if (target != null)
                {
                    if (OnScreen(target))
                        if (NotBlocked(target))
                            if (InDistance(target))
                            {
                                lockOnTargets.Add(target);
                            }
                }
            }

            float hypotenuse;
            float smallestHypotenuse = Mathf.Infinity;

            // float curDist;
            // float closestDistance = Mathf.Infinity;

            ITargetable closestTarget = null;
            foreach (ITargetable target in lockOnTargets)
            {
                // curDist = Vector3.Distance(player.position, target.TargetTransform.position);
                // print($"{target.TargetTransform.root.name} is {curDist} away");
                // if (closestDistance > curDist)
                // {
                //     closestDistance = curDist;
                //     closestTarget = target;
                // }
                hypotenuse = CalculateHypotenuse(target.TargetTransform.position);
                if (smallestHypotenuse > hypotenuse)
                {
                    closestTarget = target;
                    smallestHypotenuse = hypotenuse;
                }
            }

            lockOnTarget = closestTarget;
            lockedOn = lockOnTarget != null;
            lockOnTarget?.ToggleCircle(true);
            
        }
        else
        {
            lockOnTarget.ToggleCircle(false);
        }
    }

    private float CalculateHypotenuse(Vector3 position)
    {
        float screenCenterX = regularCamera.pixelWidth / 2;
        float screenCenterY = regularCamera.pixelHeight / 2;
    
        Vector3 screenPosition = regularCamera.WorldToScreenPoint((position));
        float deltaX = screenCenterX - screenPosition.x;
        float deltaY = screenCenterY - screenPosition.y;
    
        float hypotenuse = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
    
        return hypotenuse;
    }

    private bool OnScreen(ITargetable targetable)
    {
        Vector3 viewPortPosition = regularCamera.WorldToViewportPoint(targetable.TargetTransform.position);
        if (!(viewPortPosition.x > 0) || !(viewPortPosition.x < 1)) { return false; }
        if (!(viewPortPosition.y > 0) || !(viewPortPosition.y < 1)) { return false; }
        if (!(viewPortPosition.z > 0)) { return false; }

        return true;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ToggleLockOn(!lockedOn);
        }
        UpdateFocusPoint();
        Quaternion lookRotation;
        
        if (ManualRotation() || AutomaticRotation() || LockOnRotation())
        {
            ConstrainAngles();
            lookRotation = Quaternion.Euler(orbitAngles);
        }
        else
        {
            lookRotation = transform.localRotation;
        }
        
        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = player.position - lookDirection * distance;

        Vector3 rectOffset = lookDirection * regularCamera.nearClipPlane;
        Vector3 rectPosition = lookPosition + rectOffset;
        Vector3 castFrom = focus.position;
        Vector3 castLine = rectPosition - castFrom;
        float castDistance = castLine.magnitude;
        Vector3 castDirection = castLine / castDistance;

        if (Physics.BoxCast(
            castFrom, CameraHalfExtends, castDirection, out RaycastHit hit, 
            lookRotation, castDistance, obstructionMask))
        {
            rectPosition = castFrom + castDirection * hit.distance;
            lookPosition = rectPosition - rectOffset;
        }

        transform.SetPositionAndRotation(lookPosition, lookRotation);

        if (lockedOn && lockOnTarget.TargetTransform != null)
        {
            bool stillValid = OnScreen(lockOnTarget) && InDistance(lockOnTarget) && NotBlocked(lockOnTarget);
            lockedOn = stillValid;
            if (!stillValid)
                lockOnTarget.ToggleCircle(false);
        }

        if (lockedOn && lockOnTarget.TargetTransform == null)
        {
            lockedOn = false;
        }
    }

    private bool NotBlocked(ITargetable targetable)
    {
        Vector3 origin = transform.position + transform.forward;
        Vector3 direction = targetable.TargetTransform.position - origin;

        float radius = 0.15f;
        float distance = direction.magnitude;

        int obfuscatingHits = Physics.SphereCastNonAlloc(origin, radius, direction.normalized, targetObfuscators, distance, obstructionMask);

        return obfuscatingHits == 0;
    }

    private bool InDistance(ITargetable targetable)
    {
        return Vector3.Distance(player.position, targetable.TargetTransform.position) < lockOnDistance;
    }

    private void ConstrainAngles()
    {
        orbitAngles.x =
            Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

        if (orbitAngles.y < 0)
        {
            orbitAngles.y += 360f;
        } 
        else if (orbitAngles.y >= 360f)
        {
            orbitAngles.y -= 360f;
        }
    }

    private void UpdateFocusPoint()
    {
        previousFocusPoint = focusPoint;
        Vector3 targetPoint = focus.position;

        if (focusRadius > 0f)
        {
            float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;

            if (distance > 0.01f && focusCentering > 0f)
            {
                t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
            }
            if (distance > focusRadius)
            {
                t = Mathf.Min(t, focusRadius / distance);
            }
            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
        }
        else
        {
            focusPoint = targetPoint;
        }
    }
    
    private bool LockOnRotation()
    {
        if (!lockedOn || lockOnTarget.TargetTransform == null)
        {
            return false;
        }
        lastManualRotationTime = Time.unscaledTime;
        
        var lockOnPosition = lockOnTarget.TargetTransform.position;
        var cameraTransform = transform;
        var cameraPosition = cameraTransform.position;
        
        Vector2 mov = new Vector2(
            lockOnPosition.x - cameraPosition.x,
            lockOnPosition.z - cameraPosition.z);

        float movSqr = mov.sqrMagnitude;

        Vector2 normMov = mov / Mathf.Sqrt(movSqr);
        
        float headingAngle = GetAngle(normMov);
        
        orbitAngles.y = headingAngle;
        orbitAngles.x = lockOnHeight;

        // distance = Vector3.Distance(player.position, lockOnPosition);
        
        return true;
    }

    private bool AutomaticRotation()
    {
        if (lockedOn) return false;
        if (Time.unscaledTime - lastManualRotationTime < alignDelay)
        {
            return false;
        }

        
        Vector2 movement = new Vector2(
            focusPoint.x - previousFocusPoint.x,
            focusPoint.z - previousFocusPoint.z
        );

        float movementDeltaSqr = movement.sqrMagnitude;
        if (movementDeltaSqr < 0.00001f)
        {
            return false;
        }

        float headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
        float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(orbitAngles.y, headingAngle));
        
        float rotationChange = rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
        
        if (deltaAbs < alignSmoothRange)
        {
            rotationChange *= deltaAbs / alignSmoothRange;
        }
        else if (180f - deltaAbs < alignSmoothRange)
        {
            rotationChange *= (180f - deltaAbs) / alignSmoothRange;
        }

        orbitAngles.y = Mathf.MoveTowardsAngle(orbitAngles.y, headingAngle, rotationChange);

        return true;
    }

    static float GetAngle(Vector2 direction)
    {
        float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        return direction.x < 0f ? 360f - angle : angle;
    }

    private bool ManualRotation()
    {
        if (lockedOn) return false;
        Vector2 input = new Vector2(
            Input.GetAxis("Vertical Camera"),
            Input.GetAxis("Horizontal Camera")
        );
        
        const float e = 0.001f;
        if (input.x < -e || input.x > e || input.y < -e || input.y > e)
        {
            orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
            lastManualRotationTime = Time.unscaledTime;
            return true;
        }

        return false;
    }
}
