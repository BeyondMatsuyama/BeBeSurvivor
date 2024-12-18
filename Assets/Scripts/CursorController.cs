using UnityEngine;

/// <summary>
/// カーソル制御
/// </summary>
public class CursorController : MonoBehaviour
{
    /// <summary>
    /// パーツ種類
    /// </summary>
    private enum Parts
    {
        Base = 0,   // 下敷き
        Dir,        // 向き
        Num
    }
    [SerializeField] private GameObject[] parts = new GameObject[(int)Parts.Num];

    // センター座標
    private Vector2 center;
    // センター座標取得
    public Vector2 Center { get { return center; } }

    /// <summary>
    /// センター座標設定（タッチ開始時）
    /// </summary>
    /// <param name="center">タッチ開始時の座標</param>
    public void SetCenter(Vector2 center)
    {
        this.center = center;
        parts[(int)Parts.Base].transform.localPosition = center;
        UpdatePosition(center); // カーソル初期位置
        activate(true);
    }

    /// <summary>
    /// 座標更新（移動時）
    /// </summary>
    /// <param name="pos">現在のタッチ座標</param>
    public void UpdatePosition(Vector2 pos)
    {
        // タッチ座標に応じて Dir の位置を調整
        Vector2 offset = (pos - center).normalized * 26.0f;
        parts[(int)Parts.Dir].transform.localPosition = offset;
    }

    /// <summary>
    /// カーソル非表示
    /// </summary>
    public void Hide()
    {
        activate(false);
    }

    /// <summary>
    /// オブジェクトアクティブ設定
    /// </summary>
    /// <param name="isActive">アクティブ状態</param>
    private void activate(bool isActive)
    {
        // ベースオブジェクト（親）のみアクティブ設定
        parts[(int)Parts.Base].SetActive(isActive);
    }

}
