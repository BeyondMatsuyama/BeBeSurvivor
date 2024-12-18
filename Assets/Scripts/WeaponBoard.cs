using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武器ボード
/// </summary>
public class WeaponBoard : MonoBehaviour
{
    // ゲーム管理
    [SerializeField] private GameController gameController;
    // 武器管理
    [SerializeField] private WeaponController weaponController;

    // ボード（オブジェクト）
    [SerializeField] private GameObject board;
    // ボタン
    [SerializeField] private Button[] btnWeapon;
    // 星（text）
    [SerializeField] private StarText[] txtStars;
    [System.Serializable]
    private class StarText
    {
        public Text[] stars;
    }

    /// <summary>
    /// 起動時処理
    /// </summary>
    void Start()
    {
        // ボタン操作（リスナー）
        for(int i=0; i<btnWeapon.Length; i++)
        {
            int index = i;
            btnWeapon[i].onClick.AddListener(() => 
            {
                if(weaponController.LevelUp((WeaponController.WeaponType)index))
                {
                    // ボードを非表示
                    HideBoard();
                    // ポーズ解除
                    gameController.Resume();

                    SoundManager.Instance.PlaySE(SoundManager.SE.Select);
                }
            });
        }
        // ボードを非表示
        HideBoard();
    }

    /// <summary>
    /// ボード表示
    /// </summary>
    public void ShowBoard()
    {
        // ボードを表示
        board.SetActive(true);

        // 各武器レベルを設定
        for(int type=0 ; type<(int)WeaponController.WeaponType.Num ; type++)
        {
            int lp = weaponController.GetLevelPointForWeapon((WeaponController.WeaponType)type);
            for(int i=0; i<txtStars[type].stars.Length; i++)
            {
                txtStars[type].stars[i].text = (lp > i) ? "★" : "☆";
            }
        }
    }

    /// <summary>
    /// ボード非表示
    /// </summary>
    private void HideBoard()
    {
        // ボードを非表示
        board.SetActive(false);
    }

}
