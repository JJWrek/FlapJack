using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;

    public float offSetX = 0.5f;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    /// <summary>
    /// Camera follows the player
    /// </summary>
    void Update()
    {
        transform.position = new Vector3(player.position.x + offSetX, 0, transform.position.z);
    }
}
