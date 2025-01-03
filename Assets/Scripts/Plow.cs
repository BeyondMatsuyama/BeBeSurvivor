using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// すき（農具）
/// 実装にあたり Hatena Blog 様の記事を参考にさせていただきました
/// https://robamemo.hatenablog.com/entry/2017/09/01/114748
/// </summary>
public class Plow : MonoBehaviour
{
/*    
    void Start()
    {
        Initialize(
            new Vector3(0f, 0, 0),
            new Vector3(-1f, 25, 0),
            new Vector3(-2f, -10, 0)
        );
    }
*/
    // 生存フラグ
    private bool isAlive = true;
    // ヒット数
    private int hitCount = 5;

    // 左右向き
    private int course = 0; // 0:右、1:左
    private float[] courseGoal = { -180, 180 };


    /// <summary>
    ///  初期化
    /// </summary>
    /// <param name="p0">始点</param>
    /// <param name="p1">頂点</param>
    /// <param name="p2">終点</param>
    /// <param name="course">左右向き</param>
    public void Initialize(Vector3 p0, Vector3 p1, Vector3 p2, int course)
    {
        // 向き
        this.course = course;
        // 放物線を描く（コルーチン）
        StartCoroutine(Throw(p0, p1, p2));
    }

    /// <summary>
    /// 放物線を描く
    /// </summary>
    /// <param name="p0">始点</param>
    /// <param name="p1">頂点（相対座標）</param>
    /// <param name="p2">終点（相対座標）</param>
    /// <returns>コルーチン</returns>
    IEnumerator Throw (Vector3 p0, Vector3 p1, Vector3 p2)
    {
       float distance = Vector3.Distance(Vector3.zero, p2);
       float speed = 0.3f;

       float t = 0f;
        while (t <= 1 && isAlive) {
            // ポーズ中は無視
            if (!GameController.isPause)
            {
                float Vx = 2 * (1f - t) * t * p1.x + Mathf.Pow (t, 2) * p2.x + p0.x;
                float Vy = 2 * (1f - t) * t * p1.y + Mathf.Pow (t, 2) * p2.y + p0.y;
                transform.localPosition = new Vector3 (Vx, Vy, 0);

                t += 1 / distance / speed * Time.deltaTime;

                // t が 0.4 から 0.6 の間に、徐々に180°回転させる
                if (t > 0.4f && t < 0.6f)
                {
                    float angle = Mathf.Lerp(0, courseGoal[course], (t - 0.4f) / 0.2f);
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
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
            Enemy enemy = collision.GetComponent<Enemy>();
            // 対象のエネミーが生存
            if (enemy.CurStatus == Enemy.Status.Alive)
            {
                // デバッグログ（自オブジェクトとヒットしたオブジェクトの名称）
                // Debug.Log(this.name + " Hit " + collision.name);

                // ヒット数を減らし、ヒット数が０になったら生存フラグを降ろす
                hitCount--;
                if (hitCount <= 0) isAlive = false;
            }
        }
    }

}
