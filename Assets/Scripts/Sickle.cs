using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 鎌（農具）
/// </summary>
public class Sickle : MonoBehaviour
{
/*
    void Start()
    {
        // 初期化
        Initialize(0.0f);
    }
*/

    // 半径
    public const float Radius = 4.0f;
    // 回転速度（秒間）
    private const float RotateSpeed = 180f;
    // 自転速度（秒間）
    private const float SpinSpeed   = 1080f;

    // 角度（位置を決定するための角度）
    private float rotateAngle = 0.0f;
    // 角度（自転）
    private float spinAngle = 0.0f;

    // プレイヤー
    private Player player;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="player">プレイヤー</param>
    /// <param name="rot">初期角度</param>
    public void Initialize(Player player, float rot)
    {
        rotateAngle = rot;
        spinAngle = 0.0f;
        this.player = player;
    }

    /// <summary>
    /// フレームワーク
    /// </summary>
    void Update()
    {
        // 角度（公転）を増加
        rotateAngle -= RotateSpeed * Time.deltaTime;
        // 角度（自転）を増加
        spinAngle -= SpinSpeed * Time.deltaTime;

        // 角度（公転）をラジアンに変換
        float rad = rotateAngle * Mathf.Deg2Rad;
        // 角度（自転）をラジアンに変換
        float spinRad = spinAngle * Mathf.Deg2Rad;

        // 位置を更新
        Vector2 pos = player.Position;
        transform.position = new Vector3(
            pos.x + Radius * Mathf.Cos(rad),
            pos.y + Radius * Mathf.Sin(rad),
            0.0f
        );

        // 回転を更新
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, spinAngle);
    }

    /// <summary>
    /// コライダーが当たったら最初に呼ばれる
    /// </summary>
    /// <param name="collision">相手側の情報が格納される</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy に当たったら消滅
        if (collision.tag == "Enemy")
        {
            // 親オブジェクトへ通知
            OnHit.Invoke();
        }
    }

    // ヒット時に呼ばれるイベント
    public UnityEvent OnHit = new UnityEvent();

}
