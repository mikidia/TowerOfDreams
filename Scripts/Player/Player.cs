using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields

    [Header("DEBUG INFO")]
    [SerializeField] private Vector3 _input;
    [SerializeField] private PlayerMovementScript movement;
    [SerializeField] private Rigidbody _rb;

    [SerializeField] private Vector3 facingDirection;
    public static Player _instance;

    [Header("Player Settings")]
    [SerializeField] private float _hp;
    [SerializeField] private float _maxHp;
    [SerializeField] public float hpRegeneration;
    [SerializeField] public bool VampirismIsActive = false;

    [SerializeField] private bool _controlIsEnable = true;
    [SerializeField] private bool _scrollIsEnable = true;
    [SerializeField] private bool _isPlayerAlive = true;

    // Inventory settings
    [SerializeField] private int sellectedSlot = 0;

    [Header("Skills settings")]
    [SerializeField] private Skills[] skillPrefabs;
    [SerializeField] private GameObject[] skills;

    [SerializeField] private bool[] activeSkills = new bool[4];

    [Header("Player stats")]
    [SerializeField] private float _intelect;
    [SerializeField] private float _stamina;
    [SerializeField] private float _strength;
    [SerializeField] private float _agility;
    [SerializeField] private float _vitality;

    [SerializeField] private int playerBaseDamage;
    [SerializeField] private int _playerDamage;

    [SerializeField] private UImanager ui;
    [SerializeField] private StatusEffect _statusEffect;
    [SerializeField] private FOVController fovController;
    [SerializeField] private PlayerAttack attackZone;
    [SerializeField] private GameObject castEffect;


    public List<StatusEffect.EffectType> effectsToApply = new List<StatusEffect.EffectType>();

    [Header("Child Objects")]
    [SerializeField] private Transform childObjectToRotate;
    [SerializeField] private Transform secondChildObjectToRotate;

    private string currentAnimation;

    #region Input Buttons
    private KeyCode interractButton = KeyCode.E;
    private KeyCode attackButton = KeyCode.Mouse0;
    private KeyCode rollButton = KeyCode.LeftShift;
    private KeyCode inventoryButton = KeyCode.I;
    private KeyCode pause = KeyCode.Escape;

    private KeyCode firstSlot = KeyCode.LeftShift;
    private KeyCode secondSlot = KeyCode.Alpha2;
    private KeyCode thirdSlot = KeyCode.Alpha3;
    private KeyCode fourSlot = KeyCode.Alpha4;
    private KeyCode[] slots;
    #endregion

    private bool _attackIsOnCooldown = false;
    [SerializeField] private float attackCooldown = 0.5f; // Set your desired cooldown time here

    #endregion

    #region Properties

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
    public float Hp { get => _hp; set => _hp = value; }
    public float MaxHp { get => _maxHp; set => _maxHp = value; }

    Animator animator;
    [SerializeField] GameObject attackEffectPrefab;
    #endregion

    #region MonoBehaviour Methods

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
        SetDefaultEffects();

        fovController = GetComponent<FOVController>();
        attackZone = GetComponentInChildren<PlayerAttack>();
        attackZone.gameObject.SetActive(false);
        skills = new GameObject[skillPrefabs.Length];
        _rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovementScript>();
        ui = GameObject.Find("UiManager").GetComponent<UImanager>();
        animator = GetComponent<Animator>();
        for (int i = 0; i < skillPrefabs.Length; i++)
        {
            if (skillPrefabs[i] != null)
            {
                skills[i] = skillPrefabs[i].skillEffect;
            }
            activeSkills[i] = false;
        }
    }
    public void ChangeAnimation(string animationName)
    {
        if (animationName == currentAnimation)
        {

            return;
        }
        else
        {


            animator.Play(animationName);
            currentAnimation = animationName;
        }




    }

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

    #endregion

    #region Private Methods

    private void GetInputs()
    {
        fovController.SetInput(_input);
        HandleScrollInput();
        HandleKeyPresses();
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (_input.x > 0)

        {

            ChangeAnimation("Walk Right");


        }
        else if (_input.x < 0 || _input.z != 0) { ChangeAnimation("Walk  Left"); }
        else if (_input.x == 0) { ChangeAnimation("idle"); }
    }

    private void HandleScrollInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && _scrollIsEnable) // forward
        {
            sellectedSlot = (sellectedSlot + 1) % 4;
            ui.SelectNextSlot();
            StartCoroutine(Scrollcd());
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && _scrollIsEnable) // backwards
        {
            sellectedSlot = (sellectedSlot - 1 + 4) % 4;
            ui.SelectPreviouslySlot();
            StartCoroutine(Scrollcd());
        }
    }

    private void HandleKeyPresses()
    {
        if (Input.GetKeyDown(rollButton) && (math.abs(_input.x) != 0 || math.abs(_input.z) != 0))
        {
            PlayerMovementScript.instance.UseDash();
            // ChangeAnimation("Dash");
        }
        if (Input.GetKeyDown(interractButton) && !activeSkills[sellectedSlot])
        {
            activeSkills[sellectedSlot] = true;
            // ChangeAnimation("Cast");
            try
            {
                StartCoroutine(CastSkill());

            }
            catch (IndexOutOfRangeException)
            {

                throw;
            }
        }
        if (Input.GetKeyDown(secondSlot))
        {
            LevelingScr._instance.AddExp(5);
        }
        if (Input.GetKeyDown(attackButton) && !_attackIsOnCooldown)
        {
            // ChangeAnimation("Attack");
            StartCoroutine(Attack());
        }
    }

    private void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            facingDirection = targetPosition - transform.position;
            facingDirection.y = 0;
            facingDirection.Normalize();
            RotateChildObjects(facingDirection);
        }
    }

    private void RotateChildObjects(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        if (childObjectToRotate != null)
        {
            childObjectToRotate.transform.rotation = targetRotation;
        }
        if (secondChildObjectToRotate != null)
        {
            secondChildObjectToRotate.transform.rotation = targetRotation;

        }
    }

    private void AddHp()
    {
        if (_hp < _maxHp)
        {
            _hp += hpRegeneration * Time.deltaTime;
        }
    }

    private void SetDefaultEffects()
    {
        // Initialize default effects if necessary
    }

    private IEnumerator Scrollcd()
    {
        _scrollIsEnable = false;
        yield return new WaitForSeconds(0.08f);
        _scrollIsEnable = true;
    }

    private IEnumerator CastSkill()
    {
        UpdatePlayerSkillList();
        castEffect.SetActive(true);

        var tempSpeed = movement.PlayerMoveSpeed;
        movement.PlayerMoveSpeed = tempSpeed / 2;

        yield return new WaitForSeconds(skills[sellectedSlot].GetComponent<SkillPrefab>().CastTime);
        movement.PlayerMoveSpeed = tempSpeed;
        castEffect.SetActive(false);

        var skill = Instantiate(skills[sellectedSlot]);
        skill.transform.SetParent(transform);
        if (!skill.active)
        {
            skill.SetActive(true);
        }
        skill.transform.position = transform.position - new Vector3(0, 0.3f, 0);
        StartCoroutine(SkillsCd(sellectedSlot));
    }

    private IEnumerator SkillsCd(int slotWhichWasSelected)
    {
        yield return new WaitForSeconds(skillPrefabs[slotWhichWasSelected].duration);
        ui.ReloadScripts[slotWhichWasSelected].ChangeCD(skillPrefabs[slotWhichWasSelected].cd);

        yield return new WaitForSeconds(skillPrefabs[slotWhichWasSelected].cd);
        activeSkills[slotWhichWasSelected] = false;
    }

    private IEnumerator Attack()
    {
        attackZone.gameObject.SetActive(true);
        attackEffectPrefab.SetActive(true);
        if (_input.x > 0)
        {
            attackEffectPrefab.gameObject.transform.localScale = new Vector3(
                attackEffectPrefab.transform.lossyScale.x * -1,
                attackEffectPrefab.transform.localScale.y,
                attackEffectPrefab.transform.localScale.z
            );


        }
        else
        {
            attackEffectPrefab.gameObject.transform.localScale = new Vector3(
    math.abs(attackEffectPrefab.transform.lossyScale.x),
    attackEffectPrefab.transform.localScale.y,
    attackEffectPrefab.transform.localScale.z
);

        }

        yield return new WaitForSeconds(0.1f); // Attack duration
        attackZone.gameObject.SetActive(false);
        attackEffectPrefab.SetActive(false);
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        _attackIsOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown); // Cooldown duration
        _attackIsOnCooldown = false;
    }

    public void UpdatePlayerSkillList()
    {
        var tempList = new List<GameObject>();
        foreach (var selectedSkill in skillPrefabs)
        {
            tempList.Add(selectedSkill.skillEffect);
        }
        skills = tempList.ToArray();
    }

    #endregion

    #region Public Methods

    public void HpAndStaminaMax()
    {
        _hp = _maxHp;
        movement.Energy = movement.MaxEnergy;
    }

    public void UpdatePlayerDamage()
    {
        _playerDamage = playerBaseDamage + (int)(_strength * 1.5);
    }

    #endregion
}
