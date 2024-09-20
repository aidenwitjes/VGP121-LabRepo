using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float minXClamp = 0.0f;
    public float maxXClamp = 84.6f;
    public float minYClamp = 0.0f;
    public float maxYClamp = 71.5f;

    //This function always runs after fixed update - unity specifies that this is where camera movement should happen
    private void LateUpdate()
    {
        PlayerController pc = GameManager.Instance.PlayerInstance;
        Vector3 cameraPos = transform.position;

        cameraPos.x = Mathf.Clamp(pc.transform.position.x, minXClamp, maxXClamp);
        cameraPos.y = Mathf.Clamp(pc.transform.position.y, minYClamp, maxYClamp);

        transform.position = cameraPos;
    }
}
