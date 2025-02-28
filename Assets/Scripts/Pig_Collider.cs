using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

//敌人AI状态

public enum EnemyState
{
    Idle,//待机
    Move,//移动 
    Pursue,//追击
    Attack,//攻击
    Hurt,//受伤
    Die//死亡
}

public class Pig_Collider : ObjectBase
{
    //野猪AI
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] CheckCollider checkCollider;
    //行动范围
    public float maxX = 4.74f;
    public float minX = -5.62f;
    public float maxZ = 5.92f;
    public float minZ = -6.33f;


    private EnemyState enemyState;
    private Vector3 targetPos;
    //当状态修改时，进入新状态要做的事，相当于OnEnter

    public EnemyState EnemyState { get => enemyState;
        set {
            enemyState = value;
            switch (enemyState)
            {
                case EnemyState.Idle:
                    //播放动画
                    //关闭导航
                    //休息一段时间再巡逻
                    animator.CrossFadeInFixedTime("Idle",0.25f);
                    navMeshAgent.enabled = false;
                    Invoke(nameof(Gomove), Random.Range(3f, 10f));
                    break;
                case EnemyState.Move:
                    //播放动画
                    //开启导航
                    //获取巡逻点
                    //移动到指定位置
                    animator.CrossFadeInFixedTime("Move", 0.25f);
                    navMeshAgent.enabled = true;
                    targetPos = GetTargetPos();
                    navMeshAgent.SetDestination(targetPos);
                    break;
                case EnemyState.Pursue:
                    animator.CrossFadeInFixedTime("Move", 0.25f);
                    navMeshAgent.enabled = true;
                    break;
                case EnemyState.Attack:
                    animator.CrossFadeInFixedTime("Attack", 0.25f);
                    transform.LookAt(Player_Controller.Instance.transform.position);
                    navMeshAgent.enabled = false;
                    break;
                case EnemyState.Hurt:
                    animator.CrossFadeInFixedTime("Hurt", 0.25f);
                    PlayAudio(0);
                    navMeshAgent.enabled = false;
                    break;
                case EnemyState.Die:
                    animator.CrossFadeInFixedTime("Die", 0.25f);
                    PlayAudio(0);
                    navMeshAgent.enabled = false;
                    break;
            }
        } 
    }


    private void Start()
    {
        checkCollider.Init(this, 10);
        EnemyState=EnemyState.Idle;
    }
    private void Update()
    {
        StateOnUpdate();
    }

    private void StateOnUpdate()
    {
        switch (enemyState)
        {
            //case EnemyState.Idle:
                //break;
            case EnemyState.Move:
                if(Vector3.Distance(transform.position,targetPos)<1.5f)
                {
                    EnemyState = EnemyState.Idle;
                }
                break;
            case EnemyState.Pursue:
                //距离足够近，切换到攻击状态
                
                if (Vector3.Distance(transform.position, Player_Controller.Instance.transform.position) < 1)
                {
                    EnemyState = EnemyState.Attack;
                }
                else
                {
          
                    navMeshAgent.SetDestination(Player_Controller.Instance.transform.position);
                }
                break;
            case EnemyState.Attack:
                if (Player_Controller.Instance != null && navMeshAgent != null)
                {
                    
                    if (Vector3.Distance(transform.position, Player_Controller.Instance.transform.position) >1) 
                    {
                        EnemyState = EnemyState.Pursue;
                    }
                }
                break;
                /* case EnemyState.Attack:
                     break;
                 case EnemyState.Hurt:
                     break;
                 case EnemyState.Die:
                     break;*/
        }
    }
    private void Gomove()
    {
        EnemyState = EnemyState.Move;
    }

    //获取一个范围内的随机点
    private Vector3 GetTargetPos()
    {
        return new Vector3(Random.Range(maxX, minX), 0, Random.Range(maxZ, minZ));
    }

    public override void Hurt(int damage)
    {
        if (EnemyState == EnemyState.Die) return;
        CancelInvoke(nameof(Gomove));//取消切换到移动状态的延迟调用
        base.Hurt(damage);
        if(Hp>0)
        {
            EnemyState = EnemyState.Hurt;
        }
    }

    protected override void Dead()
    {
        base.Dead();
        EnemyState = EnemyState.Die;
       
    }


    #region 动画事件

    private void StartHit()
    {
        checkCollider.StartHit();
    }

    private void StopHit()
    {
        checkCollider.StopHit();
    }

    private void StopAttack()
    {
        if (EnemyState != EnemyState.Die) EnemyState = EnemyState.Pursue;

    }

    private void HurtOver()
    {
        if (EnemyState != EnemyState.Die) EnemyState = EnemyState.Pursue;
    }
   private void Die()
    {
        Destroy(gameObject);
    }
    #endregion


}
