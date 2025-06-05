using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMetadata : MonoBehaviour
{
    public enum TileType
    {
        Normal,
        FloatingPlatform,
        Obstacle,
        // agrega más si quieres
    }
    public TileType tileType;

    #if UNITY_EDITOR

    public class TileDebugGizmos : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Transform start = transform.Find("StartPoint");
            Transform end = transform.Find("NextSpawnPoint");

            if (start != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(start.position, 0.2f);
            }

            if (end != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(end.position, 0.2f);
            }
        }
    }
#endif

}
