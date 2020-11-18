using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂载在每个对象上，随机移动，碰撞检测
/// </summary>
public class MenuBtnRandomMove : MonoBehaviour
{
    // 运动速度
    public float speed = 1;
    // 运动方向向量
    private Vector2 direction;

    private void Awake()
    {
        direction = RandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(((direction * speed) / 20) * Time.deltaTime);     
    }

    // 碰撞检测，碰撞后向相反方向移动，并随机几秒改变方向
    public void OnCollisionEnter2D(Collision2D collision)
    {
        direction = new Vector2(-direction.x, -direction.y);
        StartCoroutine(RandomChangeDirection());
    }

    // 用协程隔几秒随机变方向
    public IEnumerator RandomChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        direction = RandomDirection();
    }

    // 生成随机运动方向
    public Vector2 RandomDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }
}
