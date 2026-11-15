using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BallMove : MonoBehaviour
{
    [Header("动态设置（可在面板修改）")]
    public float moveSpeed = 2.0f;
    public float waitTime = 1.0f;

    [Header("移动范围")]
    public float leftPos = -5.0f;
    public float rightPos = 5.0f;

    private float pingPongTime = 0f; // 用独立的时间变量逐帧累加
    private float timer = 0f;
    private bool isWaiting = false;

    void Update()
    {
        if (isWaiting)
        {
            timer -= Time.deltaTime;
            if (timer <= 0) isWaiting = false;
            return;
        }

        // 逐帧累加时间
        pingPongTime += Time.deltaTime * moveSpeed;

        // 计算 0~1 之间的来回值
        float pingPongValue = Mathf.PingPong(pingPongTime, 1f);

        // 使用 SmoothStep 实现两端快中间慢
        float smoothValue = Mathf.SmoothStep(0f, 1f, pingPongValue);

        // 映射到真实的坐标
        float currentX = Mathf.Lerp(leftPos, rightPos, smoothValue);
        transform.position = new Vector3(currentX, transform.position.y, transform.position.z);

        // 到达两端开始等待
        if (pingPongValue <= 0.01f || pingPongValue >= 0.99f)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                timer = waitTime;
            }
        }
    }
}