using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class MovementHandler : MonoBehaviour
{
    [Header("Slope")]
    #region SlopeData
    [SerializeField] private float rayDistance;
    [SerializeField] private float maxSlope;
    protected RaycastHit slopeHit;
    #endregion

    [Header("Collision")]
    #region CollisionData
    [SerializeField] private Vector3 boxSize;
    //[SerializeField] private float radius;
    [SerializeField] private Transform checkGround;
    [SerializeField] private LayerMask whatIsGround;
    #endregion

    // 프로퍼티
    public Vector2 Direction { get; set; }
    public float CurrentSpeed { get; set; }

    public bool CheckGround()
    {
        return Physics.CheckBox(checkGround.position, boxSize, Quaternion.identity, whatIsGround);
        //return Physics.CheckSphere(checkGround.position, radius, whatIsGround);
    }

    // 경사면 체크
    public bool IsOnSlope()
    {
        Ray ray = new Ray(checkGround.position, Vector3.down);
        if(Physics.Raycast(ray, out slopeHit, rayDistance, whatIsGround))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlope;
        }

        return false;
    }

    // 방향 벡터 추출
    public Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(checkGround.position, boxSize);
    }
}
