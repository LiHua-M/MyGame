﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 时间管理器
/// </summary>
public class TimeManager : MonoBehaviour
{
    [SerializeField] Light sunLight;
    public static TimeManager Instance;

    public float dayTime;//白天的时间
    public float dayToNightTime;//白到黑的时间
    public float nightTime;//夜晚的时间
    public float nightToDayTime;//夜晚到白天的时间

    private float lightValue = 1;
    private int dayNum=0;

    [SerializeField] Image timeStateImg;
    [SerializeField] Text dayNumText;
    [SerializeField] Sprite[] dayStateSprites;//白天和黑夜的图片

    private bool isDay = true;

    public bool IsDay { get => isDay;
        set {
            isDay = value;
            if (isDay)
            {
                dayNum += 1;
                dayNumText.text = "Day " + dayNum;
                timeStateImg.sprite = dayStateSprites[0];
            }
            else
            {
                timeStateImg.sprite = dayStateSprites[1];
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        IsDay = true;
        //计算时间
        StartCoroutine(UpdateTime());
    }

    private IEnumerator UpdateTime()
    {
        while(true)
        {
            yield return null;
            //当前是白天
            if (IsDay)
            {
                lightValue -= 1 / dayToNightTime * Time.deltaTime;
                SetLightValue(lightValue);
                if(lightValue<=0)
                {
                    IsDay = false;
                    yield return new WaitForSeconds(nightTime);//等待夜晚过去
                }
            }
            //当前是夜晚
            else
            {
                lightValue += 1 / nightToDayTime * Time.deltaTime;
                SetLightValue(lightValue);
                if (lightValue >=1)
                {
                    IsDay = true;
                    yield return new WaitForSeconds(dayTime);//等待白天过去
                }
            }
        }
    }
    /// <summary>
    /// 设置灯光的值
    /// </summary>
    /// <param name="value"></param>
    private void SetLightValue(float value)
    {
        RenderSettings.ambientIntensity = value;
        sunLight.intensity = value;
    }
}
