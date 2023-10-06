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
    [SerializeField] private bool canUpdate = true;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;
    [SerializeField] private int _maxMagic;
    [SerializeField] private int _magic;
    [SerializeField] private Material _mainMat;
    [SerializeField] private GameObject _playerModel;
    private Color _startColor;
    
    private float _interactionRadius = 2f;
    private Collider _collider;
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
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (CardMenuManager.Instance.menuOpen || !canUpdate) return;
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
        if (CardMenuManager.Instance.menuOpen || !canUpdate) return;
        _playerMovement.FixedUpdate();
    }

    public void TryInteraction()
    {
        var collisions = Physics.OverlapSphere(transform.position, _interactionRadius);
        var colList = new Dictionary<Collider, IInteracteable>();
        foreach (var col in collisions)
        {
            col.TryGetComponent(out IInteracteable interacteable);
            if(interacteable != null)
                colList.Add(col,interacteable);
        }
        Debug.Log("Colisiones de interaccion son " + colList);
        if (colList.Count > 0)
        {
            var first = colList.OrderBy(x => Vector3.Distance(x.Key.transform.position, transform.position)).First();
            first.Value.Interaction();
        }
    }

    public void EnterCannon()
    {
        _playerModel.SetActive(false);
        _collider.enabled = false;
        canUpdate = false;
    }

    public void ExitCannon(Vector3 destination)
    {
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
            collision.GetComponent<IDamageable>()?.TakeDamage(1,transform);
        }
    }
    
    
    public void TakeDamage(int damage, Transform attacker){
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
        EventManager.instance.AddAction("OnTimeChanged", (object[] args) => { _anim.speed = (float)args[0];
            _rb.velocity = Vector3.zero;
        });
        
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
        EventManager.instance.RemoveAction("OnTimeChanged", (object[] args) => { _anim.speed = (float)args[0];
            _rb.velocity = Vector3.zero;
        });
        
        EventManager.instance.RemoveAction("OnCardBuilt", (object[] args) =>
        {
            _magic -= (int) args[0];
            _playerMagicBar.SetMagic(_magic);
        });
        _mainMat.color = _startColor;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_swordTransform.position, _playerAttack.AttackRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position,_interactionRadius);
    }
}
