using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3;

    private float _timer;

    [HideInInspector]
    public int damage;

    [SerializeField] int speed;
    // void Awake()
    // {
    //     Destroy(gameObject, life);
    // }

    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageable>()?.TakeDamage(damage);
        Debug.Log(other.name);
        Destroy(gameObject);
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 10)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.up * speed * Time.deltaTime * TimeManager.Instance.currentTimeScale);
    }


}
