using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private void Awake()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        List<EnemyMain> enemy = new List<EnemyMain>();
        List<StatusEffect> statusEffect = new List<StatusEffect>();


        foreach (var i in other.GetComponents<EnemyMain>())
        {
            enemy.Add(i);

        }
        foreach (var i in other.GetComponents<StatusEffect>())
        {

            statusEffect.Add(i);
        }


        if (enemy.Count > 0)
        {
            // �������� ����������� ������
            var playerInstance = Player._instance;
            var playerDamage = playerInstance.PlayerDamage;
            var effectsToApply = playerInstance.effectsToApply;

            // ���������� ������
            for (int i = 0; i < enemy.Count; i++)
            {

                enemy[i].GetDamage(playerDamage);
                // ���������, ��� ������ j ��������� � �������� ������ ��������
                for (int j = 0; j < effectsToApply.Count; j++)
                {
                    var effectType = effectsToApply[j];
                    // ���������, ��� ������ j ��������� � �������� ������ statusEffect
                    if (statusEffect[j].EffectFloatValues.TryGetValue(effectType, out float chance))
                    {
                        // �������� ����� �� ���������� �������
                        if (Random.Range(0f, chance) == 0)
                        {
                            statusEffect[j].AddEffectByType(effectType);
                        }

                    }

                }

            }

        }

    }

}


