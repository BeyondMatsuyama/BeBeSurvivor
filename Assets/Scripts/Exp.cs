using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 経験値オブジェクト
/// </summary>
public class Exp : MonoBehaviour
{
    private PlayerController playerController;
    private const float MoveSpeed = 4f;
    private bool isMoving = false;
    public bool IsMoving { get => isMoving; }

    /// <summary>
    /// プレイヤーに向けて移動するトリガー
    /// </summary>
    /// <param name="playerController"></param>
    public void Move(PlayerController playerController)
    {
        this.playerController = playerController;
        isMoving = true;
        StartCoroutine(move());
    }

    /// <summary>
    /// プレイヤーに向けて移動する処理
    /// </summary>
    /// <returns>コルーチン</returns>
    private IEnumerator move()
    {
        var elapsedTime = 0f;
        var startPosition = transform.position;
        while (elapsedTime < 1f)
        {
            var targetPosition = playerController.GetPosition();
            elapsedTime += Time.deltaTime * MoveSpeed;
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime);
            yield return null;
        }
        Destroy(gameObject);
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
