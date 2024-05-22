using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    public enum EffectType { Fire, Ice, Poison, Stun, }
    public enum DamageType { Fire, Ice, Poison, None }

    [System.Serializable]
    public class Effect
    {
        public EffectType effectType;
        public float duration;
        public float damagePerSecond;
    }

    public List<Effect> activeEffects = new List<Effect>();

    void Update()
    {
        ApplyEffects();
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
        //Debug.Log($"Applying {amount} {damageType} damage.");
    }

    public void AddEffect(Effect newEffect)
    {
        activeEffects.Add(newEffect);
    }
}

