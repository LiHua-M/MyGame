using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    [SerializeField] float hp;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClip ;
    public GameObject lootObject;//掉落的物品

    public float Hp 
    { 
        get => hp;
        set{ 
            hp = value;
            if(hp<=0)
            {
                hp = 0;
                Dead();
            }
            OnHpUpdate();
        }
    }

    public  void PlayAudio(int index)
    {
        audioSource.PlayOneShot(audioClip[index]);
    }
    protected virtual void OnHpUpdate() { }
    protected virtual void Dead()
    {
        if(lootObject!=null)
        {
            Instantiate(lootObject, 
                transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(1f, 1.5f), Random.Range(-0.5f, 2f)), 
                Quaternion.identity, 
                null);
        }
    }

    //受伤
    public virtual void Hurt(int damage)
    {
        Hp -= damage;
    }
    /// <summary>
    /// 添加物品
    /// </summary>
    /// <returns></returns>
    public virtual bool AddItem(ItemType itemType)
    {
        return false;
    }
}
