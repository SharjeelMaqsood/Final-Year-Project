using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;
    public int amount = 1;

    private bool hasPickedUp = false;

    public void Pickup(Inventory inventory)
    {
        if (hasPickedUp) return;
        hasPickedUp = true;

        bool added = inventory.AddItem(item, amount);

        if (added)
        {
            if (item.isWeapon)
            {
                PlayerCombat combat = inventory.GetComponent<PlayerCombat>();

                if (combat != null)
                {
                    combat.EquipWeapon();
                }
            }

            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer != null) renderer.enabled = false;

            this.enabled = false;

            Destroy(gameObject, 0.1f);
        }
    }
}