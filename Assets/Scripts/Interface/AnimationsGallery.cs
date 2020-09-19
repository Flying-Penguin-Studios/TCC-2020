using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsGallery : MonoBehaviour
{
    public Animator test;


    // Start is called before the first frame update
    void Start()
    {
        test = GetComponent<Animator>();
        GetComponent<Animator>().fireEvents = false;
        GetAnimations(test);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAnimations(Animator anim)
    {
        anim = GetComponent<Animator>();

        AnimationClip[] animationClips = anim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip animClip in animationClips)
        {
            Debug.Log(animClip.name + ": " + animClip.length);
        }
    }

    public void ChangeParemeter(string paramName)
    {
        test.SetBool(paramName, !test.GetBool(paramName));
    }

    public void BackToIdle()
    {
        foreach(AnimatorControllerParameter param in test.parameters)
        {
            test.SetBool(param.name, false);
        }
    }
}
