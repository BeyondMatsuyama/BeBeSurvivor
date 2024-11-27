using UnityEngine;

/// <summary>
/// HP ゲージ
/// </summary>
public class HpGauge : MonoBehaviour
{
    [SerializeField] private Transform hpGauge;    // HP ゲージ

    private const float BaseScale = 3.0f;         // 基本スケール値

    private float hp;               // HP 量
    private float cur;              // 現在の HP（割合）
    private float max;              // 最大 HP（割合）
    private float tgt;              // 目標 HP（割合）
    private const float AnimTime = 1f;      // アニメーション時間(秒)
    private float animTime = 0f;            // アニメーション経過時間

    // ベース表示位置
    private Vector3 basePosition = new Vector3(-0.5f, -0.7f, 0);

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="hp"></param> <summary>
    /// HP 量
    /// </summary>
    public void Init(float hp)
    {
        this.hp = hp;
        cur = hp;
        max = hp;
        tgt = hp;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage"></param> <summary>
    /// ダメージ量
    /// </summary>
    public void Hit(float damage)
    {
        tgt -= damage;
        if(tgt < 0) tgt = 0;
        animTime = AnimTime;
    }

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

            // スケール変更
            float scl = BaseScale * cur / max;
            // Debug.Log("scl: " + scl + " cur: " + cur + " max: " + max);
            hpGauge.localScale = new Vector3(scl, 0.4f, 1f);
        }
    }

}
