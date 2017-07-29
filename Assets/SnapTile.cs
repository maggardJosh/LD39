using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapTile : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
            return;
        transform.position = Snap(transform.position);

    }

    public static Vector3 Snap(Vector3 pos)
    {
        pos.x = Snap(pos.x, GameSettings.Instance.TileSize);
        pos.y = Snap(pos.y, GameSettings.Instance.TileSize);
        pos.z = Snap(pos.z, GameSettings.Instance.TileSize);
        return pos;

    }
    public static float Snap(float value, float tileSize)
    {
        tileSize /= 100f;
        return Mathf.RoundToInt((value) / tileSize) * tileSize;
    }
}
