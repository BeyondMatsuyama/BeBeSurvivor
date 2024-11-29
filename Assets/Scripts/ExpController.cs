using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経験値管理
/// </summary>
public class ExpController : MonoBehaviour
{
    public enum Type
    {
        Exp,    // 経験値
        Mag     // マグネット
    };

    public struct ExpData
    {
        public Type         type;
        public GameObject   obj;
    };

    [SerializeField] private PlayerController playerController;
    [SerializeField] private HeaderController headerController;

    [SerializeField] private GameObject[] prefab;
    [SerializeField] private GameObject parent;

    private List<ExpData> exps = new List<ExpData>();

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
        headerController.AddDefeatCount();  // 討伐数をカウント

        // マグネットが既に存在するか確認する
        bool isExistMag = false;
        foreach (var exp in exps)
        {
            if (exp.type == Type.Mag)
            {
                isExistMag = true;
                break;
            }
        }

        // 経験値が50を超えたら確率でマグネットを生成する
        Type type = Type.Exp;
        if (expNum >= 50 && !isExistMag)
        {
            if (Random.Range(0, 100) < 10)
            {
                type = Type.Mag;
            }
        }

        // オブジェクト生成
        var obj = Instantiate(prefab[(int)type], pos, Quaternion.identity, parent.transform);
        exps.Add(new ExpData { type = type, obj = obj });
    }

    /// <summary>
    /// exp オブジェクトを削除する
    /// </summary>
    /// <param name="exp">削除するオブジェクト</param>
    public void Remove(ExpData exp)
    {
        if (exp.type == Type.Exp) expNum++; // 経験値を取得
        else {                              // マグネットの効果
            // 移動中でない exp オブジェクトをプレイヤーに向けて移動させる
            foreach (var e in exps)
            {
                GameObject obj = e.obj;
                if (!obj.GetComponent<Exp>().IsMoving)
                {
                    pull(e);
                }
            }
        }

        // 破棄
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
            GameObject obj = exp.obj;
            if (!obj.GetComponent<Exp>().IsMoving)
            {
                // プレイヤーとの距離を計算
                float distance = Vector2.Distance(playerPos, obj.transform.localPosition);
                if (distance < MoveThreshold)
                {
                    pull(exp);
                }
            }
        }
    }

    /// <summary>
    /// 引き寄せ処理
    /// </summary>
    /// <param name="exp">Exp データ</param>
    private void pull(ExpData exp)
    {
        exp.obj.GetComponent<Exp>().Move(playerController);
        // 移動終了したらリストから削除
        exp.obj.GetComponent<Exp>().OnDeleted.AddListener(() => Remove(exp));
    }

}
