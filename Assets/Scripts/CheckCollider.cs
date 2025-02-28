using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//专门用来检测的   碰撞物体

public class CheckCollider : MonoBehaviour
{
    private ObjectBase owner;
    private  int damage;
    private bool canHit=false;

    [SerializeField] List<string> enemyTags=new List<string>();
    [SerializeField] List<string> ItemTags = new List<string>();
    public void Init(ObjectBase owner,int damage)
    {
        this.owner = owner;
        this.damage = damage;
    }


    //开启伤害检测
    public void StartHit()
    {
        canHit = true;
    }
    //关闭伤害检测

    public void StopHit()
    {
        canHit=false;
        lastAttackObjectList.Clear();
    }

    private List<GameObject> lastAttackObjectList = new List<GameObject>();
    private void OnTriggerStay(Collider other)
    {
        //如果当前伤害允许伤害检测
        if(canHit)
        {
            //此次伤害还没有检测到这个单位&&敌人的标签包含在敌人列表中
            if(!lastAttackObjectList.Contains(other.gameObject)&&enemyTags.Contains(other.tag))
            {

                lastAttackObjectList.Add(other.gameObject);
                //具体的伤害逻辑
                other.GetComponent<ObjectBase>().Hurt(damage);

            }
            return;
        }
        //检测拾取
        if(ItemTags.Contains(other.tag))
        {

            //检到物品的tag转枚举

           // ItemType itemType = Enum.Parse<ItemType>(other.tag);

            // 使用非泛型版本的 Enum.Parse 方法
            object parsedValue = Enum.Parse(typeof(ItemType), other.tag);
            ItemType itemType = (ItemType)parsedValue;
            if (owner.AddItem(itemType))
            {
                owner.PlayAudio(1);//告诉宿主播放音效
                Destroy(other.gameObject);//销毁检到的物品
            }
        }
    }
}
