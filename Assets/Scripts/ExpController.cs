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
        Exp_S = 0,  // 経験値（小）
        Exp_M,      // 経験値（中）
        Exp_L,      // 経験値（大）
        Health,     // HP 回復
        Mag         // マグネット
    };

    [SerializeField] private PlayerController playerController;
    [SerializeField] private HeaderController headerController;

    [SerializeField] private GameObject[] prefab;
    [SerializeField] private GameObject parent;

    private List<GameObject> exps = new List<GameObject>();

    private const float MoveThresholdSingle =  1f;
    private const float MoveThresholdMulti  = 10f;

    // 経験値の取得数
    private int expNum = 0;
    public int GetExpNum { get { return expNum; } }
    public void SubExpNum(int value) { expNum -= value; }

    /// <summary>
    /// exp オブジェクトを生成する
    /// </summary>
    /// <param name="enemyType">エネミータイプ</param>
    /// <param name="pos">配置座標</param>
    public void Spawn(Enemy.Type enemyType, Vector2 pos)
    {
        headerController.AddDefeatCount();  // 討伐数をカウント

        // エネミータイプに応じて、経験値のタイプを変更
        Type expType = Type.Exp_S;
        switch(enemyType)
        {
            case Enemy.Type.Normal_1:
                expType = Type.Exp_S;
                break;
            case Enemy.Type.Normal_2:
                expType = Type.Exp_M;
                break;
            case Enemy.Type.Boss:
                expType = Type.Exp_L;
                break;
        }

        // 経験値S, M の場合に、回復アイテム、マグネットになる可能性がある
        if(expType == Type.Exp_S || expType == Type.Exp_M)
        {
            // 回復アイテム or マグネットを低確率で生成
            if(Random.Range(0, 100) < 1)
            {
                // 回復アイテム 80%、マグネット 20%
                expType = Type.Health;
                if(Random.Range(0, 100) < 20)
                {
                    expType = Type.Mag;
                }
            }
        }

        // オブジェクト生成
        Vector3 expPos = new Vector3(pos.x, pos.y, 0.1f);
        var obj = Instantiate(prefab[(int)expType], expPos, Quaternion.identity, parent.transform);
        exps.Add(obj);
    }

    /// <summary>
    /// exp オブジェクトを削除する
    /// </summary>
    /// <param name="curObj">削除対象のオブジェクト</param>
    public void Remove(GameObject curObj)
    {
        Exp exp = curObj.GetComponent<Exp>();
        switch(exp.type)
        {
            case Type.Exp_S:
                expNum += 1;
                break;
            case Type.Exp_M:
                expNum += 2;
                break;
            case Type.Exp_L:
                expNum += 10;
                break;
            case Type.Health:
                Debug.Log("Get Health");
                // playerController.AddHp(1);
                break;
            case Type.Mag:
                // 移動中でない exp オブジェクトをプレイヤーに向けて移動させる（マグネットは引き寄せられない）
                Vector2 playerPos = playerController.GetPosition();
                foreach (var obj in exps)
                {
                    Exp e = obj.GetComponent<Exp>();
                    float distance = Vector2.Distance(playerPos, obj.transform.localPosition);
                    if(e.type != Type.Mag && !e.IsMoving && distance < MoveThresholdMulti)
                    {
                        pull(obj, false);
                    }
    }
                break;
        }

        // リストから破棄
        exps.Remove(curObj);
    }

    /// <summary>
    /// プレイヤーに近い exp オブジェクトをプレイヤーに向けて移動させる 
    /// </summary>
    private void Update()
    {
        // ポーズ中は無視
        if (GameController.isPause) return;

        Vector2 playerPos = playerController.GetPosition();
        foreach (var obj in exps)
        {
            // 移動中でない場合
            if (!obj.GetComponent<Exp>().IsMoving)
            {
                // プレイヤーとの距離を計算
                float distance = Vector2.Distance(playerPos, obj.transform.localPosition);
                if (distance < MoveThresholdSingle)
                {
                    pull(obj, true);
                }
            }
        }
    }

    /// <summary>
    /// 引き寄せ処理
    /// </summary>
    /// <param name="obj">対象のオブジェクト</param>
    private void pull(GameObject obj, bool isSingle)
    {
        Exp exp = obj.GetComponent<Exp>();
        exp.Move(playerController, isSingle);
        // 移動終了したらリストから削除
        exp.OnDeleted.AddListener(() => Remove(obj));
    }

}
