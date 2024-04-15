using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable
{
    static PlayerManager _instance;

    private Rigidbody _rb;
    private Animator _anim;

    [SerializeField] private EnemyChecker _enemyChecker;

    [SerializeField] private PlayerHealthBar _playerHealthBar;
    [SerializeField] private PlayerMagicBar _playerMagicBar;


    [SerializeField] private PlayerMovement _playerMovement;

    [SerializeField] private PlayerAttack _playerAttack;
    private PlayerInputs _playerInputs;
    public Vector3 dir;
    [SerializeField] private bool _isAttackPressed;
    [SerializeField] private bool _isDashPressed;
    [SerializeField] private bool _requireNewAttackPress = true;
    [SerializeField] private bool _requireNewDashPress = true;
    [SerializeField] private bool _isDashing;
    [SerializeField] private bool _isAttacking;
    [SerializeField] private bool canUpdate = true;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;
    [SerializeField] private int _maxMagic;
    public float _magic;
    [SerializeField] private Material _mainMat;
    [SerializeField] private GameObject _playerModel;
    [SerializeField] public GameObject hitVfx;
    private Color _startColor;

    private float _interactionRadius = 2f;
    private Collider _collider;

    [SerializeField] private Transform _spawnOnHandPoint;
    [SerializeField] private bool _hasHandsOccupied;
    private CardsTypeSO _lastCard;

    [SerializeField] private bool _isInsideCannon;
    private bool _hasMana;

    [SerializeField] private ParticleSystem _slashGO;

    private bool _isChargingMana;

    private bool _canAttack = true;

   [SerializeField] private EnemyStateMachine _nearEnemy;
    public EnemyStateMachine NearEnemy => _nearEnemy;
    public bool HasMana => _hasMana;
    public Rigidbody Rb => _rb;

    public bool RootMotion
    {
        set => _anim.applyRootMotion = value;
    }

    //setter for isattack
    public bool IsAttackPressed
    {
        set => _isAttackPressed = value;
    }

    //setter for isattacking
    public bool IsAttacking
    {
        set => _isAttacking = value;
        get => _isAttacking;
    }

    //setter for isdashpressed
    public bool IsDashPressed
    {
        set => _isDashPressed = value;
    }

    //setter for requirenewattackpress
    public bool RequireNewAttackPress
    {
        set => _requireNewAttackPress = value;
    }

    public bool RequireNewDashPress
    {
        set => _requireNewDashPress = value;
    }

    //setter and getter for isdashing
    public bool IsDahing
    {
        set => _isDashing = value;
        get => _isDashing;
    }

    public bool HasHandsOccupied
    {
        set => _hasHandsOccupied = value;
        get => _hasHandsOccupied;
    }

    public static PlayerManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _playerMovement = new PlayerMovement(this, transform, _rb, GetComponent<Collider>());
        _playerInputs = new PlayerInputs(this);
        _playerAttack = new PlayerAttack(this, _anim, transform, hitVfx, _slashGO);
        _playerInputs.ArtificialAwake();
        _health = _maxHealth;
        _magic = _maxMagic;
        _startColor = _mainMat.color;

        _playerHealthBar.SetMaxHealth(_maxHealth);
        _playerMagicBar.SetMaxMagic(_maxMagic);
        _collider = GetComponent<Collider>();
        _hasMana = true;
    }

    private void Update()
    {
        // if(_isInsideCannon)
        // {
        //     if (_isAttackPressed && !_requireNewAttackPress)
        //     {
        //         if (!_isAttacking)
        //         {
        //             _currentCannon.Shoot();
        //             _isInsideCannon = false;
        //         }
        //     }
        // }
        // if (Input.GetButtonDown("Fire1"))
        // {
        //     CardMenuManager.Instance.cardTemp.TriggerInstantiateEvent();
        // }

        if ( /*CardMenuManager.Instance.menuOpen ||*/ !canUpdate) return;
        _playerAttack.Update();
        if (_isAttackPressed && !_requireNewAttackPress)
        {
            if (!_isAttacking && _canAttack)
            {
                bool isCardTriggered = false;
                if (CardMenuManager.Instance.cardTemp.IsSword)
                {
                StartCoroutine(_playerAttack.SpinAttack());
                _canAttack = false;
                }
                else
                {
                    CardMenuManager.Instance.TriggerCard();
                    isCardTriggered = true;
                }
                Invoke(nameof(CanAttackAgain),.66f);
            }
        }

        //
        // else
        // {
        //     _swordTransform.gameObject.SetActive(false);
        //     if (CardMenuManager.Instance.menuOpen)
        //     {
        //         if (_isAttackPressed && !_requireNewAttackPress)
        //         {
        //             if (!_isAttacking)
        //             {
        //                 EventManager.instance.TriggerEvent("OnObjectTrigger", _lastCard);
        //                 Destroy(built);
        //                 _hasHandsOccupied = false;
        //             }
        //         }
        //     }
        // }

        //  var rotation = Quaternion.LookRotation(Helper.GetMouseWorldPosition(), Vector3.up);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 960f * Time.deltaTime);
        _playerMovement.Update();

        _nearEnemy = _enemyChecker.CheckForEnemies(10f);
        if (_isDashPressed && !_requireNewDashPress)
        {
            _playerMovement.Dash();
        }
    }

    void CanAttackAgain()
    {
        _canAttack = true;
    }
    
    private void FixedUpdate()
    {
        if ( /*CardMenuManager.Instance.menuOpen ||*/ !canUpdate) return;
        _playerMovement.FixedUpdate();
    }

    public void TryInteraction()
    {
        var collisions = Physics.OverlapSphere(transform.position, _interactionRadius);
        var colList = new Dictionary<Collider, IInteracteable>();
        foreach (var col in collisions)
        {
            col.TryGetComponent(out IInteracteable interacteable);
            if (interacteable != null)
                colList.Add(col, interacteable);
        }

        if (colList.Count > 0)
        {
            var first = colList.OrderBy(x => Vector3.Distance(x.Key.transform.position, transform.position)).First();
            first.Value.Interaction();
        }
    }

    private Cannon _currentCannon;

    public void EnterCannon(Vector3 pos, Cannon cannon)
    {
        EventManager.instance.TriggerEvent("HideCardPanel");
        transform.position = pos;
        _playerModel.SetActive(false);
        _rb.velocity = Vector3.zero;
        _collider.enabled = false;
        canUpdate = false;
        _isInsideCannon = true;
        _currentCannon = cannon;
    }

    public void ExitCannon(Vector3 destination)
    {
        _isInsideCannon = false;
        _collider.enabled = true;
        Vector3 maxRange = new Vector3(17, 0, 17);
        destination = Vector3.ClampMagnitude(destination, 17);
        _playerModel.SetActive(true);
        canUpdate = true;
        transform.position = destination;
        var collisions = Physics.OverlapSphere(transform.position, 3);
        foreach (var collision in collisions)
        {
            if (collision.CompareTag("Player")) continue;
            collision.GetComponent<IDamageable>()?.TakeDamage(1, transform);
        }
        EventManager.instance.TriggerEvent("ShowCardPanel");
    }


    public void TakeDamage(int damage, Transform attacker)
    {
        _health -= damage;
        _playerHealthBar.SetHealth(_health);
        StartCoroutine(nameof(DamagedMat));
        EventManager.instance.TriggerEvent("PlayerDamaged");
        if (_health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        LevelManager.instance.LoadScene("GameOver");
    }

    IEnumerator DamagedMat()
    {
        _mainMat.SetFloat("Smoothness", 0f);
        _mainMat.color = Color.black;
        yield return new WaitForSeconds(.075f);
        _mainMat.color = Color.white;
        yield return new WaitForSeconds(.1f);
        _mainMat.SetFloat("Smoothness", 0.5f);
        _mainMat.color = _startColor;
    }

    private void OnEnable()
    {
        _playerInputs.OnEnable();
        EventManager.instance.AddAction("OnPlayerAttackFinished", (object[] args) =>
        {
            _isAttacking = false;
            _requireNewAttackPress = true;
        });
        EventManager.instance.AddAction("OnTimeChanged", (object[] args) =>
        {
            _anim.speed = (float)args[0];
            _rb.velocity = Vector3.zero;
        });

        EventManager.instance.AddAction("OnCardBuilt", (object[] args) =>
        {
            if (_magic <= 0)
            {
                _magic = 0;
                _hasMana = false;
            }
            else
            {
                _magic -= (int)args[0];
                _playerMagicBar.SetMagic(_magic);
            }

            CardMenuManager.Instance.CheckManaCost(_magic);
        });

        EventManager.instance.AddAction("OnEnemyKilled", (object[] args) =>
        {
            _hasMana = true;
            _magic += (int)args[0];
            if (_magic >= _maxMagic)
                _magic = _maxMagic;
            _playerMagicBar.SetMagic(_magic);

            CardMenuManager.Instance.CheckManaCost(_magic);
        });
        EventManager.instance.AddAction("GamePaused", (x) =>
        {
            _playerInputs.DisablePI();
        });EventManager.instance.AddAction("GameResumed", (x) =>
        {
            _playerInputs.EnablePI();
        });
        EventManager.instance.AddAction("OnCardTrigger", (x) => { CreateObjOnHand((CardsTypeSO)x[0]); });
    }


    private void OnDisable()
    {
        EventManager.instance.RemoveAction("OnPlayerAttackFinished", (object[] args) =>
        {
            _isAttacking = false;
            _requireNewAttackPress = true;
        });
        EventManager.instance.RemoveAction("OnTimeChanged", (object[] args) =>
        {
            _anim.speed = (float)args[0];
            _rb.velocity = Vector3.zero;
        });

        EventManager.instance.RemoveAction("OnCardBuilt", (object[] args) =>
        {
            
            _magic -= (int)args[0];
            if (_magic <= 0)
            {
                _magic = 0;
                _hasMana = false;
            }
            _playerMagicBar.SetMagic(_magic);
            CardMenuManager.Instance.CheckManaCost(_magic);
        });
        EventManager.instance.RemoveAction("OnEnemyKilled", (object[] args) =>
        {
            _hasMana = true;
            _magic += (int)args[0];
            if (_magic >= _maxMagic)
                _magic = _maxMagic;
            _playerMagicBar.SetMagic(_magic);

            CardMenuManager.Instance.CheckManaCost(_magic);
        });
        _mainMat.color = _startColor;
    }

    private GameObject built;

    private void CreateObjOnHand(CardsTypeSO prefab)
    {
        // built = Instantiate(prefab.miniObject, _spawnOnHandPoint.position, Quaternion.identity);
        // built.transform.parent = transform;
        _hasHandsOccupied = true;
        _lastCard = prefab;
    }

    public void ChargeMana(float mana)
    {
        _magic += mana;
        if (_magic >= _maxMagic)
            _magic = _maxMagic;
        _playerMagicBar.SetMagic(_magic);
        CardMenuManager.Instance.CheckManaCost(_magic);
        _isChargingMana = true;
    }

    public bool CheckMana(int manaCost)
    {
        return manaCost <= _magic;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _playerAttack.AttackRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _interactionRadius);
    }
}