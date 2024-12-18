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
    // ヘッダ情報
    [SerializeField] HeaderController headerController;
    // リザルト情報
    [SerializeField] ResultController resultController;
    // カーソル制御
    [SerializeField] CursorController cursorController;
    
    // 長押し判定時間（sec）
    private const float HoldTime = 0.5f;
    // 長押し判定変数
    private float holdTime = HoldTime;

    // 武器のレベルポイントが閾値を超えたら、武器レベルアップ（初期レベルは１で、９レベルまで）
    private readonly int[] levelThreshold = { 0, 10, 25, 50, 100, 200, 300, 400, 500, 65535 };

    // 討伐数の閾値
    private const int DefeatThreshold = 2000;

    // ポーズフラグ
    public static bool isPause = false;
    private int curLevelPoint = 0;

    void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.BGM.Game);
    }

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
                // プレイヤー位置
                Vector3 pos = playerController.GetPosition();
                // カメラの移動制限
                Vector2 camPos = fieldController.GetCameraPosition(new Vector2(pos.x, pos.y));
                // カメラ位置更新
                mainCamera.gameObject.transform.localPosition = new Vector3(camPos.x, camPos.y, -1.0f);
            }

            // ヘッダの経験値ゲージ更新
            headerController.SetExpGauge(expController.GetExpNum, levelThreshold[weaponController.GetLevelPoint()]);

            // 武器レベルアップ処理
            if(expController.GetExpNum >= levelThreshold[weaponController.GetLevelPoint()])
            {
                // ポーズ
                isPause = true;
                // 現在のレベルポイントを保持
                curLevelPoint = weaponController.GetLevelPoint();
                // レベルアップボードを表示
                weaponBoard.ShowBoard();
                // カーソル非表示
                cursorController.Hide();

                SoundManager.Instance.PlaySE(SoundManager.SE.LevelUp);
            }

            // 死亡処理
            if(playerController.IsDead())
            {
                // ポーズ
                isPause = true;
                // リザルト表示（ゲームオーバー）
                resultController.Show(false, headerController.TimeValue, headerController.DefeatCount);

                SoundManager.Instance.PlaySE(SoundManager.SE.Lose);
            }
            // クリア判定
            else
            {
                // 討伐数が一定数を超えたらクリア
                if(headerController.DefeatCount >= DefeatThreshold)
                {
                    // ポーズ
                    isPause = true;
                    // リザルト表示（クリア）
                    resultController.Show(true, headerController.TimeValue, headerController.DefeatCount);
                    // カーソル非表示
                    cursorController.Hide();

                    SoundManager.Instance.PlaySE(SoundManager.SE.Win);
                }
            }
        }
        else
        {
            if(resultController.ToTitle)
            {
                SoundManager.Instance.StopBGM();
                SoundManager.Instance.StopSE();

                // タイトルシーンへ戻る
                UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
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

        // 押された瞬間
        if(mouse.press.wasPressedThisFrame)
        {
            Vector2 pos = mouse.position.ReadValue();
            cursorController.SetCenter(pos);
            // Debug.Log("press : "+pos);
        }
        // 押したまま
        else if(mouse.press.isPressed)
        {
            if(holdTime > 0.0f)                 // 一定時間待機
            {
                holdTime -= Time.deltaTime;
                if(holdTime < 0.0f) holdTime = 0.0f;
            }
            else                                // 移動
            {
                // タッチ座標から移動方向のベクトルを求める
                Vector2 pos = mouse.position.ReadValue();
                Vector2 vec = (pos - cursorController.Center).normalized;
                // Debug.Log("repeated");                
                // Debug.Log("vec:"+vec);

                // カーソル更新
                cursorController.UpdatePosition(pos);
                // プレイヤーへ渡す
                playerController.Move(vec);
                moved = true;                
            }
        }
        // 離した瞬間
        else if(mouse.press.wasReleasedThisFrame)
        {
            holdTime = HoldTime;
            cursorController.Hide();
            // Debug.Log("release");
        }

        return moved;
    }

}
