using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクター基底クラス（プレイヤー、エネミーの基底クラス）
/// </summary>
public class BaseCharacter : MonoBehaviour
{
    // アニメーション管理
    protected Animator animator;
    // 向き
    protected Vector2 direction = new Vector2(1, 0);

    // 起動時処理(初期化)
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 位置
    /// </summary>
    /// <value>座標</value>
    public Vector3 Position { get => this.transform.localPosition; }

    /// <summary>
    /// 向き
    /// </summary>
    /// <value>向き</value>
    public Vector2 Direction { get => direction; }

    /// <summary>
    /// dir に応じてオブジェクトの向きを設定
    /// </summary>
    /// <param name="dir">向き</param>
    protected void setDirection(Vector2 dir)
    {
        if(dir.x > 0) this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        if(dir.x < 0) this.transform.localRotation = Quaternion.Euler(0, 180, 0);
        direction = dir;
    }

}
