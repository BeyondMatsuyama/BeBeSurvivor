using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールド管理
/// </summary>
public class FieldController : MonoBehaviour
{
    private enum FieldID
    {
        Grass = 0,
        Grass2,
        Grass3,
        Grass4,
        Grass5,
        OutOfArea
    }

    // フィールドの初期位置
    private Vector2 startPos = new Vector2(-20.5f, 100.5f);
    // タイルの数
    private Vector2Int tileNum = new Vector2Int(40, 200);

    // プレイヤーの移動制限
    private const float walkLimit = 1.0f;
    private Vector2 walkLeftUpper, walkRightLower;

    // カメラの移動制限
    private readonly Vector2 cameraLimit  = new Vector2(5.0f, 12.0f);
    private Vector2 cameraLeftUpper, cameraRightLower;

    // オブジェクト（Prefab）
    [SerializeField] private GameObject[] field;
    [SerializeField] private GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーの移動制限
        walkLeftUpper  = new Vector2(startPos.x + walkLimit, startPos.y - walkLimit);
        walkRightLower = new Vector2(startPos.x + (tileNum.x - 1) - walkLimit, startPos.y - (tileNum.y - 1) + walkLimit);

        // カメラの移動制限
        cameraLeftUpper  = new Vector2(startPos.x + cameraLimit.x, startPos.y - cameraLimit.y);
        cameraRightLower = new Vector2(startPos.x + (tileNum.x - 1) - cameraLimit.x, startPos.y - (tileNum.y - 1) + cameraLimit.y);

        initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void initialize()
    {
        for(int x = 0; x < tileNum.x; x++)
        {
            for(int y = 0; y < tileNum.y; y++)
            {
                // 最外側はエリア外
                int idx = Random.Range(0, 5);
                if(x == 0 || x == tileNum.x-1 || y == 0 || y == tileNum.y-1)
                {
                    idx = (int)FieldID.OutOfArea;
                }
                // 生成
                GameObject obj = Instantiate(field[idx], new Vector3(startPos.x + x, startPos.y - y, 0.5f), Quaternion.identity);
                obj.name = "field_" + idx + " : "+ x + ", " + y;
                obj.transform.parent = parent.transform;
            }
        }
    }

    /// <summary>
    /// プレイヤーの移動制限
    /// </summary>
    /// <param name="playerPos">プレイヤー座標</param>
    /// <returns>プレイヤー座標</returns>
    public Vector2 GetPlayerPosition(Vector2 playerPos)
    {
        Vector2 pos = playerPos;
        if(playerPos.x < walkLeftUpper.x)
        {
            pos.x = walkLeftUpper.x;
        }
        if(playerPos.x > walkRightLower.x)
        {
            pos.x = walkRightLower.x;
        }
        if(playerPos.y > walkLeftUpper.y)
        {
            pos.y = walkLeftUpper.y;
        }
        if(playerPos.y < walkRightLower.y)
        {
            pos.y = walkRightLower.y;
        }
        return pos;
    }

    /// <summary>
    /// カメラの座標制限（追従限界）
    /// </summary>
    /// <param name="playerPos">プレイヤー座標</param>
    /// <returns>カメラ座標</returns>
    public Vector2 GetCameraPosition(Vector2 playerPos)
    {
        Vector2 cameraPos = playerPos;
        if(playerPos.x < cameraLeftUpper.x)
        {
            cameraPos.x = cameraLeftUpper.x;
        }
        if(playerPos.x > cameraRightLower.x)
        {
            cameraPos.x = cameraRightLower.x;
        }
        if(playerPos.y > cameraLeftUpper.y)
        {
            cameraPos.y = cameraLeftUpper.y;
        }
        if(playerPos.y < cameraRightLower.y)
        {
            cameraPos.y = cameraRightLower.y;
        }
        return cameraPos;
    }

}
