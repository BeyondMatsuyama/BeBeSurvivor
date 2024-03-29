using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ゲーム管理
/// </summary>
public class GameController : MonoBehaviour
{
    // カメラ
    [SerializeField] Camera mainCamera;
    // プレイヤー管理
    [SerializeField] PlayerController playerController;
    // 武器管理
    [SerializeField] WeaponController weaponController;
    // 武器ボード
    [SerializeField] WeaponBoard weaponBoard;
    // 経験値管理
    [SerializeField] ExpController expController;
    // フィールドワーク
    [SerializeField] FieldController fieldController;

    // 画面サイズ
    static  Vector2 screenSize = new Vector2(1284, 2788);
    // 中心座標（左下原点）
    private Vector2 centerAxis = new Vector2(screenSize.x / 2, screenSize.y / 2);
    // 長押し判定時間（sec）
    private const float HoldTime = 0.5f;
    // 長押し判定変数
    private float holdTime = HoldTime;

    // 武器のレベルポイントが閾値を超えたら、武器レベルアップ（初期レベルは１で、９レベルまで）
    private readonly int[] levelThreshold = { 0, 5, 10, 20, 30, 40, 60, 80, 100, 65535 };

    // ポーズフラグ
    public static bool isPause = false;
    private int curLevelPoint = 0;

    /// <summary>
    /// フレームワーク
    /// </summary>
    void Update()
    {
        if(!isPause)
        {        
            // プレイヤー移動処理
            if(onMove())
            {
                // カメラを追従させる
                Vector3 pos = playerController.GetPosition();
                pos.z = -1.0f;
                mainCamera.gameObject.transform.localPosition = pos;
            }

            // 武器レベルアップ処理
            if(expController.GetExpNum >= levelThreshold[weaponController.GetLevelPoint()])
            {
                // ポーズ
                isPause = true;
                // 現在のレベルポイントを保持
                curLevelPoint = weaponController.GetLevelPoint();
                // レベルアップボードを表示
                weaponBoard.ShowBoard();
            }
        }
    }

    // ポーズ解除
    public void Resume()
    {
        // 経験値を保持したレベルポイント分減らす
        expController.SubExpNum(levelThreshold[curLevelPoint]);
        // ポーズ解除
        isPause = false;
    }

    // 移動処理
    /// <summary>
    /// 移動（タッチ座標から移動方向のベクトルを求める）
    /// </summary>
    /// <returns>移動した場合は true を返す</returns>
    private bool onMove()
    {
        bool moved = false;
        var mouse = Mouse.current;
        if(mouse.press.ReadValue() == 1)    // press 状態
        {
            if(holdTime > 0.0f)                 // 待機
            {
                holdTime -= Time.deltaTime;
                if(holdTime < 0.0f) holdTime = 0.0f;
            }
            else                                // 移動
            {
                // タッチ座標から移動方向のベクトルを求める
                Vector2 pos = mouse.position.ReadValue();
                Vector2 vec = (pos - centerAxis).normalized;
                // Debug.Log("vec:"+vec);

                // プレイヤーへ渡す
                playerController.Move(vec);
                moved = true;
            }
        }
        else                                // release 状態
        {
            holdTime = HoldTime;
        }

        return moved;
    }

}
