using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
#if UNITY_EDITOR
    #region Declarations

    [Header("DEBUG INFO")]
    [SerializeField] Vector3 _input;
    [SerializeField] PlayerMovementScript movement;
    [SerializeField] Rigidbody _rb;

    [SerializeField] Vector3 facingDirection;
    public static Player _instance;

    [Header("Player Settings")]
    [SerializeField] float _energy;
    [SerializeField] float _maxEnergy;
    [SerializeField] float staminaRegeneration;
    [SerializeField] float _hp;
    [SerializeField] float _maxHp;
    [SerializeField] float hpRegeneration;

    [SerializeField] bool _controlIsEnable = true;
    [SerializeField] bool _scrollIsEnable = true;
    [SerializeField] bool _isPlayerAlive = true;

    // Inventory settings
    [SerializeField] int sellectedSlot = 0;
    [SerializeField] Skills[] skillPrefabs;
    [SerializeField] GameObject[] skills;
    [SerializeField] bool[] activeSkills = new bool[4];

    [Header("Player stats")]
    [SerializeField] float _intelect;
    [SerializeField] float _stamina;
    [SerializeField] float _strength;
    [SerializeField] float _agility;
    [SerializeField] float _vitality;

    [SerializeField] UImanager ui;


    #region Input Buttons
    KeyCode interractButton = KeyCode.E;
    KeyCode attackButton = KeyCode.Mouse0;
    KeyCode rollButton = KeyCode.LeftShift;
    KeyCode inventoryButton = KeyCode.I;
    KeyCode pause = KeyCode.Escape;

    KeyCode firstSlot = KeyCode.Alpha1;
    KeyCode secondSlot = KeyCode.Alpha2;
    KeyCode thirdSlot = KeyCode.Alpha3;
    KeyCode fourSlot = KeyCode.Alpha4;
    KeyCode[] slots;

    public int SellectedSlot { get => sellectedSlot; set => sellectedSlot = value; }
    public bool[] ActiveSkills { get => activeSkills; set => activeSkills = value; }
    public Skills[] SkillPrefabs
    {
        get { return skillPrefabs; }
        set { skillPrefabs = value; }
    }

    public float Energy { get => _energy; set => _energy = value; }
    public float MaxEnergy { get => _maxEnergy; set => _maxEnergy = value; }
    public float Intelect { get => _intelect; set => _intelect = value; }
    public float Stamina { get => _stamina; set => _stamina = value; }
    public float Strength { get => _strength; set => _strength = value; }
    public float Agility { get => _agility; set => _agility = value; }
    public float Vitality { get => _vitality; set => _vitality = value; }

    #endregion

    #endregion

#else

#endif

    #region MonoBehaviour
    private void Awake()
    {
        KeyCode[] slots = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
        _instance = this;
    }

    private void Start()
    {
        skills = new GameObject[skillPrefabs.Length];
        _rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovementScript>();
        ui = GameObject.Find("UiManager").GetComponent<UImanager>();

        for (int i = 0; i < skillPrefabs.Length; i++)
        {
            if (skillPrefabs[i] != null)
            {
                skills[i] = skillPrefabs[i].skillEffect;
            }
            else
            {
                // Debug.LogWarning("Skill prefab at index " + i + " is null.");
            }

            activeSkills[i] = false;
        }
    }
    #endregion

    private void Update()
    {
        if (_controlIsEnable && _isPlayerAlive)
        {
            movement.Movement(_input);
            GetInputs();
            AddStamina();
            AddHp();
        }
#if UNITY_EDITOR
        // Debugging();
        // Debug.Log(sellectedSlot);
#endif
    }

    void GetInputs()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && _scrollIsEnable) // forward
        {
            if (sellectedSlot + 1 < 4)
            {
                sellectedSlot += 1;
            }
            else { sellectedSlot = 0; }
            ui.SelectNextSlot();
            StartCoroutine("Scrollcd");
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && _scrollIsEnable) // backwards
        {
            if (sellectedSlot - 1 >= 0)
            {
                sellectedSlot -= 1;
            }
            else { sellectedSlot = 3; }
            ui.SelectPreviouslySlot();
            StartCoroutine("Scrollcd");
        }

        if (Input.GetKeyDown(rollButton) && (math.abs(_input.x) != 0 || math.abs(_input.z) != 0))
        {
            movement.Dash();
            // animator.SetTrigger("Roll");
        }
        if (Input.GetKeyDown(interractButton))
        {
            // Interract
            // animator.SetTrigger("Interract");

            if (!activeSkills[sellectedSlot])
            {
                activeSkills[sellectedSlot] = true;
                StartCoroutine(SkillsCd(sellectedSlot));
                var skill = Instantiate(skills[sellectedSlot]);
                skill.transform.SetParent(transform);
                if (!skill.active)
                {
                    skill.SetActive(true);

                }
                skill.transform.position = transform.position - new Vector3(0, 0.3f, 0);
                skills[sellectedSlot].GetComponent<SkillPrefab>().ApplySlot(sellectedSlot);
                // activeSkills[sellectedSlot] = true;
            }
        }
        if (Input.GetKeyDown(firstSlot))
        {
            LevelingScr._instance.AddExp(5);
            // animator.SetTrigger("Attack");
            // audio.PlayerAttackSound();
            // StartCoroutine("AttackCd");
        }
        // if (Input.GetKeyDown(inventoryButton))
        // {
        // }
        // if (Input.GetKeyDown(pause) && !paused)
        // {
        //     paused = true;
        //     gameManager.Pause();
        // }
        // else if (Input.GetKeyDown(pause) && paused)
        // {
        //     gameManager.UpPause();
        //     paused = false;
        // }

        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        SetDirection();
    }

    private void SetDirection()
    {
        if (_input.x != 0 || _input.z != 0)
        {
            facingDirection = new Vector3(_input.x, 0, _input.z);
        }
        // attackDirection = new Vector3(facingDirection.x, transform.position.y, facingDirection.z);
    }

#if UNITY_EDITOR
    void Debugging()
    {
        Debug.DrawRay(_rb.position, facingDirection * 2, Color.green);
    }
#endif

    void AddStamina()
    {
        if (_energy < _maxEnergy)
        {
            _energy += staminaRegeneration * Time.deltaTime;
        }
    }

    void AddHp()
    {
        if (_hp < _maxHp)
        {
            _hp += hpRegeneration * Time.deltaTime;
        }
    }

    IEnumerator Scrollcd()
    {
        _scrollIsEnable = false;
        yield return new WaitForSeconds(0.08f);
        _scrollIsEnable = true;
    }

    IEnumerator SkillsCd(int slotWhichWasSelected)
    {
        yield return new WaitForSeconds(skillPrefabs[slotWhichWasSelected].duration);
        ui.ReloadScripts[slotWhichWasSelected].ChangeCD(skillPrefabs[slotWhichWasSelected].cd);

        yield return new WaitForSeconds(skillPrefabs[slotWhichWasSelected].cd);
        activeSkills[slotWhichWasSelected] = false;

    }
}
