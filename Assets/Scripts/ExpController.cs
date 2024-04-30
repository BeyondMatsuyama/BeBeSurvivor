using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経験値管理
/// </summary>
public class ExpController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject parent;

    private List<GameObject> exps = new List<GameObject>();

    private const float MoveThreshold = 2f;

    // 経験値の取得数
    private int expNum = 0;
    public int GetExpNum { get { return expNum; } }
    public void SubExpNum(int value) { expNum -= value; }

    /// <summary>
    /// exp オブジェクトを生成する
    /// </summary>
    /// <param name="pos">配置座標</param>
    public void Spawn(Vector2 pos)
    {
        var exp = Instantiate(prefab, pos, Quaternion.identity, parent.transform);
        exps.Add(exp);
    }

    /// <summary>
    /// exp オブジェクトを削除する
    /// </summary>
    /// <param name="exp">削除するオブジェクト</param>
    public void Remove(GameObject exp)
    {
        expNum++;
        exps.Remove(exp);
    }

    /// <summary>
    /// プレイヤーに近い exp オブジェクトをプレイヤーに向けて移動させる 
    /// </summary>
    private void Update()
    {
        // ポーズ中は無視
        if (GameController.isPause) return;

        Vector2 playerPos = playerController.GetPosition();
        foreach (var exp in exps)
        {
            // 移動中でない場合
            if (!exp.GetComponent<Exp>().IsMoving)
            {
                // プレイヤーとの距離を計算
                float distance = Vector2.Distance(playerPos, exp.transform.localPosition);
                if (distance < MoveThreshold)
                {
                    exp.GetComponent<Exp>().Move(playerController);
                    // 移動終了したらリストから削除
                    exp.GetComponent<Exp>().OnDeleted.AddListener(() => Remove(exp));
                }
            }
        }
    }

}
