using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class MovementHandler : MonoBehaviour
{
    [Header("Slope")]
    #region SlopeData
    [SerializeField] private float rayDistance;
    [SerializeField] private float maxSlope;
    [SerializeField] private RaycastHit slopeHit;
    #endregion

    [Header("Collision")]
    #region CollisionData
    [SerializeField] private Vector3 checkSize;
    [SerializeField] private Transform checkGround;
    [SerializeField] private LayerMask whatIsGround;
    #endregion

    // ������Ƽ
    public Vector3 Direction { get; set; }
    public float CurrentSpeed { get; set; }

    private bool CheckGround()
    {
        return Physics.CheckBox(checkGround.position, checkSize, Quaternion.identity, whatIsGround);
    }

    // ���� üũ
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

    // ���� ���� ����
    private Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(checkGround.position, checkSize);
    }
}
