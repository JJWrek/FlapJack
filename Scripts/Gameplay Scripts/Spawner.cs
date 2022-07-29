using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public PlayerScript player;

    public GameObject spikes;
    public float timerMin, timerMax;
    public float yBoundMin, yBoundMax;

    private bool startSpawning;

    /// <summary>
    /// This starts spawning the spikes
    /// </summary>
    void Update()
    {
        if (player.hasGameStarted && !startSpawning)
        {
            StartCoroutine(SpawnSpike());
            startSpawning = true;
        }

        ChangeSpeedSpawner();
    }

    /// <summary>
    /// This helps spawn the spikes in a random order inside the set y axis
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnSpike()
    {
        yield return new WaitForSeconds(Random.Range(timerMin, timerMax));

        Instantiate(spikes, new Vector2(transform.position.x, Random.Range(yBoundMin, yBoundMax)), Quaternion.identity);

        StartCoroutine(SpawnSpike());
    }
    /// <summary>
    /// Changes the speed that the spikes spawn as the game gets faster
    /// </summary>
    void ChangeSpeedSpawner()
    {
        if(player.score == 20)
        {
            timerMax = 1.7f;
            timerMin = 1.1f;
        }
        if(player.score == 100)
        {
            timerMax = 1.4f;
            timerMin = 0.8f;
        }
        if(player.score == 300)
        {
            timerMax = 1.1f;
            timerMin = 0.5f;
        }
    }
}
