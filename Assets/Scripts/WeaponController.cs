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
        Num         // 武器数
    }

    // 武器インターフェース
    [System.Serializable]
    private struct WeaponInterface
    {
        public Button btnDown;  // レベルダウンボタン
        public Button btnUp;    // レベルアップボタン
        public Text txtLevel;   // レベル表示テキスト
    }
    [SerializeField] private WeaponInterface[] weaponInterface = new WeaponInterface[(int)WeaponType.Num];

    // 最大レベル
    public static readonly int maxLevel = 3;    

    // プレイヤー管理
    [SerializeField] private PlayerController playerController;

    // 武器１（デフォルト武器）
    [SerializeField] private Weapon_1 weapon_1;
    // 武器２（すき）
    [SerializeField] private Weapon_2 weapon_2;
    // 武器３（鎌）
    [SerializeField] private Weapon_3 weapon_3;

    /// <summary>
    /// 開始時処理
    /// </summary>
    void Start()
    {
        // 武器１のボタンイベント設定
        weaponInterface[(int)WeaponType.Weapon_1].btnDown.onClick.AddListener(() => { weapon_1.LevelDown(); setWeapon1Level(); } );
        weaponInterface[(int)WeaponType.Weapon_1].btnUp.onClick.AddListener(() => { weapon_1.LevelUp(); setWeapon1Level(); } );
        weapon_1.Activate();    // アクティブ化
        setWeapon1Level();      // 初期レベル

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

        // 武器３のボタンイベント設定
        weaponInterface[(int)WeaponType.Weapon_3].btnDown.onClick.AddListener(() =>
        {
            // レベルダウン（非アクティブの場合は何もしない）
            if (weapon_3.IsActive())
            {
                weapon_3.LevelDown();
                weapon_3.CreateAndInitialize(playerController.GetPlayer());
                setWeapon3Level();
            }
        });
        weaponInterface[(int)WeaponType.Weapon_3].btnUp.onClick.AddListener(() =>
        {
            // 非アクティブの場合はアクティブ化
            if (!weapon_3.IsActive())   weapon_3.Activate();
            // レベルアップ
            else                        weapon_3.LevelUp();
            weapon_3.CreateAndInitialize(playerController.GetPlayer());
            setWeapon3Level();
        });

    }

    /// <summary>
    /// 武器１のレベル表示
    /// </summary>
    private void setWeapon1Level()
    {
        weaponInterface[(int)WeaponType.Weapon_1].txtLevel.text = (weapon_1.GetLevelPoint()).ToString();  // 表示はプラス１する
    }

    // 武器２のレベル表示
    private void setWeapon2Level()
    {
        weaponInterface[(int)WeaponType.Weapon_2].txtLevel.text = (weapon_2.GetLevelPoint()).ToString();  // 表示はプラス１する
    }

    // 武器３のレベル表示
    private void setWeapon3Level()
    {
        weaponInterface[(int)WeaponType.Weapon_3].txtLevel.text = (weapon_3.GetLevelPoint()).ToString();  // 表示はプラス１する
    }

    /// <summary>
    /// 全武器のレベルポイント取得
    /// </summary>
    /// <returns>レベルポイント</returns>
    public int GetLevelPoint() { return weapon_1.GetLevelPoint() + weapon_2.GetLevelPoint() + weapon_3.GetLevelPoint();}

    /// <summary>
    /// 武器タイプごとのレベルポイント取得
    /// </summary>
    /// <param name="type">武器タイプ</param>
    /// <returns>レベルポイント</returns>
    public int GetLevelPointForWeapon(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Weapon_1:   return weapon_1.GetLevelPoint();
            case WeaponType.Weapon_2:   return weapon_2.GetLevelPoint();
            case WeaponType.Weapon_3:   return weapon_3.GetLevelPoint();
        }
        return 0;        
    }

    /// <summary>
    /// レベルアップ処理
    /// </summary>
    /// <param name="type">武器タイプ</param>
    /// <returns>レベルアップした場合は true を返す</returns>
    public bool LevelUp(WeaponType type)
    {
        bool isLevelUp = false;
        switch (type)
        {
            case WeaponType.Weapon_1:
                isLevelUp = weapon_1.LevelUp();
                setWeapon1Level();
                break;
            case WeaponType.Weapon_2:
                isLevelUp = weapon_2.LevelUp();
                setWeapon2Level();
                break;
            case WeaponType.Weapon_3:
                isLevelUp = weapon_3.LevelUp();
                weapon_3.CreateAndInitialize(playerController.GetPlayer());
                setWeapon3Level();
                break;
        }
        return isLevelUp;        
    }

}
