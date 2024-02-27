using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミー制御
/// </summary>
public class Enemy : MonoBehaviour
{
    // アニメーション管理
    private Animator animator;

    // ステータス
    public enum Status
    {
        Alive = 0,
        Dead
    }
    private Status status = Status.Alive;
    public Status CurStatus { get => status; }

    /// <summary>
    /// 起動時処理（初期化）
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    /// <summary>
    /// コライダーが当たったら最初に呼ばれる
    /// </summary>
    /// <param name="collision">相手側の情報が格納される</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 死んでいる場合は無視
        if (status == Status.Dead) return;

        // Weapon に当たったら消滅
        if (collision.tag == "Weapon")
        {
            Debug.Log("Enemy Hit : " + collision.name);

            // アニメーションの status を変更
            status = Status.Dead;
            animator.SetInteger("status", (int)status);
        }
    }

    // アニメーション終了時に呼ばれる
    public void OnAnimationEnd()
    {
        // 死亡アニメーションが終わったら消滅
        if (status == Status.Dead) Destroy(this.gameObject);
    }


}
