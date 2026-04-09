using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();

        if (health != null)
        {
            health.ChangeHealth(damage);
        }
    }
}