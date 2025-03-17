using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjGravity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 게임 전체 중력 설정 (기본 중력 값)
        Physics.gravity = new Vector3(0, -9.81f, 0);
    }

}
