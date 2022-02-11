using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;
    
    public Animator PlayerAnimator => playerAnimator;

    private static AnimationManager instance;
    
    public float attackAnimLength;
    public float dashAnimLength;
    public float idleAnimLength;
    public static AnimationManager Instance => instance;

    public delegate void AnimationEnd();
    public static event AnimationEnd OnAnimationEnd;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        
        GetAnimationLengths();
    }

    private void GetAnimationLengths()
    {
        AnimationClip[] clips = playerAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip animationClip in clips)
        {
            switch (animationClip.name)
            {
                case "attack":
                    attackAnimLength = animationClip.length;
                    break;
                case "dash":
                    dashAnimLength = animationClip.length;
                    break;
                case "idle":
                    idleAnimLength = animationClip.length;
                    break;
                default:
                    break;
            }
        }
    }

    // public void PlayAttackAnimation()
    // {
    //     if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack") &&
    //         !(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1))
    //         playerAnimator.Play("attack");
    // }
    
    
    

    // Update is called once per frame
    void Update()
    {
        // if ((isPlaying && 
        //      playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack") && 
        //      playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) 
        //     || !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack") )
        // {
        //     isPlaying = false;
        //     OnAnimationEnd?.Invoke();
        // }
    }

    public void PlayDashAnimation()
    {
        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("dash") && !(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1))
            playerAnimator.Play("dash");
    }
}
