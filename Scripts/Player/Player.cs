using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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

    [SerializeField] float _hp;
    [SerializeField] float _maxHp;
    [SerializeField] float hpRegeneration;

    [SerializeField] bool _controlIsEnable = true;
    [SerializeField] bool _scrollIsEnable = true;
    [SerializeField] bool _isPlayerAlive = true;

    // Inventory settings
    [SerializeField] int sellectedSlot = 0;
    [Header("Skills setings ")]
    [SerializeField] Skills[] skillPrefabs;
    [SerializeField] GameObject[] skills;

    [SerializeField] bool[] activeSkills = new bool[4];

    [Header("Player stats")]
    [SerializeField] float _intelect;
    [SerializeField] float _stamina;
    [SerializeField] float _strength;
    [SerializeField] float _agility;
    [SerializeField] float _vitality;

    [SerializeField] int playerBaseDamage;
    [SerializeField] int _playerDamage;

    [SerializeField] UImanager ui;
    [SerializeField] StatusEffect _statusEffect;
    [SerializeField] FOVController fovController;
    [SerializeField] PlayerAttack attackZone;

    public List<StatusEffect.EffectType> effectsToApply = new List<StatusEffect.EffectType>();



    [Header("Child Objects")]
    [SerializeField] Transform childObjectToRotate;

    [SerializeField] Transform secondChildObjectToRotate;



    #region Input Buttons
    KeyCode interractButton = KeyCode.E;
    KeyCode attackButton = KeyCode.Mouse0;
    KeyCode rollButton = KeyCode.LeftShift;
    KeyCode inventoryButton = KeyCode.I;
    KeyCode pause = KeyCode.Escape;

    KeyCode firstSlot = KeyCode.LeftShift;
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


    public float Intelect { get => _intelect; set => _intelect = value; }
    public float Stamina { get => _stamina; set => _stamina = value; }
    public float Strength { get => _strength; set => _strength = value; }
    public float Agility { get => _agility; set => _agility = value; }
    public float Vitality { get => _vitality; set => _vitality = value; }
    public int PlayerDamage { get => _playerDamage; set => _playerDamage = value; }

    #endregion

    #endregion

#else

#endif

    #region MonoBehaviour
    private void Awake()
    {
        slots = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
        _instance = this;
    }

    private void Start()
    {
        UpdatePlayerDamage();
        _statusEffect = GetComponent<StatusEffect>();

        UpdatePlayerDamage();

        // Установка стандартных эффектов
        SetDefaultEffects();


        fovController = GetComponent<FOVController>();
        attackZone = GetComponentInChildren<PlayerAttack>();
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
            RotateTowardsMouse();
            movement.Movement(_input);
            GetInputs();
            AddHp();

        }
#if UNITY_EDITOR
        // Debugging();
        // Debug.Log(sellectedSlot);
#endif
    }


    void GetInputs()
    {

        fovController.SetInput(_input);
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
            PlayerMovementScript.instance.UseDash();
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
        if (Input.GetKeyDown(secondSlot))
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
        if (Input.GetKeyDown(attackButton) && !_attackIsOnCooldown)
        {

            StartCoroutine(Attack());
        }

        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);

            Vector3 facingDirection = targetPosition - transform.position;
            facingDirection.y = 0; // Убедимся, что направление параллельно земле

            facingDirection.Normalize();
            RotateChildObjects(facingDirection);
        }
    }

    private void RotateChildObjects(Vector3 direction)
    {
        if (childObjectToRotate != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            childObjectToRotate.transform.rotation = targetRotation;
        }

        if (secondChildObjectToRotate != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            secondChildObjectToRotate.transform.rotation = targetRotation;
        }
    }

#if UNITY_EDITOR
    void Debugging()
    {
        Debug.DrawRay(_rb.position, facingDirection * 2, Color.green);
    }
#endif



    void AddHp()
    {
        if (_hp < _maxHp)
        {
            _hp += hpRegeneration * Time.deltaTime;
        }
    }
    private void SetDefaultEffects()
    {
        effectsToApply.Add(StatusEffect.EffectType.Bleeding);
        effectsToApply.Add(StatusEffect.EffectType.Stun);

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

    public void UpdatePlayerDamage()
    {
        _playerDamage = playerBaseDamage + (int)(_strength * 1.5);
    }

    private bool _attackIsOnCooldown = false;
    [SerializeField] private float attackCooldown = 0.5f; // Set your desired cooldown time here

    IEnumerator Attack()
    {
        attackZone.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f); // Attack duration
        attackZone.gameObject.SetActive(false);
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        _attackIsOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown); // Cooldown duration
        _attackIsOnCooldown = false;
    }
}
