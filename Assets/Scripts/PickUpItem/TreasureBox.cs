using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        GameManager.instance.uiLevelUp.Show();
        gameObject.SetActive(false);
        GameManager.instance.pool.CallOnActive<Coin>(coin => coin.MagnetEnable());
        
    }
}
