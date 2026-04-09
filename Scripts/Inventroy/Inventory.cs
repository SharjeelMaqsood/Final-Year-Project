using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [Header("Weapon Offset")]
    [SerializeField] private Vector3 weaponPositionOffset;
    [SerializeField] private Vector3 weaponRotationOffset;

    public List<InventorySlot> slots = new List<InventorySlot>();

    [SerializeField] private int maxSlots = 10;
    public Transform handTransform;
    private GameObject currentWeapon;
    public bool AddItem(ItemData item, int amount = 1)
    {
        if (slots.Count >= maxSlots)
        {
            Debug.Log("Inventory Full");
            return false;
        }

        InventorySlot newSlot = new InventorySlot
        {
            item = item,
            quantity = amount
        };

        slots.Add(newSlot);

        EquipItem(item);
        return true;
    }
    public void EquipItem(ItemData item)
    {
        if (item.equippedPrefab == null) return;

        if (currentWeapon != null)
            Destroy(currentWeapon);

        currentWeapon = Instantiate(item.equippedPrefab, handTransform);

        // Apply your hardcoded position
        currentWeapon.transform.localPosition = new Vector3(-0.08f, 0.09f, -0.03f);

        // Apply your hardcoded rotation
        currentWeapon.transform.localRotation = Quaternion.Euler(335.8434f, 322.3214f, 101.2828f);
    }

}
