using System;
using System.Collections;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] Transform player;          // objetivo a mirar
    [SerializeField] Transform finalMarker;     // pos. final deseada
    [SerializeField] float duration = 1f;       // segundos
    [SerializeField] AnimationCurve ease =      // curva de suavizado
        AnimationCurve.EaseInOut(0,0,1,1);
    [Header("Framing")]
    [SerializeField] float finalLookUp = 1.5f;
    [Header("Framing")]
    [SerializeField] float startLookUp = 1.5f;   // → igual que lookUpInit del CameraController

    public void BeginOrbit() => StartCoroutine(Orbit());

    IEnumerator Orbit()
    {
        var cam = GetComponent<CameraController>();
        
        yield return new WaitForSeconds(cam.startupLerpTime);
        
        cam.rotationEnabled = false;   // ← desactiva YA (así no pisa el LookAt)
        cam.canMove         = false;   // opcional, evita que la cámara se desplace mientras orbita

        /* ——— Órbita original ——— */
        Vector3 startOff = transform.position - player.position;
        Vector3 endOff   = finalMarker.position - player.position;

        float startAng = Mathf.Atan2(startOff.z, startOff.x);
        float endAng   = Mathf.Atan2(endOff.z, endOff.x);
        float startRad = new Vector2(startOff.x, startOff.z).magnitude;
        float endRad   = new Vector2(endOff.x, endOff.z).magnitude;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float k   = ease.Evaluate(t / duration);
            float ang = Mathf.LerpAngle(startAng * Mathf.Rad2Deg,
                endAng   * Mathf.Rad2Deg, k) * Mathf.Deg2Rad;
            float rad = Mathf.Lerp(startRad, endRad, k);
            float y   = Mathf.Lerp(startOff.y, endOff.y, k);

            transform.position = player.position +
                                 new Vector3(Mathf.Cos(ang) * rad, y, Mathf.Sin(ang) * rad);

            Vector3 lookOffset = Vector3.up *
                                 Mathf.Lerp(startLookUp, finalLookUp, k);
            transform.LookAt(player.position + lookOffset);

            yield return null;
        }

        cam.canMove = true;                              // vuelve a seguir al jugador (sin rotar)
        player.GetComponent<StateController>().canChangeLanes = true;
    }
}
