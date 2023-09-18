using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTrap : MonoBehaviour
{
    [SerializeField] GameObject _spikes;
    IEnumerator ActivateTrap()
    {
        yield return new WaitForSeconds(.6f);
        _spikes.SetActive(true);
        yield return new WaitForSeconds(1);
        _spikes.SetActive(false);
    }
    private void OnCollisionEnter(Collision other)
    {
        StartCoroutine(ActivateTrap());
    }
}
