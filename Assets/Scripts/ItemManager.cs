using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    None,
    Meat,
    CookedMeat,
    Wood,
    Campfire
}

/// <summary>
/// 物品定义
/// </summary>
public class ItemDefine
{
    public ItemType ItemType;
    public Sprite Icon;
    public GameObject Prefab;

    public ItemDefine(ItemType itemType,Sprite icon,GameObject prefab)
    {
        ItemType = itemType;
        Icon = icon;
        Prefab = prefab;

    }
}
/// <summary>
/// 物品管理器
/// </summary>
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    [SerializeField] Sprite[] icons;
    [SerializeField] GameObject[] itemPrefabs;
    private void Awake()
    {
        Instance = this;
    }

    //获取物品定义
    public ItemDefine GetItemDefine(ItemType itemType)
    {
        return new ItemDefine(itemType, icons[(int)itemType-1],itemPrefabs[(int)itemType-1]);
    }
}
