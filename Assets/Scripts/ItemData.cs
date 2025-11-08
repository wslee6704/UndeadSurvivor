using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc; //item discription
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;//0레벨의 기준
    public int baseCount;
    public float baseSpeed;
    public float baseBulletSpeed;//무기 자체 날아가는 속도

    public float[] damages;
    public int[] counts;
    public float[] bulletSpeeds;

    [Header("# Weapon")]
    public GameObject projectile;//투사체
    public Sprite hand;

    public string GetDescription(int level)
    {
        List<object> parameters = new List<object>();

        if (damages != null && damages.Length > 0)
        {
            float dmg = (level < damages.Length) ? damages[level] : 0;
            parameters.Add(dmg);
        }

        if (counts != null && counts.Length > 0)
        {
            int cnt = (level < counts.Length) ? counts[level] : 0;
            parameters.Add(cnt);
        }

        if (bulletSpeeds != null && bulletSpeeds.Length > 0)
        {
            float spd = (level < bulletSpeeds.Length) ? bulletSpeeds[level] : 0;
            parameters.Add(spd);
        }

        // {키워드} + optional 단위 처리
        // 2. {0}, {1} 처리
        string result = Regex.Replace(itemDesc, @"\{(\d+)\}(%?)", match =>
        {
            int index = int.Parse(match.Groups[1].Value); // 0, 1, 2...
            string unit = match.Groups[2].Value;          // % 있으면 "%"

            if (index < parameters.Count)
            {
                object val = parameters[index];
                if (unit == "%")
                {
                    if (val is float f) return Mathf.RoundToInt(f * 100).ToString() + "%";
                    if (val is double d) return Mathf.RoundToInt((float)d * 100).ToString() + "%";
                }
                return val.ToString() + unit; // 그냥 단위 붙여줌
            }

            return match.Value; // 값이 없으면 원문 유지
        });

        return result;
    }
    
}
