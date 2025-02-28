using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_BagItem : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField] Image bg;
    [SerializeField] Image iconImg;
    public  ItemDefine itemDefine;
    private bool isSelect = false;

    public bool IsSelect { get => isSelect;
        set {
            isSelect = value;
            if (isSelect)
            {
                bg.color = Color.green;
            }
            else
            {
                bg.color = Color.white;
            }
        }
    }

    private void Update()
    {
        if (IsSelect&&itemDefine!=null&&Input.GetMouseButtonDown(1))
        {
            if (Player_Controller.Instance.UseItem(itemDefine.ItemType))
            {
                Init(null);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsSelect = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsSelect = false;
    }

    public void Init(ItemDefine itemDefine=null)
    {
        this.itemDefine = itemDefine;
        IsSelect = false;
        if(this.itemDefine==null)
        {
            iconImg.gameObject.SetActive(false);
        }
        else
        {
            iconImg.gameObject.SetActive(true);
            iconImg.sprite = itemDefine.Icon;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDefine == null) return;
        Player_Controller.Instance.isDarging = true;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemDefine == null) return;
        iconImg.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemDefine == null) return;
        Player_Controller.Instance.isDarging = false;
        //发射射线查看当前碰到的物品
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hitInfo))
        {
            string targetTag = hitInfo.collider.tag;
            iconImg.transform.localPosition = Vector3.zero;//Icon归位

            //根据自身类型 和目标类型 做逻辑判断
            switch (itemDefine.ItemType)
            {
                case ItemType.Meat:
                    if(targetTag=="Ground")
                    {
                        Instantiate(itemDefine.Prefab,hitInfo.point+new Vector3(0,1.5f,0),Quaternion.identity);
                        Init(null);
                    }
                    else if (targetTag == "Campfire")
                    {
                        Init(ItemManager.Instance.GetItemDefine(ItemType.CookedMeat));
                    }
                    break;
                case ItemType.CookedMeat:
                    if (targetTag == "Ground")
                    {
                        Instantiate(itemDefine.Prefab, hitInfo.point + new Vector3(0, 1.5f, 0), Quaternion.identity);
                        Init(null);
                    }
                    else if (targetTag == "Campfire")
                    {
                        hitInfo.collider.GetComponent<Campfire_Collider>().AddWood();
                        Init(null);
                    }
                    break;
                case ItemType.Wood:
                    if (targetTag == "Ground")
                    {
                        Instantiate(itemDefine.Prefab, hitInfo.point + new Vector3(0, 1.5f, 0), Quaternion.identity);
                        Init(null);
                    }else if (targetTag=="Campfire")
                    {
                        hitInfo.collider.GetComponent<Campfire_Collider>().AddWood();
                        Init(null);
                    }
                    break;
                case ItemType.Campfire:
                    if (targetTag == "Ground")
                    {
                        Instantiate(itemDefine.Prefab, hitInfo.point, Quaternion.identity);
                        Init(null);
                    }
                    break;
               
            }
        }
    }
}
