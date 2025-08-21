using UnityEngine;

[CreateAssetMenu(fileName = "Item",menuName ="Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public string itemDesc; //item discription
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;//0레벨의 기준
    public int baseCount;

    public float[] damages;
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;//투사체
}
