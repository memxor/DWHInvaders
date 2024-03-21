using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    private void Start()
    {
        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 30f));
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).transform.eulerAngles = new(0, 0, 180);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Lasers(Clone)")
        {
            TakeDamage(25);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            FindObjectOfType<EnemyManager>().CheckWin();
            Destroy(gameObject);
        }
    }
}