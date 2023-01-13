using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationName
{
    public struct EnemyAnimationNames
    {
        public static readonly string Attack = "Attack";
        public static readonly string Charge = "Charge";
        public static readonly string Damage = "Damage";
        public static readonly string SpecialAttack = "Ultimate";
        public static readonly string SpecialCharge = "Charge";
        public static readonly string HitRight = "HitRight";
        public static readonly string HitLeft = "HitLeft";
        public static readonly string Down = "Down";
        public static readonly string Threat = "Threat";
    }
    public struct PlayerAnimationNames
    {
        public static readonly string Kick = "Kick";
        public static readonly string Damage = "Damage";
        public static readonly string Ultimate = "Ultimate";
    }
}

public class InGameConst
{
    /// <summary>
    /// 必殺技ゲージが貯まる1回あたりの値
    /// </summary>
    public const int ALTIMATE_GAUGE_POINT = 2;
}
