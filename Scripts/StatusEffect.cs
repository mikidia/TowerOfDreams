using System.Collections;
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
        public int duration;
        public int damagePerSecond;
        public int defaultChance;
    }

    [SerializeField] private List<Effect> activeEffects = new List<Effect>();
    public List<EffectType> selectedStatusEffects = new List<EffectType>();

    public Dictionary<EffectType, int> EffectFloatValues { get; private set; } = new Dictionary<EffectType, int>();

    private void Awake()
    {
        InitializeEffectFloatValues();
    }

    private void Update()
    {
        if (activeEffects.Count > 0)
        {
            ApplyEffects();
        }
    }

    private void InitializeEffectFloatValues()
    {
        var effectSettings = EffectSettingsManager.Instance.effectSettings;
        foreach (var setting in effectSettings)
        {
            EffectFloatValues[setting.effectType] = setting.defaultChance;
        }
    }

    private void ApplyEffects()
    {
        List<Effect> expiredEffects = new List<Effect>();

        foreach (var effect in activeEffects)
        {
            StartCoroutine(DurationCoroutine(effect));
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

    private void ApplyEffect(Effect effect)
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
                // Обработка оглушения
                break;
        }
    }

    private void ApplyDamage(DamageType damageType, float amount)
    {
        // Логика применения урона
        Debug.Log($"Применение {amount} урона типа {damageType}.");
    }

    public void AddEffect(Effect newEffect)
    {
        if (newEffect != null)
        {
            activeEffects.Add(newEffect);
        }
    }

    public void AddEffectByType(EffectType effectType)
    {
        var effectSetting = EffectSettingsManager.Instance.effectSettings.Find(es => es.effectType == effectType);
        if (effectSetting != null)
        {
            Effect newEffect = new Effect
            {
                effectType = effectType,
                duration = effectSetting.defaultDuration,
                damagePerSecond = effectSetting.defaultDamagePerSecond,
                defaultChance = effectSetting.defaultChance
            };

            AddEffect(newEffect);
        }
    }

    private IEnumerator DurationCoroutine(Effect effect)
    {
        yield return new WaitForSeconds(effect.duration);
        effect.duration = 0;
    }
}
