using UnityEngine;
using UnityEngine.Rendering;

public class BallSpawner : MonoBehaviour
{
    public GameObject spawnVolume;
    public GameObject ballPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // Get volume bounds
            var volumeTransform = spawnVolume.GetComponent<Transform>();
            var volumeMinX = volumeTransform.position.x - volumeTransform.localScale.x / 2;
            var volumeMaxX = volumeTransform.position.x + volumeTransform.localScale.x / 2;

            // Pick random position within bounds
            var spawnPosX = Random.Range(volumeMinX, volumeMaxX);
            var spawnPos = new Vector3(
                spawnPosX,
                volumeTransform.position.y,
                volumeTransform.position.z
            );

            // Create new ball instance at pos
            Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        }
    }
}
