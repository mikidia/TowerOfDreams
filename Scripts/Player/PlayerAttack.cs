using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private void Awake()
    {
        // ����� ����� ��������� �������������, ���� �����������
    }

    private void OnTriggerEnter(Collider other)
    {
        // �������� ���������� EnemyMain � StatusEffect � �������������� �������
        EnemyMain enemy = other.GetComponent<EnemyMain>();
        StatusEffect statusEffect = other.GetComponent<StatusEffect>();

        // ���������, ��� ���� ������������� ����� ���������� EnemyMain � StatusEffect
        if (enemy != null && statusEffect != null)
        {
            // �������� ����������� ������ ������
            var playerInstance = Player._instance;
            var playerDamage = playerInstance.PlayerDamage;
            var effectsToApply = playerInstance.effectsToApply;

            other.GetComponent<EnemyStateMachine>().SlowDown(0.15f);
            
            if (playerInstance != null && playerInstance.VampirismIsActive)
            {
                playerInstance.Hp += playerDamage;
                if (playerInstance.Hp > playerInstance.MaxHp)
                {

                    playerInstance.Hp = playerInstance.MaxHp;

                }

            }


            // ������� ���� �����
            enemy.GetDamage(playerDamage);

            // ��������� ������� � �����
            foreach (var effectType in effectsToApply)
            {
                if (statusEffect.EffectFloatValues.TryGetValue(effectType, out int chance))
                {
                    // �������� ����� �� ���������� �������
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
