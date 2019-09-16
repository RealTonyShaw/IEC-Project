using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MissileHitBasicHandler : IMissileHitHandler
{
    public static void MissileCollisionHandler(Missile m1, Missile m2)
    {
        m1.TakeDamage(m2.Damage);
        m2.TakeDamage(m1.Damage);
        if (!m1.IsAlive && m1.Skill.Data.IsAOE)
        {
            m1.Blast(m2);
        }
        if (!m2.IsAlive && m2.Skill.Data.IsAOE)
        {
            m2.Blast(m1);
        }
    }


    public void Fade(Missile self)
    {
        if (self.Skill.Data.IsAOE)
        {
            self.Blast(null);
        }
    }

    public void HitMissile(Missile self, Missile other)
    {
        if (self.ID < other.ID)
        {
            MissileCollisionHandler(self, other);
        }
    }

    public void HitTerrain(Missile self)
    {
        if (self.Skill.Data.IsAOE)
        {
            self.Blast(null);
        }
        self.TakeDamage(1e7f);
    }

    public void HitUnit(Missile self, Unit unit)
    {
        if (self.Skill.Data.IsAOE)
        {
            self.Blast(null);
        }
        else
        {
            Gamef.Damage(self.Damage, DamageType.unset, self.Caster, unit);
        }
        self.TakeDamage(1e7f);
    }
}
