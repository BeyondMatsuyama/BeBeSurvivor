using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 経験値オブジェクト
/// </summary>
public class Exp : MonoBehaviour
{
    public ExpController.Type type;

    private PlayerController playerController;
    private const float MoveSpeedSingle =  4f;
    private const float MoveSpeedMulti  = 12f;
    private bool isMoving = false;
    public bool IsMoving { get => isMoving; }
    private float moveSpeed = 0f;

    /// <summary>
    /// プレイヤーに向けて移動するトリガー
    /// </summary>
    /// <param name="playerController"></param>
    public void Move(PlayerController playerController, bool isSingle)
    {
        this.playerController = playerController;
        isMoving = true;

        moveSpeed = isSingle ? MoveSpeedSingle : MoveSpeedMulti;
    }

    /// <summary>
    /// フレームワーク
    /// </summary>
    private void Update()
    {
        // ポーズ中はスキップ
        if (GameController.isPause) return;

        if (isMoving)
        {
            var targetPosition = playerController.GetPosition();
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);

            if (Vector2.Distance(transform.localPosition, targetPosition) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// GameObject 破棄時に呼ばれるイベント
    /// </summary>
    /// <returns>イベント</returns>
    public UnityEvent OnDeleted = new UnityEvent();
    private void OnDestroy()
    {
        OnDeleted.Invoke();
    }

}
