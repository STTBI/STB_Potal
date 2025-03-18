using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    // FirePortal(int portalID, Vector3 pos, Vector3 dir, float distance)
    public Action<int, Vector3, Vector3, float> firePortal; 
    private PlayerStateSystem stateSystem;

    private void Awake()
    {
        stateSystem = GetComponentInParent<PlayerStateSystem>();
    }

    [SerializeField] private float fireDelay;
    private float curDelay = 0f;

    private void Update()
    {
        curDelay -= Time.deltaTime;
    }

    public void Fire(int portalID, Vector3 pos, Vector3 dir, float distance)
    {
        stateSystem.SetFloat("FireDelay", curDelay);

        if (curDelay < 0f)
        {
            curDelay = fireDelay;
            firePortal?.Invoke(portalID, pos, dir, distance);
        }
    }

}
