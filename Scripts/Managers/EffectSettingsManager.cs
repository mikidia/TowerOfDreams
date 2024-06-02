using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectSetting
{
    public StatusEffect.EffectType effectType;
    public int defaultDuration;
    public int defaultDamagePerSecond;
    public int defaultChance;
}

public class EffectSettingsManager : MonoBehaviour
{
    public static EffectSettingsManager Instance { get; private set; }

    public List<EffectSetting> effectSettings = new List<EffectSetting>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняет объект при загрузке новой сцены
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    void Start()
    {
        // Пример настройки некоторых значений по умолчанию
        effectSettings.Add(new EffectSetting { effectType = StatusEffect.EffectType.Bleeding, defaultDuration = 6, defaultDamagePerSecond = 5, defaultChance = 10 });
        effectSettings.Add(new EffectSetting { effectType = StatusEffect.EffectType.Fire, defaultDuration = 10, defaultDamagePerSecond = 10, defaultChance = 10 });
        effectSettings.Add(new EffectSetting { effectType = StatusEffect.EffectType.Ice, defaultDuration = 10, defaultDamagePerSecond = 7, defaultChance = 10 });
        effectSettings.Add(new EffectSetting { effectType = StatusEffect.EffectType.Poison, defaultDuration = 10, defaultDamagePerSecond = 8, defaultChance = 10 });
        effectSettings.Add(new EffectSetting { effectType = StatusEffect.EffectType.Stun, defaultDuration = 5, defaultDamagePerSecond = 0, defaultChance = 5 });
    }
}
