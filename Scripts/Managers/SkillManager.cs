using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager _instance;
    GameObject[] activeSkills = new GameObject[0];
    GameObject[] skills;

    public GameObject[] Skills { get => skills; set => skills = value; }

    void Start()
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

}
