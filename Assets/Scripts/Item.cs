using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int itemID;  //아이템의 고유 ID값, 중복 불가능 (ex. 50001, 50002)
    public string itemName; //아이템의 이름, 중복 가능 (ex. 고대 유물, 고대 유물)
    public string itemDescription;  //아이템 설명
    public int itemCount; //소지 개수
    public Sprite itemIcon; //아이템의 아이콘이미지
    public ItemType itemType;   //아이템 유형 열거형으로 미리 설정

    public enum ItemType
    {
        Use,
        Equip,
        Quest,
        ETC
    }

    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, int _itemCount = 1)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        itemCount = _itemCount;
        itemIcon = Resources.Load("ItemIcon/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
