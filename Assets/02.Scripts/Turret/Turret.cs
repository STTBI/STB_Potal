using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5f;


    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position; // Ÿ�� ��ġ�� ȸ��
            direction.y = 0f; // y���� ȸ�� ����

            Quaternion targetRotaion = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotaion, rotationSpeed * Time.deltaTime);
        }
    }
}
