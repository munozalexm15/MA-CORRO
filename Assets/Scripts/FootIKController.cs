using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FootIKController : MonoBehaviour
{
    public LayerMask groundLayer;
    public float footOffset = 0.1f;
    public float raycastDistance = 1.5f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            AdjustFootPosition(AvatarIKGoal.LeftFoot);
            AdjustFootPosition(AvatarIKGoal.RightFoot);
        }
    }

    void AdjustFootPosition(AvatarIKGoal foot)
    {
        Vector3 footPosition = animator.GetIKPosition(foot);
        Ray ray = new Ray(footPosition + Vector3.up * 0.5f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, groundLayer))
        {
            Vector3 targetPosition = hit.point + Vector3.up * footOffset;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);

            animator.SetIKPositionWeight(foot, 1f);
            animator.SetIKRotationWeight(foot, 1f);
            animator.SetIKPosition(foot, targetPosition);
            animator.SetIKRotation(foot, targetRotation);
        }
        else
        {
            animator.SetIKPositionWeight(foot, 0);
            animator.SetIKRotationWeight(foot, 0);
        }
    }
}
