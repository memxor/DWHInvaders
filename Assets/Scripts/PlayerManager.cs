using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 20.0f;
    public int health = 100;
    public Slider healthSlider;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(horizontalInput * speed * Time.deltaTime * Vector2.right);

        var viewPos = Camera.main.WorldToViewportPoint(transform.position);
        viewPos.x = Mathf.Clamp(viewPos.x, 0.05f, 0.95f);
        transform.position = Camera.main.ViewportToWorldPoint(viewPos);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = Vector3.up * bulletSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Enemy_Bear(Clone)" || other.gameObject.name == "Enemy_Eth(Clone)") GameOver();

        if (other.gameObject.name == "Bullet_Bear(Clone)" || other.gameObject.name == "Bullet_Eth(Clone)")
        {
            Destroy(other.gameObject);
            health -= 5;
            healthSlider.value = health;
            if (health <= 0) GameOver();
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        FindObjectOfType<InGameUIManager>().OnLose();
    }
}