using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器３・鎌（農具）
/// </summary>
public class Weapon_3 : WeaponBase
{
    // 同時出現数（３レベル）
    private readonly int[] simultaneous = { 1, 2, 3 };
    private readonly float[] relativeAngles = { 0.0f, 180.0f, 120.0f };
    private readonly int[] hitMaxes = { 5, 15, 30 };
    private int curHit = 0;
    private readonly int waitTime = 5;

    // 生成済みの鎌
    private List<GameObject> sickles = new List<GameObject>();

    /// <summary>
    /// 生成と初期配置
    /// </summary>
    /// <param name="player">プレイヤー</param>
    public void CreateAndInitialize(Player player)
    {
        float rot = 0.0f;
        // 生成済みの鎌の数を取得
        int existNum = sickles.Count;
        // 生成する鎌の数を取得
        int num = simultaneous[GetLevel()];

        for(int i=0 ; i<num ; i++)
        {
            GameObject sickle = null;
            // 生成済みの鎌がある場合は、List から取得
            if(i < existNum)
            {
                sickle = sickles[i];
            }
            // 鎌を生成
            else
            {
                sickle = Instantiate(prefab, parent);
                sickles.Add(sickle);
            }
            // 初期化
            sickle.GetComponent<Sickle>().Initialize(player, rot);
            rot += relativeAngles[GetLevel()];

            // 当たり判定を監視し、
            sickle.GetComponent<Sickle>().OnHit.AddListener(() => Hit(sickle));
        }

        // 生成済みの鎌の数が生成する鎌の数より多い場合は、削除
        for(int i=num ; i<existNum ; i++)
        {
            Destroy(sickles[i]);
            sickles.RemoveAt(i);
        }
    }

    /// <summary>
    /// 当たり判定が発生したら呼ばれる
    /// </summary>
    /// <param name="sickle"></param>
    private void Hit(GameObject sickle)
    {
        curHit++;
        if(curHit >= hitMaxes[GetLevel()])
        {
            // ヒット数をリセット
            curHit = 0;
            // 武器を非アクティブにする
            for(int i=0 ; i<sickles.Count ; i++)
            {
                sickles[i].SetActive(false);
            }

            // 待機時間後に再度アクティブにする
            StartCoroutine(WaitActive());
        }
    }

    /// <summary>
    /// 使用不可時間
    /// </summary>
    /// <returns>こルーチン</returns>
    private IEnumerator WaitActive()
    {
        yield return new WaitForSeconds(waitTime);

        for(int i=0 ; i<sickles.Count ; i++)
        {
            sickles[i].SetActive(true);
        }
    }
}
