using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾丸
/// </summary>
public class Bullet : MonoBehaviour
{
    // 寿命
    private readonly float lifeTime = 0.5f;
    private float timer = 0;
    // 方向
    private Vector2 direction;
    // 秒間の移動量
    private float moveSpeed = 0.1f;
    // 初期化済み
    private bool initialized = false;

    // 初期化（初期座標と方向を受け取る）
    public void Initialize(Vector3 pos, Vector2 dir, float range)
    {
        this.transform.localPosition = pos;
        direction = dir;
        moveSpeed = range / lifeTime;
        timer = lifeTime;
        initialized = true;
    }

    // フレームワーク
    private void Update()
    {
        if (!initialized) return;

        // 移動
        Vector2 val = direction * (moveSpeed * Time.deltaTime);
        Vector3 pos = this.transform.localPosition + new Vector3(val.x, val.y, 0);
        this.transform.localPosition = pos;

        // 寿命
        timer -= Time.deltaTime;
        if (timer < 0) Destroy(this.gameObject);
    }

    /// <summary>
    /// コライダーが当たったら最初に呼ばれる
    /// </summary>
    /// <param name="collision">相手側の情報が格納される</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy に当たったら消滅
        if (collision.tag == "Enemy")
        {
            // Enemy が生きていたら消滅
            if (collision.GetComponent<Enemy>().CurStatus == Enemy.Status.Alive)
            {
                Destroy(this.gameObject);
                Debug.Log("Bullet Hit + " + collision.name);
            }
        }
    }

}
