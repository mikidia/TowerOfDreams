using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private void Awake()
    {
        // Здесь можно выполнить инициализацию, если потребуется
    }

    private void OnTriggerEnter(Collider other)
    {
        // Получаем компоненты EnemyMain и StatusEffect у столкнувшегося объекта
        EnemyMain enemy = other.GetComponent<EnemyMain>();
        StatusEffect statusEffect = other.GetComponent<StatusEffect>();

        // Проверяем, что враг действительно имеет компоненты EnemyMain и StatusEffect
        if (enemy != null && statusEffect != null)
        {
            // Кешируем необходимые данные игрока
            var playerInstance = Player._instance;
            var playerDamage = playerInstance.PlayerDamage;
            var effectsToApply = playerInstance.effectsToApply;

            // Наносим урон врагу
            enemy.GetDamage(playerDamage);

            // Применяем эффекты к врагу
            foreach (var effectType in effectsToApply)
            {
                if (statusEffect.EffectFloatValues.TryGetValue(effectType, out int chance))
                {
                    // Проверка шанса на добавление эффекта
                    int randomizeNumber = Random.Range(0, chance);
                    Debug.Log(randomizeNumber);
                    if (randomizeNumber == 0)
                    {
                        statusEffect.AddEffectByType(effectType);
                    }
                }
            }
        }
    }
}
