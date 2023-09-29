using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable
{
    static PlayerManager _instance;

    private Rigidbody _rb;
    private Animator _anim;
    [SerializeField] private Transform _swordTransform;

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
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;
    [SerializeField] private int _maxMagic;
    [SerializeField] private int _magic;
    [SerializeField] private Material _mainMat;
    private Color _startColor;

    public Rigidbody Rb => _rb;

    //setter for isattack
    public bool IsAttackPressed { set => _isAttackPressed = value; }

    //setter for isattacking
    public bool IsAttacking { set => _isAttacking = value; get => _isAttacking; }

    //setter for isdashpressed
    public bool IsDashPressed { set => _isDashPressed = value; }

    //setter for requirenewattackpress
    public bool RequireNewAttackPress { set => _requireNewAttackPress = value; }

    public bool RequireNewDashPress { set => _requireNewDashPress = value; }

    //setter and getter for isdashing
    public bool IsDahing { set => _isDashing = value; get => _isDashing; }
    public static PlayerManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _playerMovement = new PlayerMovement(this, transform, _rb, GetComponent<Collider>());
        _playerInputs = new PlayerInputs(this);
        _playerAttack = new PlayerAttack(this, _anim, _swordTransform);
        _playerInputs.ArtificialAwake();
        _health = _maxHealth;
        _magic = _maxMagic;
        _startColor = _mainMat.color;

        _playerHealthBar.SetMaxHealth(_maxHealth);
        _playerMagicBar.SetMaxMagic(_maxMagic);
        
    }

    private void Update()
    {
        if (CardMenuManager.Instance.menuOpen) return;
        _playerMovement.Update();
        _playerAttack.Update();
        if (_isAttackPressed && !_requireNewAttackPress)
        {
            if (!_isAttacking)
            {
                StartCoroutine(_playerAttack.SpinAttack());
            }
        }

        if (_isDashPressed && !_requireNewDashPress)
        {
            _playerMovement.Dash();
        }
    }

    private void FixedUpdate()
    {
        if (CardMenuManager.Instance.menuOpen) return;
        _playerMovement.FixedUpdate();
    }

    private void OnEnable()
    {
        _playerInputs.OnEnable();
        EventManager.instance.AddAction("OnPlayerAttackFinished", (object[] args) =>
        {
            _isAttacking = false;
            _requireNewAttackPress = true;
        });
        EventManager.instance.AddAction("OnTimeChanged", (object[] args) => { _anim.speed = (float)args[0]; });
        
        EventManager.instance.AddAction("OnCardBuilt", (object[] args) =>
        {
            _magic -= (int) args[0];
            _playerMagicBar.SetMagic(_magic);
        });
    }

    private void OnDisable()
    {
        EventManager.instance.RemoveAction("OnPlayerAttackFinished", (object[] args) =>
        {
            _isAttacking = false;
            _requireNewAttackPress = true;
        });
        EventManager.instance.RemoveAction("OnTimeChanged", (object[] args) => { _anim.speed = (float)args[0]; });
    }

    // private void OnDrawGizmos(){
    //     Gizmos.color = Color.red;
    //     if(_playerAttack.debugAttack)
    //         Gizmos.DrawWireSphere(_swordTransform.position, _playerAttack.AttackRadius);
    // }
    public void TakeDamage(int damage, Transform attacker){
        _health -= damage;
        _playerHealthBar.SetHealth(_health);
        StartCoroutine(nameof(DamagedMat));
        EventManager.instance.TriggerEvent("PlayerDamaged");
    }

    public void Die()
    {
        //throw new NotImplementedException();
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
}
