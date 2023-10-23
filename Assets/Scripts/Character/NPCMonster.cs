using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Mathf;
/*
[System.Serializable]
public class NPCMove
{
    [Tooltip("NPCMove�� Ŭ���ϸ� NPC�� ������")]
    public bool NPCmove;

    public string direction; //npc�� ������ ���� ���� 
    
    [Range(1,5)]
    [Tooltip("1 = õõ��, 2 = ���� õõ��, 3 = ����, 4 = ������, 5 = ����������")]
    public int frequency; //npc�� ������ �������� �󸶳� ���� �ӵ��� ������ ���ΰ�
}
*/
public class NPCMonster : MovingObject
{
    private int random_int;
    private string direction;

    public float inter_MoveWaitTime; //���ð�, ex�������� ���� ��Ÿ��
    private float current_interMWT;

    private Vector2 playerPos;
    public string nearSound;
    private Vector2 MovePos = new Vector2(0, 0);
    private int MoveCheck = 0;

    public bool fail = false;

    //private Transform player;
    //public float speed = 5f;
    //private bool dirRU = true;
    //[SerializeField]
    //public NPCMove npc;
    // Start is called before the first frame update
    void Start()
    {
        queue = new Queue<string>();
        //StartCoroutine(MoveCoroutine());
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if(NearPlayer())
        {   
            StartCoroutine(WarningCoroutine());
            if(Mathf.Abs(playerPos.x - this.transform.position.x) > Mathf.Abs(playerPos.y - this.transform.position.y))
            {   //Debug.Log("�����̿�����111");
                if(playerPos.x > this.transform.position.x)
                    direction = "RIGHT";
                else if(playerPos.x < this.transform.position.x)
                    direction = "LEFT";
            }
            else
            {//Debug.Log("�����̿�����222");
                if(playerPos.y > this.transform.position.y)
                    direction = "UP";
                else if(playerPos.y < this.transform.position.y)
                    direction = "DOWN";
            }
            current_interMWT -= Time.deltaTime;
            if(current_interMWT <= 0)
            {
                current_interMWT = inter_MoveWaitTime;
                if(base.CheckCollision())
                    return;
                AudioManager.instance.Play(nearSound);
                base.Move(direction);
            }
            Debug.Log("����üũ��");
            Debug.Log(MoveCheck);
            if(MoveCheck == 1)
                Destroy(this.gameObject);
            else if(MoveCheck == 2)
            {
                fail = true; //���ӿ���
                Destroy(this.gameObject);
            }
        }
        else
        {
            current_interMWT -= Time.deltaTime;
        
            if(current_interMWT <= 0)
            {
                current_interMWT = inter_MoveWaitTime;

            //if(NearPlayer())
            //{
            //    Flip();
            //    return;
            //}

                RandomDirection();

                if(base.CheckCollision())
                    return;

                base.Move(direction);
            }
        }

    }
    IEnumerator MoveCoroutine()
    {   RandomDirection();
        //if(npc.direction.Length != 0)
        //{
            //for(int i = 0; i < npc.direction.Length; i++)
            //{
                yield return new WaitUntil(() => queue.Count < 2);
                base.Move(direction);
                /*
                if(i == npc.direction.Length - 1)
                {
                    i = -1;
                }
                */
            //}
        //}
    }
    private void RandomDirection()
    {
        vector.Set(0,0,vector.z);
        random_int = Random.Range(0, 4);
        switch(random_int)
        {
            case 0:
                vector.y = 1f;
                direction = "UP";
                break;
            case 1:
                vector.y = -1f;
                direction = "DOWN";
                break;
            case 2:
                vector.x = 1f;
                direction = "RIGHT";
                break;
            case 3:
                vector.x = -1f;
                direction = "LEFT";
                break;
        }
    }

    private bool NearPlayer()
    {
        playerPos = PlayerManager.instance.transform.position;
        if(Mathf.Abs(Mathf.Abs(playerPos.x) - Mathf.Abs(this.transform.position.x)) <= base.speed * walkCount * 10f)
        {
            if(Mathf.Abs(Mathf.Abs(playerPos.y) - Mathf.Abs(this.transform.position.y)) <= base.speed * walkCount * 10f)
            {
                return true;
            }
        }
        if(Mathf.Abs(Mathf.Abs(playerPos.y) - Mathf.Abs(this.transform.position.y)) <= base.speed * walkCount * 10f)
        {
            if(Mathf.Abs(Mathf.Abs(playerPos.x) - Mathf.Abs(this.transform.position.x)) <= base.speed * walkCount * 10f)
            {
                return true;
            }
        }
        return false;
    }
    IEnumerator WarningCoroutine()
    {   
        Debug.Log("�ڸ�ƾ ����");
        yield return new WaitForSeconds(3f);
        if(MovePos.x == 0)
        {
            MovePos.x = playerPos.x;
            MovePos.y = playerPos.y;
        }
        Debug.Log(MovePos.x+ "  MovePos  "+MovePos.y);
        yield return new WaitForSeconds(5f);
        Debug.Log("���� �ĺ��� �ٽ� 3�� ���, ���� ��ġ�� ��");
        Debug.Log(playerPos.x+ "  playerPos  "+playerPos.y);
        if((MovePos.x == playerPos.x)&&(MovePos.y == playerPos.y))
            MoveCheck = 1;
        else if((MovePos.x != playerPos.x)||(MovePos.y != playerPos.y))
            MoveCheck = 2;
        else
            MoveCheck = 2;
        Debug.Log(MoveCheck);

    }
}
