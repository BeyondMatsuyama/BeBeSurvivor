using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器３・鎌（農具）
/// </summary>
public class Weapon_3 : WeaponBase
{
    private enum Mode { None, Active, Wait };
    private Mode mode = Mode.None;

    // 同時出現数（３レベル）
    private readonly int[] simultaneous = { 1, 2, 3 };
    private readonly float[] relativeAngles = { 0.0f, 180.0f, 120.0f };

    // 発生時間と待機時間
    private readonly int ActiveTime = 10;
    private readonly int WaitTime = 5;
    private float curTime = 0.0f;

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
                // 非アクティブならアクティブにする
                if(!sickle.activeSelf)
                {
                    sickle.SetActive(true);
                }
            }
            // 鎌を生成
            else
            {
                sickle = Instantiate(prefab, parent);
                sickles.Add(sickle);
                sickle.name = "Sickle_" + i;      
            }
            // 初期化
            rot = relativeAngles[GetLevel()] * i;
            sickle.GetComponent<Sickle>().Initialize(player, rot);

            SoundManager.Instance.PlaySE(SoundManager.SE.Melee0);
        }

        // 生成済みの鎌の数が生成する鎌の数より多い場合は、削除
        for(int i=existNum-1 ; i>=num ; i--)
        {
            Destroy(sickles[i]);
            sickles.RemoveAt(i);
        }

        // アクティブから再生
        mode    = Mode.Wait;
        curTime = WaitTime;
    }

    private void Update()
    {
        if(!isActive)
        {
            mode = Mode.None;
            return;
        }
        if(mode == Mode.None) return;
        if(GameController.isPause) return;

        curTime += Time.deltaTime;
        float time = (mode == Mode.Active) ? ActiveTime : WaitTime;
        if(curTime >= time)
        {
            if(mode == Mode.Active)
            {
                toWait();
                mode = Mode.Wait;
            }
            else
            {
                toActive();
                mode = Mode.Active;
            }
            curTime = 0.0f;
        }

    }

    /// <summary>
    /// アクティブ時間
    /// </summary>
    private void toActive()
    {
        for(int i=0 ; i<sickles.Count ; i++)
        {
            sickles[i].SetActive(true);
        }
    }

    /// <summary>
    /// 使用不可時間
    /// </summary>
    private void toWait()
    {
        for(int i=0 ; i<sickles.Count ; i++)
        {
            sickles[i].SetActive(false);
        }
    }
}
