using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    public enum EffectType { Bleeding, Fire, Ice, Poison, Stun }
    public enum DamageType { Bleeding, Fire, Ice, Poison, None }

    [System.Serializable]
    public class Effect
    {
        public EffectType effectType;
        public float duration;
        public float damagePerSecond;
    }

    [System.Serializable]
    public class EffectSettings
    {
        public EffectType effectType;
        public float defaultDuration;
        public float defaultDamagePerSecond;
    }

    [SerializeField] private List<EffectSettings> effectSettings = new List<EffectSettings>();

    private Dictionary<EffectType, float> effectFloatValues = new Dictionary<EffectType, float>()
    {
        { EffectType.Bleeding, 6f },
        { EffectType.Fire, 10f },
        { EffectType.Ice, 10f },
        { EffectType.Poison, 10f },
        { EffectType.Stun, 5f }
    };

    [SerializeField] private List<Effect> activeEffects = new List<Effect>();
    public List<EffectType> selectedStatusEffects = new List<EffectType>();

    public Dictionary<EffectType, float> EffectFloatValues { get => effectFloatValues; set => effectFloatValues = value; }
    public List<Effect> ActiveEffects { get => activeEffects; set => activeEffects = value; }

    void Awake()
    {
        InitializeEffectFloatValues();
    }

    void Update()
    {
        ApplyEffects();
    }

    void InitializeEffectFloatValues()
    {
        foreach (var setting in effectSettings)
        {
            effectFloatValues[setting.effectType] = setting.defaultDuration;
        }
    }

    void ApplyEffects()
    {
        List<Effect> expiredEffects = new List<Effect>();

        foreach (var effect in activeEffects)
        {
            effect.duration -= Time.deltaTime;
            if (effect.duration <= 0)
            {
                expiredEffects.Add(effect);
            }
            else
            {
                ApplyEffect(effect);
            }
        }

        foreach (var effect in expiredEffects)
        {
            activeEffects.Remove(effect);
        }
    }

    void ApplyEffect(Effect effect)
    {
        switch (effect.effectType)
        {
            case EffectType.Fire:
                ApplyDamage(DamageType.Fire, effect.damagePerSecond * Time.deltaTime);
                break;
            case EffectType.Ice:
                ApplyDamage(DamageType.Ice, effect.damagePerSecond * Time.deltaTime);
                break;
            case EffectType.Poison:
                ApplyDamage(DamageType.Poison, effect.damagePerSecond * Time.deltaTime);
                break;
            case EffectType.Stun:
                // Пример обработки оглушения
                // Оглушение может просто замедлить или остановить персонажа
                break;
        }
    }

    void ApplyDamage(DamageType damageType, float amount)
    {
        // Логика применения урона
        Debug.Log($"Applying {amount} {damageType} damage.");
    }

    public void AddEffect(Effect newEffect)
    {
        activeEffects.Add(newEffect);
    }

    public void AddEffectByType(EffectType effectType)
    {
        var effectSetting = effectSettings.Find(es => es.effectType == effectType);
        if (effectSetting != null)
        {
            Effect newEffect = new Effect
            {
                effectType = effectType,
                duration = effectSetting.defaultDuration,
                damagePerSecond = effectSetting.defaultDamagePerSecond
            };

            AddEffect(newEffect);
        }
    }
}
