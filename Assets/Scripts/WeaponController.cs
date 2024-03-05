using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器管理
/// </summary>
public class WeaponController : MonoBehaviour
{
    // 最大レベル
    public static readonly int maxLevel = 3;
    

    // プレイヤー管理
    [SerializeField] private PlayerController playerController;

    // 武器１（デフォルト武器）
    [SerializeField] private Weapon_1 weapon_1;


}
