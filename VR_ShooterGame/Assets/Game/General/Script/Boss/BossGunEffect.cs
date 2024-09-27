using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGunEffect : MonoBehaviour
{
    [SerializeField] private BossBehaviorStateManager bossBehaviorStateManager;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private TrailRenderer tracerEffect;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private Transform raycastDestination;
    private Ray ray;
    private RaycastHit hitInfo;

    private void OnEnable()
    {
        bossBehaviorStateManager.OnAttack += CastFireEffect;
    }

    private void OnDisable()
    {
        bossBehaviorStateManager.OnAttack -= CastFireEffect;
    }

    public void CastFireEffect()
    {
        PlayMuzzleFlash();

        SetupRay();
        
        TrailRenderer tracer = CreateBulletTracer();

        if (Physics.Raycast(ray, out hitInfo))
        {
            tracer.transform.position = hitInfo.point;
        }
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Emit(1);
    }

    private void SetupRay()
    {
        ray.origin = raycastOrigin.position;

        GameObject aimtarget = bossBehaviorStateManager.player.transform.GetChild(3).gameObject;
        
        ray.direction = aimtarget.transform.position- raycastOrigin.position;
    }
    
    private TrailRenderer CreateBulletTracer()
    {
        var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        tracer.AddPosition(ray.origin);

        return tracer;
    }

    public void SetRaycastOrigin(Transform newRaycastOrigin)
    {
        raycastOrigin = newRaycastOrigin;
    }

    public void SetGunEffect(ParticleSystem newMuzzleEffect, ParticleSystem newHitEffect)
    {
        muzzleFlash = newMuzzleEffect;
    }
}
