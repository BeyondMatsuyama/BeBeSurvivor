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
        Hit,
        Dead
    }
    private Status status = Status.Init;
    public Status CurStatus { get => status; }

    // タイプ
    public enum Type
    {
        Normal_1 = 0,
        Normal_2,
        Boss
    }

    // パラメータ
    [System.Serializable]
    private struct TypeParam
    {
        public Type  type;
        public int   hp;
        public float speed;
    }
    [SerializeField] private TypeParam param;

    // 歩きパラメータ
    private const float MinInterval = 1.0f;
    private const float MaxInterval = 5.0f;

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

    private float deadTime = 0.0f;

    private ExpController expController;

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
            case Status.Hit:
                // ノックバック中
                if(nockBack.isNockBack)
                {
                    nockBack.timer -= Time.deltaTime;
                    if(nockBack.timer < 0) nockBack.isNockBack = false;
                    this.transform.localPosition += new Vector3(nockBack.dir.x * nockBack.speed * Time.deltaTime, nockBack.dir.y * nockBack.speed * Time.deltaTime, 0);
                    break;  // ノックバック中はアニメーション判定しない
                }

                // アニメーションの終了を待つ
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("hit") && stateInfo.normalizedTime >= 1.0f)
                {
                    param.hp -= 10;   // ダメージ値（仮）
                    if(param.hp > 0)
                    {
                        setStatus(Status.Alive);
                    }
                    else
                    {
                        setStatus(Status.Dead);
                    }
                }
                break;
            case Status.Dead:
                // status:dead, hp:0 でオブジェクトが消えないケースが存在する
                // そのため、一定時間経過後に強制的に消す
                // added 2024.12.5
                deadTime += Time.deltaTime;
                if(deadTime > 1.0f)
                {
                    // Debug.Log("Enemy Dead Time Over: " + this.name);
                    OnAnimationEnd();
                }
                break;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(Player player, ExpController expController)
    {
        this.expController = expController;

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
        // ポーズ中は無視
        if (GameController.isPause) return;

        // 歩く
        this.transform.localPosition += new Vector3(walkInfo.course.x * param.speed * Time.deltaTime, walkInfo.course.y * param.speed * Time.deltaTime, 0);
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
        walkInfo.course = (walkInfo.player.Position - this.transform.localPosition).normalized;
        SetDirection(walkInfo.course);
    }

    /// <summary>
    /// コライダーが当たったら最初に呼ばれる
    /// </summary>
    /// <param name="collision">相手側の情報が格納される</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // status が Alive の場合のみ
        if (status == Status.Alive) 
        {
            // Weapon に当たったらダメージ
            if (collision.tag == "Weapon")
            {
                // アニメーションの status を変更
                setStatus(Status.Hit);

                // 武器と反対方向へノックバック
                Vector2 dir = (this.transform.localPosition - collision.transform.localPosition).normalized;
                setNockBack(dir);

                SoundManager.SE seNo = (param.type == Type.Normal_1) ? SoundManager.SE.Hit0 : SoundManager.SE.Hit1;
                SoundManager.Instance.PlaySE(seNo);
            }
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
        if (status == Status.Dead) 
        {
            expController.Spawn(param.type, this.transform.localPosition);
            Destroy(this.gameObject);
        }
    }

    // 死亡（GameObject 破棄）時に呼ばれるイベント
    public UnityEvent OnDead = new UnityEvent();
    private void OnDestroy()
    {
        OnDead.Invoke();
    }

    // ステータス（アニメーション）更新
    private void setStatus(Status status)
    {
        this.status = status;
        animator.SetInteger("status", (int)status);
    }

}
