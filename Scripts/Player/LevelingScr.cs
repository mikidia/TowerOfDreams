using UnityEngine;

public class LevelingScr : MonoBehaviour
{
    [SerializeField] float exp;
    [SerializeField] float totalExp;


    [SerializeField] float maxExp;
    [SerializeField] float level;
    public static LevelingScr _instance;

    public float Exp { get => exp; set => exp = value; }
    public float TotalExp { get => totalExp; set => totalExp = value; }
    public float MaxExp { get => maxExp; set => maxExp = value; }
    public float Level { get => level; set => level = value; }

    private void Awake()
    {
        if (_instance == null)
        {

            _instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddExp(float experience)
    {
        exp += experience;
        totalExp += experience;
        if (exp >= maxExp)
        {

            exp -= maxExp;
            LevelUp(totalExp);
            //GameObject.Find("UiManager").GetComponent<UImanager>().ExpBar();







        }
    }
    void LevelUp(float experience)
    {
        switch (totalExp)
        {


            case <= 7:
                level = 1;
                maxExp = 7;
                break;
            case <= 16:
                level = 2;
                maxExp = 16;
                break;
            case <= 27:
                level = 3;
                maxExp = 27;
                break;
            case <= 40:
                level = 4;
                maxExp = 40;
                break;
            case <= 55:
                level = 5;
                maxExp = 55;
                break;
            case <= 72:
                level = 6;
                maxExp = 72;
                break;
            case <= 91:
                level = 7;
                maxExp = 91;
                break;
            case <= 112:
                level = 8;
                maxExp = 112;
                break;
            case <= 135:
                level = 9;
                maxExp = 135;
                break;
            case <= 160:
                level = 10;
                maxExp = 160;
                break;
            case <= 187:
                level = 11;
                maxExp = 187;
                break;
            case <= 216:
                level = 12;
                maxExp = 216;
                break;
            case <= 247:
                level = 13;
                maxExp = 247;
                break;
            case <= 280:
                level = 14;
                maxExp = 280;
                break;
            case <= 315:
                level = 15;
                maxExp = 315;
                break;
            case <= 352:
                level = 16;
                maxExp = 352;
                break;
            case <= 394:
                level = 17;
                maxExp = 394;
                break;
            case <= 441:
                level = 18;
                maxExp = 441;
                break;
            case <= 493:
                level = 19;
                maxExp = 493;
                break;
            case <= 550:
                level = 20;
                maxExp = 550;
                break;
            case <= 612:
                level = 21;
                maxExp = 612;
                break;
            case <= 679:
                level = 22;
                maxExp = 679;
                break;
            case <= 751:
                level = 23;
                maxExp = 751;
                break;
            case <= 828:
                level = 24;
                maxExp = 828;
                break;
            case <= 910:
                level = 25;
                maxExp = 910;
                break;
            case <= 997:
                level = 26;
                maxExp = 997;
                break;
            case <= 1089:
                level = 27;
                maxExp = 1089;
                break;
            case <= 1186:
                level = 28;
                maxExp = 1186;
                break;
            case <= 1288:
                level = 29;
                maxExp = 1288;
                break;
            case <= 1395:
                level = 30;
                maxExp = 1395;
                break;
            case <= 1507:
                level = 31;
                maxExp = 1507;
                break;
            case <= 1628:
                level = 32;
                maxExp = 1628;
                break;
            case <= 1758:
                level = 33;
                maxExp = 1758;
                break;
            case <= 1897:
                level = 34;
                maxExp = 1897;
                break;
            case <= 2045:
                level = 35;
                maxExp = 2045;
                break;
            case <= 2202:
                level = 36;
                maxExp = 2202;
                break;
            case <= 2368:
                level = 37;
                maxExp = 2368;
                break;
            case <= 2543:
                level = 38;
                maxExp = 2368;
                break;







        }




    }
}
