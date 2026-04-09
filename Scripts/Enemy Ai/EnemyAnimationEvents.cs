using UnityEngine;
public class EnemyAnimationEvents : MonoBehaviour
{
    public EnemySwordHitbox hitbox;

    public void DealDamage()
    {
        hitbox.DealDamage();
    }

    public void ResetHit()
    {
        hitbox.ResetHit();
    }
}