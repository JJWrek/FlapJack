using UnityEngine;

public class FloorMovement : MonoBehaviour
{
    public PlayerScript player;

    public float xBound;

    /// <summary>
    /// Moves the ground
    /// </summary>
    void Update()
    {
        if(transform.localPosition.x <= xBound)
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        if(!player.hasPlayerDied)
            transform.Translate(-Time.deltaTime, 0, 0);
    }
}
