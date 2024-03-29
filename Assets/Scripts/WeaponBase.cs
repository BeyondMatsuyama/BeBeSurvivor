using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器基底クラス
/// </summary>
public class WeaponBase : MonoBehaviour
{
    // プレイヤー制御
    [SerializeField] protected PlayerController playerController;
    // エネミー制御
    [SerializeField] protected EnemyController enemyController;

    // 武器のプレハブ
    [SerializeField] protected GameObject prefab;
    // オブジェクトを配置する親オブジェクト
    [SerializeField] protected Transform parent;

    // レベル
    private int level = 0;
    // レベル取得
    public int GetLevel() { return level; }
    // レベルアップ
    public bool LevelUp()
    {
        // 非アクティブの場合はアクティブする
        if(!isActive)
        {
            Activate();
            return true;
        }

        // レベルアップ（レベルアップした場合はtrueを返す）
        if(level < WeaponController.maxLevel - 1)
        {
            level++;
            return true;
        }
        return false;
    }
    // レベルダウン
    public void LevelDown()
    {
        // レベルダウン
        level = Mathf.Max(level - 1, 0);
    }

    // インターバルタイマー
    protected float intervalTimer = 0;

    // アクティブフラグ
    protected bool isActive = false;
    // アクティブであるか
    public bool IsActive() { return isActive; }
    // アクティブ化
    public void Activate() { isActive = true; }

    /// <summary>
    /// レベルに応じたポイントの取得（表示用）
    /// </summary>
    /// <returns>レベルポイント</returns>
    public int GetLevelPoint()
    {
        if(!isActive) return 0;
        else return level + 1;
    }


}
