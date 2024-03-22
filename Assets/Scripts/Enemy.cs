using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// エネミー制御
/// </summary>
public class Enemy : BaseCharacter
{
    // ステータス
    public enum Status
    {
        Init = 0,
        Alive,
        Dead
    }
    private Status status = Status.Init;
    public Status CurStatus { get => status; }

    // 歩きパラメータ
    private const float MinInterval = 1.0f;
    private const float MaxInterval = 5.0f;
    private const float WalkSpeed = 0.5f;

    // 歩き情報
    private struct Walk
    {
        public float interval;
        public float timer;
        public Vector2 course;
        public Player player;
    }
    private Walk walkInfo;

    // ノックバック情報
    private struct NockBack
    {
        public bool isNockBack;
        public Vector2 dir;
        public float speed;
        public float timer;
    }
    private NockBack nockBack;

    /// <summary>
    /// フレームワーク
    /// </summary>
    private void Update()
    {
        switch(status)
        {
            case Status.Init:
                break;
            case Status.Alive:
                walk();
                break;
            case Status.Dead:
                // 死亡アニメーション中
                if(nockBack.isNockBack)
                {
                    nockBack.timer -= Time.deltaTime;
                    if(nockBack.timer < 0) nockBack.isNockBack = false;
                    this.transform.localPosition += new Vector3(nockBack.dir.x * nockBack.speed * Time.deltaTime, nockBack.dir.y * nockBack.speed * Time.deltaTime, 0);
                }
                break;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(Player player)
    {
        // 歩き情報を設定
        walkInfo.interval = Random.Range(MinInterval, MaxInterval);
        walkInfo.timer = walkInfo.interval;
        walkInfo.player = player;
        // プレイヤー位置に向かって歩く
        setWalkDirection();

        status = Status.Alive;        
    }

    /// <summary>
    /// 歩く
    /// </summary>
    private void walk()
    {
        // 歩く
        this.transform.localPosition += new Vector3(walkInfo.course.x * WalkSpeed * Time.deltaTime, walkInfo.course.y * WalkSpeed * Time.deltaTime, 0);
        // タイマー更新
        walkInfo.timer -= Time.deltaTime;
        // タイマーが０以下になったら再設定
        if(walkInfo.timer < 0)
        {
            walkInfo.timer = walkInfo.interval;
            setWalkDirection();
        }
    }

    /// <summary>
    /// 歩く方向を決定する
    /// </summary>
    private void setWalkDirection()
    {
        walkInfo.course = (walkInfo.player.Position - this.transform.position).normalized;
        SetDirection(walkInfo.course);
    }

    /// <summary>
    /// コライダーが当たったら最初に呼ばれる
    /// </summary>
    /// <param name="collision">相手側の情報が格納される</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 死んでいる場合は無視
        if (status == Status.Init || status == Status.Dead) return;

        // Weapon に当たったら消滅
        if (collision.tag == "Weapon")
        {
            // Debug.Log("Enemy Hit : " + collision.name);

            // アニメーションの status を変更
            status = Status.Dead;
            animator.SetInteger("status", (int)status);

            // 武器と反対方向へノックバック
            Vector2 dir = (this.transform.position - collision.transform.position).normalized;
            setNockBack(dir);
        }
    }

    /// <summary>
    // ノックバック情報を設定する
    /// </summary>
    /// <param name="dir">方向</param>
    private void setNockBack(Vector2 dir)
    {
        nockBack.isNockBack = true;
        nockBack.dir = dir;
        nockBack.timer = 0.2f;
        nockBack.speed = 0.5f / nockBack.timer;
    }


    // アニメーション終了時に呼ばれる
    public void OnAnimationEnd()
    {
        // 死亡アニメーションが終わったら消滅
        if (status == Status.Dead) Destroy(this.gameObject);
    }

    // 死亡（GameObject 破棄）時に呼ばれるイベント
    public UnityEvent OnDead = new UnityEvent();
    private void OnDestroy()
    {
        OnDead.Invoke();
    }

}
