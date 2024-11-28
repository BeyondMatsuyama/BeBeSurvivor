using UnityEngine;

/// <summary>
/// HP ゲージ
/// </summary>
public class HpGauge : MonoBehaviour
{
    [SerializeField] private Transform gauge;     // HP ゲージ

    private const float BaseScale = 3.0f;         // 基本スケール値（長さ）

    private float cur;              // 現在の HP（割合）
    private int   max;              // 最大 HP（割合）
    private int   tgt;              // 目標 HP（割合）
    private const float AnimTime = 0.5f;    // アニメーション時間(秒)
    private float animTime = 0f;            // アニメーション経過時間

    // ベース表示位置
    private Vector3 basePosition = new Vector3(-0.5f, -0.7f, 0);

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="hp"></param> <summary>HP 量</summary>
    public void Init(int hp)
    {
        cur = hp;
        max = hp;
        tgt = hp;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage"></param> <summary>ダメージ量</summary>
    public bool Hit(int damage)
    {
        if(animTime > 0) return false;

        bool isDead = false;
        tgt -= damage;
        if(tgt <= 0)    // 死亡処理
        {
            tgt = 0;
            isDead = true;
        }
        animTime = AnimTime;
        return isDead;
    }

    /// <summary>
    /// 表示座標設定
    /// </summary>
    /// <param name="pos">座標</param>
    public void SetPosition(Vector3 pos)
    {
        this.transform.localPosition = pos + basePosition;
    }

    /// <summary>
    /// フレームワーク
    /// </summary>
    void Update()
    {
        if(animTime > 0)
        {
            cur = Mathf.Lerp(tgt, cur, animTime / AnimTime);
            animTime -= Time.deltaTime;
            if(animTime <= 0)
            {
                cur = tgt;
                animTime = 0;
            }

            // スケール変更
            float scl = BaseScale * cur / max;
            // Debug.Log("scl: " + scl + " cur: " + cur + " max: " + max);
            gauge.localScale = new Vector3(scl, 0.4f, 1f);

            // HP が 0 になったら非表示
            if(cur <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

}
