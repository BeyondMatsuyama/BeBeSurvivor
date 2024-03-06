using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武器管理
/// </summary>
public class WeaponController : MonoBehaviour
{
    // 武器種類
    public enum WeaponType
    {
        Weapon_1,   // 武器１（デフォルト武器）
        Weapon_2,   // 武器２
        Weapon_3,   // 武器３
        Weapon_Num  // 武器数
    }

    // 武器インターフェース
    [System.Serializable]
    private struct WeaponInterface
    {
        public Button btnDown;  // レベルダウンボタン
        public Button btnUp;    // レベルアップボタン
        public Text txtLevel;   // レベル表示テキスト
    }
    [SerializeField] private WeaponInterface[] weaponInterface = new WeaponInterface[(int)WeaponType.Weapon_Num];

    // 最大レベル
    public static readonly int maxLevel = 3;    

    // プレイヤー管理
    [SerializeField] private PlayerController playerController;

    // 武器１（デフォルト武器）
    [SerializeField] private Weapon_1 weapon_1;
    // 武器２（すき）
    [SerializeField] private Weapon_2 weapon_2;

    /// <summary>
    /// 開始時処理
    /// </summary>
    void Start()
    {
        // 武器１のボタンイベント設定
        weaponInterface[(int)WeaponType.Weapon_1].btnDown.onClick.AddListener(() => { weapon_1.LevelDown(); setWeapon1Level(); } );
        weaponInterface[(int)WeaponType.Weapon_1].btnUp.onClick.AddListener(() => { weapon_1.LevelUp(); setWeapon1Level(); } );
        setWeapon1Level();  // 初期レベル

        // 武器２のボタンイベント設定
        weaponInterface[(int)WeaponType.Weapon_2].btnDown.onClick.AddListener(() =>
        {
            // レベルダウン（非アクティブの場合は何もしない）
            if (weapon_2.IsActive())
            {
                weapon_2.LevelDown();
                setWeapon2Level();
            }
        });
        weaponInterface[(int)WeaponType.Weapon_2].btnUp.onClick.AddListener(() =>
        {
            // 非アクティブの場合はアクティブ化
            if (!weapon_2.IsActive())   weapon_2.Activate();
            // レベルアップ
            else                        weapon_2.LevelUp();
            setWeapon2Level();
        });

    }

    /// <summary>
    /// 武器１のレベル表示
    /// </summary>
    private void setWeapon1Level()
    {
        weaponInterface[(int)WeaponType.Weapon_1].txtLevel.text = (weapon_1.GetLevel() + 1).ToString();  // 表示はプラス１する
    }

    // 武器２のレベル表示
    private void setWeapon2Level()
    {
        weaponInterface[(int)WeaponType.Weapon_2].txtLevel.text = (weapon_2.GetLevel() + 1).ToString();  // 表示はプラス１する
    }
}
