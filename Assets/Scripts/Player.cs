using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static FieldController;

/// <summary>
/// プレイヤ制御
/// </summary>
public class Player : MonoBehaviour
{
    // アニメーション管理
    private Animator animator;

    // ステータス
    private enum Status
    {
        Standby = 0,
        Walk,
        Dead
    }
    private Status status = Status.Standby;
    private bool isWalking = false;

    // 移動速度の係数
    private readonly float moveSpeed = 0.005f;
    private Vector2 direction = new Vector2(1, 0);

    // 起動時処理(初期化)
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // フレームワーク
    private void Update()
    {
        // 移動中でない場合はアニメーションを Standby に戻す
        if(!isWalking && status == Status.Walk)
        {
            // Debug.Log("Player Standby");
            status = Status.Standby;
            animator.SetInteger("status", (int)status);
        }
        isWalking = false;
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="dir">方向（単位ベクトル）</param>
    public void Move(Vector2 dir)
    {
        // アニメーションの切り替え
        if(status == Status.Standby)
        {
            // Debug.Log("Player Moving");
            status = Status.Walk;
            animator.SetInteger("status", (int)status);
        }

        // 移動中フラグ
        isWalking = true;

        // 移動
        Vector2 val = dir * moveSpeed;
        Vector3 pos = this.transform.localPosition + new Vector3(val.x, val.y, 0);

        // 移動範囲制限（FieldController で生成したフィールドの範囲）により、pos を制限
        if(pos.x > fieldLimit.x) pos.x  = fieldLimit.x;
        if(pos.x < -fieldLimit.x) pos.x = -fieldLimit.x;
        if(pos.y > fieldLimit.y) pos.y  = fieldLimit.y;
        if(pos.y < -fieldLimit.y) pos.y = -fieldLimit.y;
        this.transform.localPosition    = pos;

        // 移動する方向に応じて向きを変える
        if(dir.x > 0) this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        if(dir.x < 0) this.transform.localRotation = Quaternion.Euler(0, 180, 0);

        direction = dir;
    }

    /// <summary>
    /// プレイヤー位置
    /// </summary>
    /// <value>座標</value>
    public Vector3 Position { get => this.transform.localPosition; }

    /// <summary>
    /// プレイヤーの向き
    /// </summary>
    /// <value>向き</value>
    public Vector2 Direction { get => direction; }


    /// <summary>
    /// コライダーが当たったら最初に呼ばれる
    /// </summary>
    /// <param name="collision">相手側の情報が格納される</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy に当たったらダメージ
        if (collision.tag == "Enemy")
        {
            Debug.Log("Player Hit : " + collision.name);
        }        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //コライダーが当たっていると継続して呼ばれる
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //コライダーがが離れた時に呼ばれる
    }
 
}
