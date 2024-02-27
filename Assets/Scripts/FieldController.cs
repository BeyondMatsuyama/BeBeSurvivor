using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールド管理
/// </summary>
public class FieldController : MonoBehaviour
{
    public static readonly Vector2 fieldLimit = new Vector2(46.0f, 40.0f);

    [SerializeField] private GameObject[] field;
    [SerializeField] private GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void initialize()
    {
        // フィールドを生成（-50, -50, 0.5）から（50, 50, 0.5）までの範囲（5 x 5 のマス目）
        Vector2 pos = new Vector2(-50, 50);
        float size = 5f;
        int cnt = 20;
        for(int x = 0; x <= cnt; x++)
        {
            for(int y = 0; y <= cnt; y++)
            {
                // x, y が 0 と cnt の時は filed[1] を、それ以外は filed[0] を生成
                int idx = (x == 0 || x == cnt || y == 0 || y == cnt) ? 1 : 0;
                // 中央の 1マスはfield[2]を生成
                if(x == cnt / 2 && y == cnt / 2)
                {
                    idx = 2;
                }
                // 生成
                GameObject obj = Instantiate(field[idx], new Vector3(pos.x + (x * size), pos.y - (y * size), 0.5f), Quaternion.identity);
                obj.transform.parent = parent.transform;
            }
        }
    }

}
