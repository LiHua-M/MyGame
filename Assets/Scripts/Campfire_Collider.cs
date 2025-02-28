using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 篝火管理器
/// </summary>
public class Campfire_Collider : MonoBehaviour
{
    [SerializeField] new Light light;
    private float time = 30;//最大的燃烧时间
    private float currentTime = 30;//当前剩余的燃烧时间

    private void Update()
    {
        if (currentTime<=0)
        {
            currentTime = 0;
            light.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            currentTime -= Time.deltaTime;
            light.intensity = Mathf.Clamp(currentTime / time, 0, 1) * 10f;
        }

 
    }
    //添加木材
    public void AddWood()
    {
        currentTime += 10;
        light.transform.parent.gameObject.SetActive(true);
    }
}
