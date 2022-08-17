using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : MonoBehaviour, IOnChangeSkin
{
    [SerializeField]
    Animator anim;
    Animator Anim
    {
        get
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }
            return anim;
        }
    }

    public void OnChangeSkin(RuntimeAnimatorController controller)
    {
        Anim.runtimeAnimatorController = controller;
    }
}
