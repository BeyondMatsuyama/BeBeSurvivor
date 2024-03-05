using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// すき（農具）
/// 実装にあたり Hatena Blog 様の記事を参考にしました
/// https://robamemo.hatenablog.com/entry/2017/09/01/114748
/// </summary>
public class Plow : MonoBehaviour
{
    void Start()
    {
        Initialize(
            new Vector3(0f, 0, 0),
            new Vector3(1f, 30, 0),
            new Vector3(2f, -10, 0)
        );
    }

    // 生存フラグ
    private bool isAlive = true;
    // ヒット数
    private int hitCount = 3;

    /// <summary>
    ///  初期化
    /// </summary>
    /// <param name="p0">始点</param>
    /// <param name="p1">頂点</param>
    /// <param name="p2">終点</param>
    public void Initialize(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        StartCoroutine(Throw(p0, p1, p2));
    }

    /// <summary>
    /// 放物線を描く
    /// </summary>
    /// <param name="p0">始点</param>
    /// <param name="p1">頂点</param>
    /// <param name="p2">終点</param>
    /// <returns>コルーチン</returns>
    IEnumerator Throw (Vector3 p0, Vector3 p1, Vector3 p2)
    {
       float distance = Vector3.Distance(p0, p2);
       float speed = 0.3f;

       float t = 0f;
        while (t <= 1 && isAlive) {
            float Vx = 2 * (1f - t) * t * p1.x + Mathf.Pow (t, 2) * p2.x + p0.x;
            float Vy = 2 * (1f - t) * t * p1.y + Mathf.Pow (t, 2) * p2.y + p0.y;
            transform.position = new Vector3 (Vx, Vy, 0);

            t += 1 / distance / speed * Time.deltaTime;

            yield return null;
        }

        Destroy (this.gameObject);
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
            // ヒット数を減らし、ヒット数が０になったら生存フラグを降ろす
            hitCount--;
            if (hitCount <= 0) isAlive = false;
        }
    }

}
