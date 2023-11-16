using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class Cannon : Trap, IInteracteable
{
    private bool _loaded;
    private Transform _charge;
    [SerializeField] private Transform _cannonModel;
    [SerializeField] private GameObject _smokeParticles;
    [SerializeField] private ParticleSystem _explosionParticles;
    [SerializeField] private ParticleSystem _explosionParticles1;
    [SerializeField] private ParticleSystem _explosionParticles2;
    [SerializeField] private ParticleSystem _explosionParticles3;
    [SerializeField]private MeshRenderer _meshRenderer;
    [SerializeField]private Material[] _matArray;
    private Material _mainMat;
    private float _timer;
    private float _maxTimer = 5f;

    private void Awake()
    {
        _mainMat = _meshRenderer.material;
    }

    public void Interaction()
    {
        if (!_loaded)
        {
            Reload();
        }
        else
        {
            Shoot();
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime * TimeManager.Instance.currentTimeScale;
        if (_timer >= _maxTimer)
        {
            DestroyCannon();
        }

        if (!_loaded) return;
        var rotation = Quaternion.LookRotation(PlayerManager.Instance.dir, Vector3.up);
        _cannonModel.rotation = Quaternion.RotateTowards(_cannonModel.rotation, rotation, 960 * Time.deltaTime);
        //_cannonModel.LookAt(Helper.GetMouseWorldPosition());
        //_cannonModel.eulerAngles = new Vector3(0, _cannonModel.localEulerAngles.y - 75, 0);
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Shoot();
        // }
    }

    public void DesInteraction()
    {
        throw new System.NotImplementedException();
    }

    private void Reload()
    {
        _loaded = true;
        PlayerManager.Instance.EnterCannon(transform.position,this);
        Debug.Log("Canon recargado");
        StartCoroutine(nameof(DamagedMat));
    }

    public void Shoot()
    {
        _loaded = false;
        PlayerManager.Instance.ExitCannon(_cannonModel.forward*3f);
        _explosionParticles.Play();
        _explosionParticles1.Play();
        _explosionParticles2.Play();
        _explosionParticles3.Play();
        DestroyCannon();
    }

    void DestroyCannon()
    {
        EventManager.instance.TriggerEvent("OnTrapDestroyed", gridPosition);
        //Destroy(Instantiate(_smokeParticles, transform.position + new Vector3(2, 0, 2), quaternion.identity), 2f);
        Destroy(gameObject);
        if (_loaded)
        {
            PlayerManager.Instance.ExitCannon(transform.position);
            _loaded = false;
        }
    }

    private void OnEnable()
    {
       // EventManager.instance.AddAction("ShootCannon",(x)=>Shoot());
    }
    IEnumerator DamagedMat()
    {
        for (int i = 0; i < 3; i++)
        {
            
        _meshRenderer.material = _matArray[0];
        yield return new WaitForSeconds(.075f);
        _meshRenderer.material = _matArray[1];
        yield return new WaitForSeconds(.1f);
        _meshRenderer.material = _mainMat;
        yield return new WaitForSeconds(1f);
        }
    }
}
