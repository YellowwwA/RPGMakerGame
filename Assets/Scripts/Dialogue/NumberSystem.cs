using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSystem : MonoBehaviour
{
    private AudioManager theAudio;
    public string key_sound;    //방향키 사운드
    public string enter_sound;  //결정키 사운드
    public string cancel_sound; //오답&&취소키 사운드
    public string correct_sound;//정답 사운드

    private int count;  //배열의 크기, 몇 자리수 ex.1000 -> 3
    private int selectedTextBox; //선택된 자릿수
    private int result; //플레이어가 도출해낸 값
    private int correctNumber; //정답

    private string tempNumber;

    public GameObject superObject; //자릿수에 따라 번호키를 화면 가운데 정렬을 위함
    public GameObject[] panel;
    public Text[] Number_Text;

    public Animator anim;

    public bool activated; //return new waitUntil
    private bool keyInput; //키처리 활성화, 비활성화.
    private bool correctFlag; //정답인지 아닌지 여부

    private TestNumber theTestNum;
    //private TransferMap theTransferMap;

    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        theTestNum = FindObjectOfType<TestNumber>();
        //theTransferMap = FindObjectOfType<TransferMap>();
    }

    public void ShowNumber(int _correctNumber)
    {
        correctNumber = _correctNumber;
        activated = true;
        correctFlag = false;

        string temp = correctNumber.ToString(); //정답을 문자열로 반환해서 length로 자릿수를 쉽게 계산하기 위함
        for(int i = 0; i < temp.Length; i++)
        {
            count = i;
            panel[i].SetActive(true);
            Number_Text[i].text = "0";
        }
        superObject.transform.position = new Vector3(superObject.transform.position.x + 30*count, superObject.transform.position.y, superObject.transform.position.z);
        
        selectedTextBox = 0;
        result = 0;
        SetColor();
        anim.SetBool("Appear", true);
        keyInput = true;
    }

    public bool GetResult()
    {
        return correctFlag; //나중에 다른 스크립트에서 이 함수를 불러와서 정답이 맞을시~ 틀릴시~ 코드짜면됨 
    }

    public void SetNumber(string _arrow)
    {
        int temp = int.Parse(Number_Text[selectedTextBox].text);    //선택된 자리수의 텍스트를 Integer 숫자 형식으로 "강제 형변환"
        if(_arrow == "DOWN")
        {
            if(temp == 0)
                temp = 9;
            else
                temp--;
        }
        else if(_arrow == "UP")
        {
            if(temp == 9)
                temp = 0;
            else
                temp++;
        }
        Number_Text[selectedTextBox].text = temp.ToString();
    }

    public void SetColor()
    {
        Color color = Number_Text[0].color;
        color.a = 0.3f;
        for(int i = 0; i <= count; i++)
        {
            Number_Text[i].color = color;
        }
        color.a = 1f;
        Number_Text[selectedTextBox].color = color;
    }

    // Update is called once per frame
    void Update()
    {
        if(keyInput)
        {
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                theAudio.Play(key_sound);
                SetNumber("DOWN");
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                theAudio.Play(key_sound);
                SetNumber("UP");
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                theAudio.Play(key_sound);
                if(selectedTextBox < count)
                    selectedTextBox++;
                else
                    selectedTextBox = 0;
                SetColor();
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                theAudio.Play(key_sound);
                if(selectedTextBox > 0)
                    selectedTextBox--;
                else
                    selectedTextBox = count;
                SetColor();
            }
            else if(Input.GetKeyDown(KeyCode.Z)) //결정키
            {
                theAudio.Play(key_sound);
                keyInput = false;
                StartCoroutine(OXCoroutine());
            }
            else if(Input.GetKeyDown(KeyCode.X)) //취소키
            {
                theAudio.Play(key_sound);
                keyInput = false;
                StartCoroutine(ExitCoroutine());
            }
        }
    }
    IEnumerator OXCoroutine()
    {
        Color color = Number_Text[0].color;
        color.a = 1f;

        for(int i = count; i>=0; i--)
        {
            Number_Text[i].color = color;
            tempNumber += Number_Text[i].text;  //끝번호부터 배열에 넣어야 순서대로 출력됨
        }

        yield return new WaitForSeconds(1f);

        result = int.Parse(tempNumber);

        if(result == correctNumber)
        {
            theAudio.Play(correct_sound);
            correctFlag = true;
            //theTestNum.flag = true;

            //Debug.Log("1. flag변경 완");
            //theTransferMap.PassWordCheck();

        }
        else
        {
            theAudio.Play(cancel_sound);
            correctFlag = false;
            //theTestNum.flag = false;

        }
        Debug.Log("우리가 낸 답 = "+ result + " / 정답 = "+correctNumber);
        StartCoroutine(ExitCoroutine());
    }
    IEnumerator ExitCoroutine()
    {
        result = 0;
        tempNumber = "";
        anim.SetBool("Appear", false);
        yield return new WaitForSeconds(0.1f);
        for(int i = 0; i<=count; i++)
        {
            panel[i].SetActive(false);
        }
        superObject.transform.position = new Vector3(superObject.transform.position.x - 30*count, superObject.transform.position.y, superObject.transform.position.z);

        activated = false;
    }
}
