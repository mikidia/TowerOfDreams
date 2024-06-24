using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelingScr : MonoBehaviour
{
    [SerializeField] private float exp;
    [SerializeField] private float totalExp;
    [SerializeField] private float maxExp;
    [SerializeField] private float level;
    [SerializeField] private Dictionary<string, int> skillLevels;
    [SerializeField] private Skills[] allSkills;
    [SerializeField] private int skillInLvlMenu;
    [SerializeField] private SkillsEvolution[] skillsToEvolute;

    public static LevelingScr _instance { get; private set; }

    public float Exp { get => exp; set => exp = value; }
    public float TotalExp { get => totalExp; set => totalExp = value; }
    public float MaxExp { get => maxExp; set => maxExp = value; }
    public float Level { get => level; set => level = value; }
    public Dictionary<string, int> SkillLevels { get => skillLevels; set => skillLevels = value; }

    [System.Serializable]
    private class SkillsEvolution
    {
        public Skills skillOne;
        public Skills skillTwo;
        public Skills skillFinish;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeSkillLevels();
    }

    private void InitializeSkillLevels()
    {
        skillLevels = new Dictionary<string, int>();
        foreach (var skill in Player._instance.SkillPrefabs)
        {
            skillLevels.Add(skill.name, 1);
        }
    }

    public void AddSkill(string name)
    {
        if (!skillLevels.ContainsKey(name))
        {
            skillLevels.Add(name, 1);
            foreach (var skill in allSkills)
            {
                if (skill.name == name)
                {
                    var skillPrefabsList = Player._instance.SkillPrefabs.ToList();
                    skillPrefabsList.Add(skill);
                    Player._instance.SkillPrefabs = skillPrefabsList.ToArray();
                    break;
                }
            }
        }
    }

    public void AddExp(float experience)
    {
        exp += experience;
        totalExp += experience;
        if (exp >= maxExp)
        {
            exp -= maxExp;
            LevelUp();

            List<int> usedIndex = new List<int>();
            Skills[] selectedSkillsForLevelUp = new Skills[skillInLvlMenu];

            for (int i = 0; i < skillInLvlMenu; i++)
            {
                int tempNumber;
                do
                {
                    tempNumber = Random.Range(0, allSkills.Length);
                } while (usedIndex.Contains(tempNumber));

                usedIndex.Add(tempNumber);
                selectedSkillsForLevelUp[i] = allSkills[tempNumber];
            }
            SkillEvolution(selectedSkillsForLevelUp);
            UImanager._instance.ShowLevelUpMenu(skillInLvlMenu, selectedSkillsForLevelUp);

        }
    }

    private void SkillEvolution(Skills[] selectedSkillsForLevelUp)
    {
        Player player = GetComponent<Player>();

        foreach (var evolution in skillsToEvolute)
        {
            bool hasSkillOne = player.SkillPrefabs.Contains(evolution.skillOne);
            bool hasSkillTwo = player.SkillPrefabs.Contains(evolution.skillTwo);


            if (true)
            {
                selectedSkillsForLevelUp[0] = evolution.skillFinish;
                return;
            }
        }
    }

    public void UpdatePlayerStats()
    {
        var playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovementScript>();
        var playerMain = GameObject.Find("Player").GetComponent<Player>();

        if (playerMovementScript != null)
        {
            playerMovementScript.MaxEnergy = playerMain.Stamina * 10;
        }
        playerMain.HpAndStaminaMax();
        Player._instance.UpdatePlayerSkillList();
    }

    private void LevelUp()
    {
        level = totalExp switch
        {
            <= 7 => 1,
            <= 16 => 2,
            <= 27 => 3,
            <= 40 => 4,
            <= 55 => 5,
            <= 72 => 6,
            <= 91 => 7,
            <= 112 => 8,
            <= 135 => 9,
            <= 160 => 10,
            <= 187 => 11,
            <= 216 => 12,
            <= 247 => 13,
            <= 280 => 14,
            <= 315 => 15,
            <= 352 => 16,
            <= 394 => 17,
            <= 441 => 18,
            <= 493 => 19,
            <= 550 => 20,
            <= 612 => 21,
            <= 679 => 22,
            <= 751 => 23,
            <= 828 => 24,
            <= 910 => 25,
            <= 997 => 26,
            <= 1089 => 27,
            <= 1186 => 28,
            <= 1288 => 29,
            <= 1395 => 30,
            <= 1507 => 31,
            <= 1628 => 32,
            <= 1758 => 33,
            <= 1897 => 34,
            <= 2045 => 35,
            <= 2202 => 36,
            <= 2368 => 37,
            <= 2543 => 38,
            _ => level
        };
        maxExp = (level * level + 1) * 7;  // Example formula to calculate maxExp dynamically
    }
}
