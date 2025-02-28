using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// UI背包
/// </summary>
public class UI_Bag : MonoBehaviour
{
    public static UI_Bag Instance;

    private UI_BagItem[] items;
    [SerializeField] GameObject itemPrefab;
    private void Awake()
    {
        Instance = this;
       
    }

    private void Start()
    {
        items = new UI_BagItem[5];
        //先生成篝火
        UI_BagItem item = Instantiate(itemPrefab, transform).GetComponent<UI_BagItem>();
        item.Init(ItemManager.Instance.GetItemDefine(ItemType.Campfire));
        items[0] = item;

        for (int i = 1; i < 5; i++)
        {
            item = Instantiate(itemPrefab, transform).GetComponent<UI_BagItem>();
            item.Init(null);
            items[i] = item;
        }
    }
    //添加物品
    public bool AddItem(ItemType itemType)
    {
        //查看有没有空格子
        for (int i=0;i<items.Length;i++)
        {
            if(items[i].itemDefine==null)//空格子
            {
                ItemDefine itemDefine = ItemManager.Instance.GetItemDefine(itemType);
                items[i].Init(itemDefine);
                return true;
            }
        }
        return false;
    }
}
