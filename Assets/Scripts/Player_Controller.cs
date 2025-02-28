using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Player_Controller : ObjectBase
{
    public static Player_Controller Instance;

    [SerializeField] Animator animatior;
    [SerializeField] CheckCollider checkCollider;
    [SerializeField] CharacterController characterController;
    float RotationSpeed = 10f;
    private  Quaternion targetDirQuaternion;
    [SerializeField] float moveSpeed=1;
    private bool isAttacking = false;
    private bool isHurting=false;
    private float hungry = 100f;
    public bool isDarging = false;//当前在拖拽中
    //当机饿值修改时，自动更新UI以及HP
    public float Hungry { get => hungry;
        set {
            hungry = value;
            if (hungry <= 0)
            {
                hungry = 0;
                //衰减hp
                Hp -= Time.deltaTime / 2;
            }
            //更新饥饿值图片
            hungryImage.fillAmount = hungry / 100;
        }
    }

    [SerializeField] Image hpImage;
    [SerializeField] Image hungryImage;

    private void Awake()
    {
        Instance = this;
        //初始化检测器
        checkCollider.Init(this,30);
    }

    private void  Update()
    {
        UpdateHungry(); 
        //玩家即不在攻击和受伤才能移动
        if (!isAttacking)
        {
            Move();
            Attack();
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation,targetDirQuaternion,Time.deltaTime*10);  
        }
        
    }

    //更新饥饿值
    private void UpdateHungry()
    {
        //衰减饥饿值
        Hungry-= Time.deltaTime*3;
        
    }

    public override void Hurt(int damage)
    {
        base.Hurt(damage);
        animatior.SetTrigger("Hurt");
        PlayAudio(2);
        isHurting = true;
    }
    //移动
    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if(h==0&&v==0)
        {
            animatior.SetBool("Walk", false);
        }
        else
        {
            animatior.SetBool("Walk", true);
            //获取一个最终的目标方相并过渡过去
            targetDirQuaternion = Quaternion.LookRotation(new Vector3(h, 0, v));
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetDirQuaternion, Time.deltaTime * RotationSpeed);
            //处理实际的移动
            characterController.SimpleMove(new Vector3(h, 0, v).normalized*moveSpeed);
        }
    }
    //攻击
    private void Attack()
    {
        if (Input.GetMouseButton(0))
        {
            //当前鼠标在拖拽或者正在交互UI
            if (isDarging|| UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hitInfo,100,LayerMask.GetMask("Ground")))
            {
                animatior.SetTrigger("Attack");
                isAttacking = true;
                targetDirQuaternion= Quaternion.LookRotation(hitInfo.point-transform.position);
            }
        }
    }
    //更新血量
    protected override void OnHpUpdate()
    {
        hpImage.fillAmount = Hp/100;
    }

    public override bool AddItem(ItemType itemType)
    {

        //检测背包能不能装下
        return UI_Bag.Instance.AddItem(itemType);
    }

    /// <summary>
    /// 使用物品
    /// </summary>
    public bool UseItem(ItemType itemType)
    {
        switch (itemType)
        {
            
            case ItemType.Meat:
                Hp += 10;
                Hungry += 20;
                return true;
            case ItemType.CookedMeat:
                Hp += 10;
                Hungry += 40;
                return true;
            case ItemType.Wood:
                Hp -= 5;
                Hungry += 10;
                return true;

        }
        return false;
    }



    #region 动画事件
    private void StartHit()
    {
        //播放音效
        PlayAudio(0);
        //攻击检测
        checkCollider.StartHit();
    }
    private void StopHit()
    {
        //停止攻击检测
        isAttacking=false;
        checkCollider.StopHit();
    }

    public void HurtOver()
    {
        isHurting=false;
    }
    #endregion


}
