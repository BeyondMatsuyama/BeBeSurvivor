using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー管理
/// </summary>
public class PlayerController : MonoBehaviour
{
    // プレイヤー
    [SerializeField] private Player player;
    // プレイヤーの取得
    public Player GetPlayer() { return player; }
    
    // 武器管理
    [SerializeField] private WeaponController weaponController;

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="dir">方向（ベクトル）</param>
    public void Move(Vector2 dir)
    {
        // プレイヤー移動
        player.Move(dir);
    }

    /// <summary>
    /// プレイヤー位置
    /// </summary>
    /// <returns>座標値</returns>
    public Vector3 GetPosition() 
    {
        return player.Position;
    }

    /// <summary>
    /// プレイやー向き
    /// </summary>
    /// <returns>向き（ベクトル）</returns>
    public Vector2 GetDirection()
    {
        return player.Direction;
    }

    /// <summary>
    /// プレイヤーの向きを設定
    /// </summary>
    /// <param name="dir">向き</param>
    public void SetDirection(Vector2 dir)
    {
        player.SetDirection(dir);
    }

}
