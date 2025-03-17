using System.Collections;
using System.Collections.Generic;
using UnityEngine;
static public class Utill
{
    //손자든 증손자든 조건에 맞는 오브젝트를 찾을 때까지 재귀하는 메서드
    //나중에 제너릭으로 Transform, GameObject 둘 다 받을 수 있도록 하고싶음 (후순위)
    static public Transform FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform found = FindChildRecursive(child, name);
            if (found != null)
                return found;
        }
        return null;
    }

    //모두가 쓸 것 같은, 범용성 있는 메서드라면 자유롭게 추가해주세요!!!

}