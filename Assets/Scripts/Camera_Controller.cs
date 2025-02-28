using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Camera_Controller : MonoBehaviour
{
    [SerializeField] Transform target;//要跟随的目标
    [SerializeField] Vector3 offset;//和目标的偏移量
    [SerializeField] float transitionSpeed = 2;//过渡的速度


    private void LateUpdate()
    {
        if (target !=null)
        {
            Vector3 targetPos= target.position+offset;
            //从当前坐标平滑过渡到目标点
            transform.position = Vector3.Lerp(transform.position, targetPos, transitionSpeed * Time.deltaTime); ;
        }
    }
}
