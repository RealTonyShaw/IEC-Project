using System;
using UnityEngine;

public class FireBallEffectHandler : ISpecialEffectHandler
{
    public void CreateDestroyEffect(Collider other, Missile missile, GameObject prefab)
    {
        if (prefab != null)
        {
            Vector3 fwd = missile.transform.forward;
            if (other != null)
                switch (other.gameObject.layer)
                {
                    case Layer.Unit:
                        fwd = (missile.transform.position - other.transform.position).normalized;
                        break;
                    case Layer.Missile:
                        fwd = (missile.transform.position - other.transform.position).normalized;
                        break;
                    default:
                        RaycastHit hit;
                        Ray ray = new Ray(missile.transform.position - 0.2f * missile.transform.forward, missile.transform.forward);
                        if (Physics.Raycast(ray, out hit, 0.25f, 0x1))
                        {
                            fwd = hit.normal;
                        }
                        break;
                }
            Quaternion rot = Quaternion.LookRotation(fwd);
            Gamef.Instantiate(prefab, missile.transform.position, rot);
        }
    }

    public void CreateSpawnEffect(Unit caster, Missile missile, GameObject prefab)
    {
        if (prefab != null)
            Gamef.Instantiate(prefab, caster.transform.position, caster.transform.rotation);
    }
}
