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
    public void LevelUp()
    {
        // レベルアップ
        level = Mathf.Min(level + 1, WeaponController.maxLevel - 1);
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


}
