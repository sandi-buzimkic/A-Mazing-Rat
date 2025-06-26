using UnityEngine;

public class LifePickup : Pickup
{
   protected override void Collect(GameObject player)
    {

        player.GetComponent<Mouse>().currentLives++;
        player.GetComponent<Mouse>().UpdateUI();
        Destroy(gameObject);
    }
}
