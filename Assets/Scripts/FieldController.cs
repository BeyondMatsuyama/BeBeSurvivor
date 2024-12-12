using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールド管理
/// </summary>
public class FieldController : MonoBehaviour
{
    public static readonly Vector2 fieldLimit = new Vector2(46.0f, 40.0f);

    private enum FieldID
    {
        Grass = 0,
        Grass2,
        Grass3,
        Grass4,
        Grass5,
        OutOfArea
    }

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
        Vector2 startPos = new Vector2(-20.5f, 100.5f);
        Vector2 squareNum = new Vector2(40, 200);
        int OutOfAreaWidth = 5;
        int[] threasholdX = new int[] {OutOfAreaWidth, (int)squareNum.x - 1 - OutOfAreaWidth};
        int[] threasholdY = new int[] {OutOfAreaWidth, (int)squareNum.y - 1 - OutOfAreaWidth};


        for(int x = 0; x < squareNum.x; x++)
        {
            for(int y = 0; y < squareNum.y; y++)
            {
                // エリア外は外周の４マス
                int idx = (int)FieldID.OutOfArea;
                // その内側は草地をランダムに配置
                if(x > threasholdX[0] && x < threasholdX[1] && y > threasholdY[0] && y < threasholdY[1]) idx = Random.Range(0, 5);
                // 生成
                GameObject obj = Instantiate(field[idx], new Vector3(startPos.x + x, startPos.y - y, 0.5f), Quaternion.identity);
                obj.name = "field_" + idx + " : "+ x + ", " + y;
                obj.transform.parent = parent.transform;
            }
        }
    }

}
