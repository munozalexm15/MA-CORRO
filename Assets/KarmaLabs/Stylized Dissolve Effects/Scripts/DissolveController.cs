using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
namespace KarmaLabs.DissolveEffect
{

public class DissolveController : MonoBehaviour
{
    public MeshRenderer Mesh;
    private Coroutine dissolveCoroutine;
    public VisualEffect VFXGraph;
    private Material[] Materials;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;



    void Start()
{
    if (Mesh != null)
    {

        Materials = Mesh.materials;

        if (Materials == null)
        {

        }
    }
    else
    {

    }
}


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dissolveCoroutine == null)
            {
                dissolveCoroutine = StartCoroutine(DissolveCo());
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetEffect();
        }
    }

    IEnumerator DissolveCo()
{
    if (Materials == null || Materials.Length == 0)
    {

        yield break;
    }

    if (VFXGraph != null)
    {
        VFXGraph.Play();
    }

    float counter = 0;
    while (Materials[0].GetFloat("_DissolveAmount") < 1)
    {
        counter += dissolveRate;
        for (int i = 0; i < Materials.Length; i++)
        {
            Materials[i].SetFloat("_DissolveAmount", counter);
        }
        yield return new WaitForSeconds(refreshRate);
    }

    dissolveCoroutine = null;
}


private void ResetEffect()
{
  if (dissolveCoroutine != null)
  {
      StopCoroutine(dissolveCoroutine);
      dissolveCoroutine = null;
  }

  if (Materials == null || Materials.Length == 0)
  {

      return;
  }

  for (int i = 0; i < Materials.Length; i++)
  {
      Materials[i].SetFloat("_DissolveAmount", 0);
  }

  if (VFXGraph != null)
  {
      VFXGraph.Stop();
      VFXGraph.Play();
  }

  dissolveCoroutine = StartCoroutine(DissolveCo());
}

}
}
