using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//树
public class Tree_Collider : ObjectBase
{
    [SerializeField] Animator animator;

    private void Start()
    {
        Hp = 100;

    }

    public override void Hurt(int damage)
    {
        base.Hurt(damage);
        animator.SetTrigger("Hurt");
        PlayAudio(0);
    }

    protected override void Dead()
    {
        base.Dead();
        Destroy(gameObject);
    }
}
