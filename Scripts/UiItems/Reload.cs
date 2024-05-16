using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reload : MonoBehaviour
{
    Image image;
    float duration;





    public void ChangeCD(float cd)
    {
        image = GetComponent<Image>();
        duration = cd;
        this.gameObject.SetActive(true);
        StartCoroutine("Reloading");

    }

    IEnumerator Reloading()
    {
        float elapsedTime = 0f;

        while (image.fillAmount > 0)
        {
            // Увеличиваем прошедшее время
            elapsedTime += Time.deltaTime;
            print(elapsedTime);

            // Рассчитываем уменьшение fillAmount на основе прошедшего времени и общей длительности
            float fillDecrease = elapsedTime / duration;
            print(fillDecrease);
            // Уменьшаем fillAmount
            image.fillAmount -= fillDecrease * Time.deltaTime;

            yield return null;
        }

        // После того как fillAmount достигнет или станет меньше 0, сбрасываем его в 1 и деактивируем GameObject
        image.fillAmount = 1;
        gameObject.SetActive(false);
    }

}
