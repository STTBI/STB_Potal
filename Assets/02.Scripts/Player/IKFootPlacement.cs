using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private LayerMask IgnoreLayer;
    [Range(0,1)]
    public float DistanceToGround;

    private float weightLeftIK = 0;
    private float weightRightIK = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if(anim)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weightLeftIK);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weightLeftIK);

            RaycastHit hit;
            Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if(Physics.Raycast(ray, out hit, DistanceToGround + 2f, IgnoreLayer))
            {
                if(hit.transform.CompareTag("Walkable"))
                {
                    weightLeftIK = 1f;

                    Vector3 hitPosition = hit.point;
                    hitPosition.y += DistanceToGround;
                    anim.SetIKPosition(AvatarIKGoal.LeftFoot, hitPosition);
                    anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }
            else
            {
                weightLeftIK = 0f;
            }

            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, weightRightIK);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, weightRightIK);

            ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 2f, IgnoreLayer))
            {
                if(hit.transform.CompareTag("Walkable"))
                {
                    weightRightIK = 1f;

                    Vector3 hitPosition = hit.point;
                    hitPosition.y += DistanceToGround;
                    anim.SetIKPosition(AvatarIKGoal.RightFoot, hitPosition);
                    anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }
            else
            {
                weightRightIK = 0f;
            }
        }
    }
}
