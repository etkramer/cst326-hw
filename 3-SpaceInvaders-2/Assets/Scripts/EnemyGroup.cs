using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public GameObject[] rowPrefabs;
    public int numCols = 11;

    public float moveSpeed = 1f;
    public float moveSpeedVertical = 0.5f;

    [HideInInspector]
    public Bounds bounds;

    void Start()
    {
        for (int i = 0; i < rowPrefabs.Length; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                GameObject obj = Instantiate(rowPrefabs[i], transform);
                obj.transform.position =
                    transform.position + new Vector3((j - numCols / 2) * 0.75f, i * -0.75f, 0);
            }
        }
    }

    void Update()
    {
        RecalculateBounds();

        // Did we touch a wall?
        if (
            (moveSpeed > 0 && bounds.max.x >= GameManager.Instance.Bounds.max.x)
            || (moveSpeed < 0 && bounds.min.x <= GameManager.Instance.Bounds.min.x)
        )
        {
            // Flip and move down
            moveSpeed *= -1;
            transform.position += new Vector3(0, -moveSpeedVertical, 0);
        }

        // Move left/right
        if (GameManager.Instance.isPlayerAlive)
        {
            transform.position +=
                Vector3.right * moveSpeed * GameManager.Instance.GameSpeed * Time.deltaTime;
        }
    }

    public void RecalculateBounds()
    {
        var colliders = GetComponentsInChildren<BoxCollider2D>();

        bounds = new Bounds(transform.position, Vector3.one);
        foreach (var collider in colliders)
        {
            bounds.Encapsulate(collider.bounds);
        }
    }
}
