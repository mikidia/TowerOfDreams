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
            // ����������� ��������� �����
            elapsedTime += Time.deltaTime;
            print(elapsedTime);

            // ������������ ���������� fillAmount �� ������ ���������� ������� � ����� ������������
            float fillDecrease = elapsedTime / duration;
            print(fillDecrease);
            // ��������� fillAmount
            image.fillAmount -= fillDecrease * Time.deltaTime;

            yield return null;
        }

        // ����� ���� ��� fillAmount ��������� ��� ������ ������ 0, ���������� ��� � 1 � ������������ GameObject
        image.fillAmount = 1;
        gameObject.SetActive(false);
    }

}
