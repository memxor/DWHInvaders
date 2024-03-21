using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int rows = 5;
    public int columns = 11;
    public float horizontalSpacing = 1.5f;
    public float verticalSpacing = 1.5f;
    public float speed = 5.0f;
    public float descentDistance = 0.5f;

    private Vector3 direction = Vector3.right;
    private float leftEdge;
    private float rightEdge;
    private float minX, maxX;

    void Start()
    {
        SpawnEnemies();
        CalculateBounds();
    }

    void Update()
    {
        MoveEnemies();

        if (transform.position.x <= leftEdge || transform.position.x >= rightEdge)
        {
            direction *= -1;
            Vector3 descent = Vector3.down * descentDistance;
            transform.position += descent;
            CalculateBounds();
        }
    }

    void MoveEnemies()
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }

    void SpawnEnemies()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = new(col * horizontalSpacing, -row * verticalSpacing, 0);
                Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], position, Quaternion.identity, transform).transform.eulerAngles = new(0, 0, 180);
            }
        }
    }

    void CalculateBounds()
    {
        Camera mainCamera = Camera.main;
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;

        minX = -screenWidth / 2;
        maxX = screenWidth / 2;

        float gridWidth = columns * horizontalSpacing;
        leftEdge = minX + transform.position.x + (gridWidth / 2);
        rightEdge = maxX + transform.position.x - (gridWidth / 2);
    }

    public void CheckWin()
    {
        if (transform.childCount == 1)
        {
            Time.timeScale = 0;
            FindObjectOfType<InGameUIManager>().OnWin();
        }
    }
}