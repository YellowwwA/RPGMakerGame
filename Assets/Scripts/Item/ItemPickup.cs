using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    
    public int itemID;
    public int _count;
    public string pickUpSound;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.instance.Play(pickUpSound);
            Inventory.instance.GetAnItem(itemID, _count);//인벤토리 추가
            Destroy(this.gameObject);
        }
    }
}
