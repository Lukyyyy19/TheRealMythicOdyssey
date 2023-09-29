using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTrap : MonoBehaviour {
    private bool _canAtack;
    [SerializeField] GameObject _spikes;
    IEnumerator ActivateTrap(IDamageable other)
    {
        yield return new WaitForSeconds(.6f);
        _spikes.SetActive(true);
        if (_canAtack)
        {
            other.TakeDamage(1,transform);
            _canAtack = false;
        }
        yield return new WaitForSeconds(1);
        _spikes.SetActive(false);
    }
    private void OnTriggerEnter(Collider other){
        _canAtack = true;
        if(other.TryGetComponent(out IDamageable damageable))
            StartCoroutine(ActivateTrap(damageable));
    }

    private void OnTriggerExit(Collider other){
        _canAtack = false;
    }
}
