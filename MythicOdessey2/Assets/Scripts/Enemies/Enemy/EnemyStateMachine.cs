using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using DG.Tweening;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour, IDamageable
{
    [SerializeField] protected float _speed;
    [Range(0, 10), SerializeField] protected float _chaseRange;
    [Range(0, 10), SerializeField] protected float _attackRange;
    [SerializeField] protected float _stunedTime;
    private float _maxAttackRange;
    protected float _attackDamage;
    private float _maxChaseRange;
    [SerializeField] protected float _health;
    [SerializeField] protected float _maxHealth;
    //[SerializeField] floatingHealthBar _healtBar;
    protected Animator _anim;
    [SerializeField] protected bool _isPlayerInRange, _isPlayerInAttackRange, _stopChasing, _lookAtPlayer;
    [SerializeField] protected bool _isEnemyWithSword;
    private bool _damageTaken;
    protected EnemyFactoryState _factory;

    protected EnemyBaseState _currentState;
    protected EnemyBaseState _currentSubState;
    private EnemyManager _enemyManager;
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;

    //private AudioSource _audio;

    [SerializeField] private Material _currentMat;
    private Color _initalColor;


    //[SerializeField] private VisualEffect _bloodSplash;

    public Rigidbody Rb => _rb;

    public EnemyBaseState CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }

    public EnemyBaseState CurrentSubState
    {
        get => _currentSubState;
        set => _currentSubState = value;
    }

    public bool StopChasing
    {
        get => _stopChasing;
        set => _stopChasing = value;
    }

    public bool CanLookAtPlayer => _lookAtPlayer;
    public bool IsPlayerInRange => _isPlayerInRange;
    public bool IsPlayerInAttackRange => _isPlayerInAttackRange;
    public bool IsEnemyWithSword => _isEnemyWithSword;
    public float Speed => _speed;
    public float AttackRange => _attackRange;
    public float AttackDamage => _attackDamage;
    public float Health => _health;
    public float MaxHealth => _maxHealth;
    public Animator Anim => _anim;
    
    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    private void Awake()
    {
        _health = _maxHealth;
        _rb = GetComponent<Rigidbody>();
        _maxAttackRange = _attackRange;
        _maxChaseRange = _chaseRange;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        //_audio = GetComponent<AudioSource>();
        //_healtBar = GetComponentInChildren<floatingHealthBar>();
        // if (TryGetComponent(out MeshRenderer mr))
        // {
        //     _currentMat = mr.material;
        // }
        // else
        // {
        //     _currentMat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        // }
        // _initalColor = _currentMat.GetColor("_TexColor");

    }

    private void Start()
    {
        //_anim = GetComponent<Animator>();
        _enemyManager = GetComponent<EnemyManager>();
        _factory = _enemyManager.States;
        _factory.SetContext(this);
        _currentState = _factory.Root();
        _currentState.EnterState();
    }

    private void Update()
    {
       _navMeshAgent.speed = TimeManager.Instance.currentTimeScale == 1? 3.5f : 3.5f * TimeManager.Instance.currentTimeScale;
        _currentState.UpdateStates();
        var playerPosition = PlayerManager.Instance.transform.position;
       // _isPlayerInRange = Vector3.Distance(transform.position, playerPosition) <= _chaseRange;
        //_isPlayerInAttackRange = Vector3.Distance(transform.position, playerPosition) <= _attackRange;
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }

    public void LookAtPlayer()
    {
        // Vector3 targetPostition = new Vector3(PlayerManager.instance.transform.position.x,
        //     this.transform.position.y,
        //     PlayerManager.instance.transform.position.z);
        // this.transform.LookAt(targetPostition);
        transform.LookAt(PlayerManager.Instance.gameObject.transform);
    }

    //
    // public void TakeDamage(int damage,bool wasAttacked)
    // {
    //     _health -= damage;
    //     //_healtBar.UpdateHealtBar(_health, _maxHealth);
    //    // var blood = Instantiate(_bloodSplash, transform.position + Vector3.up * 1.5f, quaternion.identity);
    //     _attackRange = 0;
    //     _chaseRange = 0;
    //     _currentMat.SetColor("_TexColor", Color.white);
    //     _currentMat.SetFloat("_Smoothness", 0);
    //     _currentMat.DOColor(Color.black, "_TexColor", .1f).OnComplete(() => _currentMat.DOColor(_initalColor, "_TexColor", .1f));
    //
    //
    //
    //     // _currentMat.DOFloat(0, "_LerpTime", .2f);
    //     //_currentMat.DOColor(Color.red, "_TexColor", 1);
    //     //_audio.Play();
    //     Invoke(nameof(Unstunt), _stunedTime);
    //     //Destroy(blood.gameObject, 1f);
    //     if (_health <= 0) Die();
    //
    // }
    public void TakeDamage(int damage){
        //if(_damageTaken)return;
        Debug.Log("Enemy took damage");
        _health -= damage;
        _rb.AddForce((PlayerManager.Instance.transform.position - transform.position)*-1*5,ForceMode.Impulse);
        Debug.Log(_health);
        if (_health <= 0)
        {
            Die();
        }
    }

    void Unstunt()
    {
        _attackRange = _maxAttackRange;
        _chaseRange = _maxChaseRange;
        _currentMat.SetFloat("_Smoothness", 0.5f);
    }



    public void Die()
    {
        //gameObject.SetActive(false);
        EventManager.instance.TriggerEvent("CheckEnemies");
        Destroy(gameObject);
    }
    private void OnEnable(){
        EventManager.instance.AddAction("OnPlayerAttackFinished", (object[] args) => {
            _damageTaken = false;
            _rb.velocity = Vector3.zero;
        });
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}