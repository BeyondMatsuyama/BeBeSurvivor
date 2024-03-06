using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器２・すき（農具）
/// </summary>
public class Weapon_2 : WeaponBase
{
    // 発射間隔
    private readonly float interval = 3.0f;
    // 同時発射数（３レベル）
    private readonly int[] simultaneous = { 1, 2, 3 };
    // 幅
    private readonly float minWidth = 2.0f;
    private readonly float[] maxWidth = { 2f, 4f, 6f };
    // 頂点の高さ
    private readonly float height = 25f;
    private readonly float rndHeight = 5f;
    // 終点の高さ
    private readonly float endHeight = -10f;

    /// <summary>
    /// フレームワーク
    /// </summary>
    void Update()
    {
        // アクティブでない場合は処理しない
        if (!isActive) return;

        // インターバルタイマー更新
        intervalTimer += Time.deltaTime;
        // インターバルチェック
        if (intervalTimer > interval)
        {
            // インターバルタイマー初期化
            intervalTimer = 0;

            // 弾発射
            StartCoroutine(fire());
        }
    }

    /// <summary>
    /// 発射
    /// </summary>
    /// <returns>コルーチン</returns>
    private IEnumerator fire()
    {
        int num = simultaneous[GetLevel()];
        while(num > 0)
        {
            // 発射数を減らす
            num--;

            // 始点（プレイヤー位置）
            Vector3 p0 = playerController.GetPosition();
            // 頂点
            float c = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;                     // 向き（左右）
            float w = UnityEngine.Random.Range(minWidth, maxWidth[GetLevel()]) * c;     // 幅
            float h = height + UnityEngine.Random.Range(0, rndHeight);                  // 高さ
            Vector3 p1 = new Vector3(p0.x + (w / 2f), p0.y + h, 0);
            // 終点
            Vector3 p2 = new Vector3(p0.x + w, p0.y + endHeight, 0);

            // 生成
            GameObject obj = Instantiate(prefab, parent);
            // obj.transform.Rotate(0, 0, 90); // 90°回転（絵の向きの都合）

            // 初期化
            obj.GetComponent<Plow>().Initialize(p0, p1, p2);

            // インターバル
            yield return new WaitForSeconds(0.8f);
        }
    }

}
