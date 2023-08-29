using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3;

    private float _timer;

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(1000,false);
        Debug.Log(collision.gameObject.name);
        Destroy(gameObject);
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 10)
        {
            Destroy(gameObject);
        }
    }


}
