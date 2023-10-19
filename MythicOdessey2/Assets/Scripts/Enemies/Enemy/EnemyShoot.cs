using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
    [SerializeField] private Transform _target;
    [SerializeField] private float _shootDistance = 10;
    private float timer = 0;

    void Update()
    {
        ShootTimer();
        if (Vector3.Distance(transform.position, _target.position) < _shootDistance && timer == 0)
        {
            bulletSpawnPoint.LookAt(_target);
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        void ShootTimer()
        {
            timer += Time.deltaTime;
            if (timer >= 1.7f)
            {
                timer = 0;
            }
        }
    }
}
