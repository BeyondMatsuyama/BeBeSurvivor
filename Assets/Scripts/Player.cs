using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static FieldController;

/// <summary>
/// プレイヤ制御
/// </summary>
public class Player : BaseCharacter
{
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
    private readonly float moveSpeed = 2.5f;

    // HP 制御
    [SerializeField] private HpGauge hpGauge;
    private const int HpInit   = 100;  // 初期 HP
    private const int HPDamage =   2;   // ダメージ量

    // 武器１オブジェクト（デフォルト武器）
    [SerializeField] private GameObject weapon_1;

    // 初期化
    new void Start() {
        base.Start();
        // HP ゲージ初期化
        hpGauge.Init(HpInit);
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
        Vector2 val = dir * (moveSpeed * Time.deltaTime);
        Vector3 pos = this.transform.localPosition + new Vector3(val.x, val.y, 0);

        // 移動範囲制限（FieldController で生成したフィールドの範囲）により、pos を制限
        if(pos.x > fieldLimit.x) pos.x  = fieldLimit.x;
        if(pos.x < -fieldLimit.x) pos.x = -fieldLimit.x;
        if(pos.y > fieldLimit.y) pos.y  = fieldLimit.y;
        if(pos.y < -fieldLimit.y) pos.y = -fieldLimit.y;
        this.transform.localPosition    = pos;

        // HpGauge の位置をプレイヤーに追従
        hpGauge.SetPosition(pos);

        // 移動する方向に応じて向きを変える
        SetDirection(dir);
    }

    /// <summary>
    /// コライダーが当たったら最初に呼ばれる
    /// </summary>
    /// <param name="collision">相手側の情報が格納される</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy に当たったらダメージ
        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if(enemy.CurStatus != Enemy.Status.Dead)    // 死んでいない場合
            {
                // Debug.Log("Player Hit : " + collision.name);
                if(hpGauge.Hit(HPDamage))
                {
                    // 死亡処理
                    Debug.Log("Player Dead");                    
                    status = Status.Dead;
                    animator.SetInteger("status", (int)status);
                    // 武器１を非表示
                    weapon_1.SetActive(false);
                }
            }   
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
 
    public bool IsDead()
    {
        return status == Status.Dead;
    }

}
