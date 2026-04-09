using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject

{
    public bool isWeapon;
    public GameObject equippedPrefab;
    public string itemName;
    public Sprite icon;

    public bool isStackable;
    public int maxStack = 10;
}