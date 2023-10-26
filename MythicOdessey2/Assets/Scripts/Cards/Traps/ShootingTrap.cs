using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTrap : Trap
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float startTimer;
    float timer;
    [SerializeField]
    int damage;
    // Start is called before the first frame update
    void Awake()
    {
        timer = startTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime * TimeManager.Instance.currentTimeScale;
        }
        else
        {
            //Shoot
            Shoot();
            timer = startTimer;
        }
    }

    void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.damage = damage;
    }
}
