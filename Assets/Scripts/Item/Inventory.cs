using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private DatabaseManager theDatabase;
    private OrderManager theOrder;
    private AudioManager theAudio;
    private OkOrCancel theOOC;
    private Equipment theEquip;

    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;
    public string itemdrink_sound;

    private InventorySlot[] slots;  //�κ��丮 ���Ե�

    private List<Item> inventoryItemList;   //�÷��̾ ������ ������ ����Ʈ
    private List<Item> inventoryTabList;    //������ �ǿ� ���� �ٸ��� ������ ������ ����Ʈ

    public Text Description_Text; //�ο�����
    public string[] tabDescription; //�� �ο� ����

    public Transform tf;    //slot�� �θ�ü

    public GameObject go; //�κ��丮 Ȱ��ȭ ��Ȱ��ȭ
    public GameObject[] selectedTabImages;
    public GameObject go_OOC; //������ Ȱ��ȭ ��Ȱ��ȭ
    public GameObject prefab_Floating_Text;

    private int selectedItem;   //���õ� ������
    private int selectedTab; //���õ� ��

    private int page; //�κ��丮 ������ �߰�
    private int slotCount; //Ȱ��ȭ�� ������ ����
    private const int MAX_SLOTS_COUNT = 12; //�ִ� ���� ����

    private bool activated; //�κ��丮 Ȱ��ȭ�� true
    private bool tabActivated; //�� Ȱ��ȭ�� true
    private bool itemActivated; //������ Ȱ��ȭ�� true
    private bool stopKeyInput; //Ű�Է� ���� (�Һ��� �� ���ǰ� ���� �ٵ�, �� �� Ű�Է� ����)
    private bool preventExec; //�ߺ����� ����

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        theEquip = FindObjectOfType<Equipment>();
        theOOC = FindObjectOfType<OkOrCancel>();
        theOrder = FindObjectOfType<OrderManager>();
        theAudio = FindObjectOfType<AudioManager>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<InventorySlot>();
        /*
        inventoryItemList.Add(new Item(10001, "���� ����", "ü���� 50 ȸ�������ִ� ������ ����", Item.ItemType.Use));
        inventoryItemList.Add(new Item(10002, "�Ķ� ����", "������ 15 ȸ�������ִ� ������ ����", Item.ItemType.Use));
        inventoryItemList.Add(new Item(10003, "���� ���� ����", "ü���� 350 ȸ�������ִ� ������ ���� ����", Item.ItemType.Use));
        inventoryItemList.Add(new Item(10004, "���� �Ķ� ����", "������ 80 ȸ�������ִ� ������ ���� ����", Item.ItemType.Use));
        inventoryItemList.Add(new Item(11001, "���� ����", "�������� ������ ���´�. ���� Ȯ���� ��", Item.ItemType.Use));
        inventoryItemList.Add(new Item(20001, "ª�� ��", "�⺻���� ����� ��", Item.ItemType.Equip));
        inventoryItemList.Add(new Item(21001, "�����̾� ����", "1�п� ���� 1�� ȸ�������ִ� ���� ����", Item.ItemType.Equip));
        inventoryItemList.Add(new Item(30001, "��� ������ ���� 1", "������ �ɰ��� ��� ������ ����", Item.ItemType.Quest));
        inventoryItemList.Add(new Item(30002, "��� ������ ���� 2", "������ �ɰ��� ��� ������ ����", Item.ItemType.Quest));
        inventoryItemList.Add(new Item(30003, "��� ����", "��� ������ �����ִ� ����� ����", Item.ItemType.Quest));*/
    }

    public List<Item> SaveItem()
    {
        return inventoryItemList;
    }

    public void LoadItem(List<Item> _itemList)
    {
        inventoryItemList = _itemList;
    }

    public void EquipToInventory(Item _item)
    {
        inventoryItemList.Add(_item);
    }

    public void GetAnItem(int _itemID, int _count = 1)
    {
        for(int i = 0; i<theDatabase.itemList.Count; i++)   //�����ͺ��̽� ������ �˻�
        {
            if(_itemID == theDatabase.itemList[i].itemID)   //�����ͺ��̽��� ������ �߰�
            {
                var clone = Instantiate(prefab_Floating_Text, PlayerManager.instance.transform.position, Quaternion.Euler(Vector3.zero)); //������ ����, ������ ��ġ,����
                clone.GetComponent<FloatingText>().text.text = theDatabase.itemList[i].itemName + " " + _count + "�� ȹ�� +";
                clone.transform.SetParent(this.transform);

                for(int j = 0; j<inventoryItemList.Count; j++)  //����ǰ�� ���� �������� �ִ��� �˻�
                {
                    if(inventoryItemList[j].itemID == _itemID)  //����ǰ�� ���� �������� �ִ� -> ������ ����
                    {
                        if(inventoryItemList[j].itemType == Item.ItemType.Use)
                        {
                            inventoryItemList[j].itemCount += _count;
                            return;
                        }
                        else
                        {
                            inventoryItemList.Add(theDatabase.itemList[i]);
                        }
                        return;
                    }
                }
                inventoryItemList.Add(theDatabase.itemList[i]); //�������� ���� �������϶��� ����ǰ�� �ش� ������ �߰�
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;  //������ ������ �߰�
                return;
            }
        }
        Debug.LogError("�����ͺ��̽��� �ش� ID���� ���� �������� �������� �ʽ��ϴ�.");   //�����ͺ��̽��� ItemID ����
    }

    public void ShowTab()   //�� Ȱ��ȭ
    {
        RemoveSlot();
        SelectedTab();
    }
    public void RemoveSlot()    //�κ��丮 ���� �ʱ�ȭ
    {
        for(int i = 0; i<slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    }

    public void SelectedTab()   //���õ� ���� �����ϰ� �ٸ� ��� ���� �÷� ���İ� 0���� ����
    {
        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        for(int i = 0; i<selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        Description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    }

    IEnumerator SelectedTabEffectCoroutine()    //���õ� �� ��¦�� ȿ��
    {
        while(tabActivated)
        {
            Color color = selectedTabImages[0].GetComponent<Image>().color;
            while(color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while(color.a>0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void ShowItem()  //������ Ȱ��ȭ(inventoryTabList�� ���ǿ� �´� �����۵鸸 �־��ְ�, �κ��丮 ���Կ� ���)
    {
        inventoryTabList.Clear();
        RemoveSlot();
        selectedItem = 0;
        page = 0;

        //�ǿ� ���� ������ �з�, �װ��� �κ��丮 �� ����Ʈ�� �߰�
        switch(selectedTab) 
        {
            case 0:
                for(int i = 0; i< inventoryItemList.Count; i++)
                {
                    if(Item.ItemType.Use == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 1:
                for(int i = 0; i< inventoryItemList.Count; i++)
                {
                    if(Item.ItemType.Equip == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 2:
                for(int i = 0; i< inventoryItemList.Count; i++)
                {
                    if(Item.ItemType.Quest == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 3:
                for(int i = 0; i< inventoryItemList.Count; i++)
                {
                    if(Item.ItemType.ETC == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
        }
        //�κ��丮 �� ����Ʈ�� ������, �κ��丮 ���Կ� �߰�
        ShowPage();
        SelectedItem();
    }

    public void ShowPage()
    {
        slotCount = -1;

        for(int i = page* MAX_SLOTS_COUNT; i<inventoryTabList.Count; i++)
        {
            slotCount = i - (page* MAX_SLOTS_COUNT);
            slots[slotCount].gameObject.SetActive(true);
            slots[slotCount].Additem(inventoryTabList[i]);

            if(slotCount == MAX_SLOTS_COUNT - 1)
                break;
        }
    }

    public void SelectedItem()  //���õ� �������� �����ϰ�, �ٸ� ��� ���� �÷� ���İ��� 0���� ����
    {
        StopAllCoroutines();
        if(slotCount> -1)
        {
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            for(int i = 0; i<= slotCount; i++)
                slots[i].selected_Item.GetComponent<Image>().color = color;
            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
            Description_Text.text = "�ش� Ÿ���� �������� �����ϰ� ���� �ʽ��ϴ�.";
    }

    IEnumerator SelectedItemEffectCoroutine()   //���õ� ������ ��¦�� ȿ��
    {
        while(itemActivated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            while(color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while(color.a>0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!stopKeyInput)
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                activated = !activated;

                if(activated)
                {
                    theAudio.Play(open_sound);
                    theOrder.NotMove();
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = false;
                    ShowTab();
                }
                else
                {
                    theAudio.Play(cancel_sound);
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    theOrder.Move();
                }
            }

            if(activated)
            {
                if(tabActivated)    //�� Ȱ��ȭ�� Ű �Է� ó��
                {
                    if(Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if(selectedTab < selectedTabImages.Length - 1)
                            selectedTab++;
                        else
                            selectedTab = 0;
                        theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    else if(Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if(selectedTab >0)
                            selectedTab--;
                        else
                            selectedTab = selectedTabImages.Length - 1;
                        theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    else if(Input.GetKeyDown(KeyCode.Z))
                    {
                        theAudio.Play(enter_sound);
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                        itemActivated = true;
                        tabActivated = false;
                        preventExec = true;
                        ShowItem();

                    }
                }
                else if(itemActivated)  //������ Ȱ��ȭ�� Ű �Է� ó��
                {
                    if(inventoryTabList.Count>0)
                    {
                        if(Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if(selectedItem + 2 > slotCount)
                            {
                                if(page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT)
                                    page++;
                                else
                                    page = 0;
                                RemoveSlot();
                                ShowPage();
                                selectedItem = -2;
                            }

                            if(selectedItem < slotCount - 1)
                                selectedItem += 2;
                            else
                                selectedItem %= 2;
                            theAudio.Play(key_sound);
                            SelectedItem();
                                
                        }
                        else if(Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if(selectedItem - 2 < 0)
                            {
                                if(page != 0)
                                    page--;
                                else
                                    page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;
                                RemoveSlot();
                                ShowPage();
                            }

                            if(selectedItem > 1)
                                selectedItem -= 2;
                            else
                                selectedItem = slotCount - selectedItem;
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if(Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if(selectedItem + 1 > slotCount)
                            {
                                if(page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT)
                                    page++;
                                else
                                    page = 0;
                                RemoveSlot();
                                ShowPage();
                                selectedItem = -1;
                            }

                            if(selectedItem < slotCount)
                                selectedItem++;
                            else
                                selectedItem = 0;
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if(Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if(selectedItem - 1 < 0)
                            {
                                if(page != 0)
                                    page--;
                                else
                                    page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;
                                RemoveSlot();
                                ShowPage();
                            }

                            if(selectedItem > 0)
                                selectedItem--;
                            else
                                selectedItem = slotCount;
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if(Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {
                            if(selectedTab == 0)    //�Ҹ�ǰ
                            {
                                //������ ���ǰų�? ���� ���� ������ ȣ��
                                StartCoroutine(OOCCoroutine("���", "���"));
                            }
                            else if(selectedTab == 1)
                            {
                                //��� ����
                                //��� �����Ұų�? ���� ���� ������ ȣ��
                                StartCoroutine(OOCCoroutine("����", "���"));
                            }
                            else//������ ���
                            {
                                theAudio.Play(beep_sound);
                            }
                        }
                    }
                    
                    if(Input.GetKeyDown(KeyCode.X))
                    {
                        theAudio.Play(cancel_sound);
                        StopAllCoroutines();
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab();
                    }
                }   //������ Ȱ��ȭ�� Ű�Է� ó��

                if(Input.GetKeyUp(KeyCode.Z))   //�ߺ� ���� ����
                    preventExec = false;
            }
        }
    }

    IEnumerator OOCCoroutine(string _up, string _down)
    {
        theAudio.Play(enter_sound);
        stopKeyInput = true;

        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice(_up, _down);
        yield return new WaitUntil(() => !theOOC.activated);
        if(theOOC.GetResult())
        {
            for(int i = 0; i<inventoryItemList.Count; i++)
            {
                if(inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {
                    if(selectedTab == 0)
                    {
                        theDatabase.UseItem(inventoryItemList[i].itemID);
                        if(inventoryItemList[i].itemCount>1)
                            inventoryItemList[i].itemCount--;
                        else
                            inventoryItemList.RemoveAt(i);
                        
                        theAudio.Play(itemdrink_sound);
                        ShowItem();
                        break;
                    }
                    else if(selectedTab == 1)
                    {
                        theEquip.EquipItem(inventoryItemList[i]);
                        inventoryItemList.RemoveAt(i);
                        ShowItem();
                        break;
                    }

                }
            }
        }
        stopKeyInput = false;
        go_OOC.SetActive(false);
    }
}
