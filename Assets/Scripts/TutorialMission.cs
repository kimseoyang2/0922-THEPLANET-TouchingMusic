using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialMission : MonoBehaviour
{
    public GameObject player;
    public GameObject missionText;
    public GameObject Check;
    public GameObject soundNote;
    public GameObject levelLoader;

    public AudioClip clickSound;
    public AudioClip appearSound;
    public AudioSource click;
    public AudioSource appear;

    private float missionNum;
    float missionTime;
    float checktime;

    bool success;
    
    // Start is called before the first frame update
    void Start()
    {
        MoveStraight();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMission();
        success = Check.GetComponent<Toggle>().isOn;

        if (missionNum == 1 && success == true)
        {
            Invoke("TurnLeft", 1.5f);
            //appear.PlayOneShot(appearSound);
            success = false;
        }
        else if (missionNum == 2 && success == true)
        {
            Invoke("TurnRight", 1.5f);
            success = false;
        }
        else if (missionNum == 3 && success == true)
        {
            Invoke("MoveRotate", 1.5f);
            success = false;
        }
        else if (missionNum == 4 && success == true)
        {
            Invoke("Jump", 1.5f);
            success = false;
        }
        else if (missionNum == 5 && success == true)
        {
            Invoke("MakeSound", 1.5f);
            success = false;
        }
        else if (missionNum == 6 && success == true)
        {
            Invoke("SaveSound", 1.5f);
            success = false;
        }
        else if (missionNum == 7 && success == true)
        {
            Invoke("PlaySound", 1.5f);
            success = false;
        }
        else if (missionNum == 8 && success == true)
        {
            Invoke("StartGame", 1.5f);
            success = false;
        }
        else if (missionNum == 9 && success == true)
        {
            levelLoader.SetActive(true);
            Invoke("LoadScene", 2.0f);
        }

    }

    //미션의 종류
    //1. 앞으로 이동하라
    void MoveStraight()
    {
        missionText.GetComponent<Text>().text = "앞으로 3초동안 걸어봐요.";
        
        checktime = 0;
        missionNum = 1;
        Check.GetComponent<Toggle>().isOn = false;
    }
    //2. 제자리 왼쪽으로 회전하라
    void TurnLeft()
    {
        missionText.GetComponent<Text>().text = "왼쪽으로 돌아봐요.";

        checktime = 0;
        missionNum = 2;
        Check.GetComponent<Toggle>().isOn = false;
    }
    //3. 제자리 오른쪽으로 회전하라
    void TurnRight()
    {
        missionText.GetComponent<Text>().text = "오른쪽으로 돌아봐요.";

        checktime = 0;
        missionNum = 3;
        Check.GetComponent<Toggle>().isOn = false;
    }
    //4. 앞으로 가면서 회전하라
    void MoveRotate()
    {
        missionText.GetComponent<Text>().text = "앞으로 가면서 자유자재로!";

        checktime = 0;
        missionNum = 4;
        Check.GetComponent<Toggle>().isOn = false;
    }
    //5. 점프하라
    void Jump()
    {
        missionText.GetComponent<Text>().text = "이번엔 점프를 해봐요!";

        checktime = 0;
        missionNum = 5;
        Check.GetComponent<Toggle>().isOn = false;
    }
    //6. 물체를 두드려봐라
    void MakeSound()
    {
        soundNote.transform.position = player.transform.localPosition + new Vector3(0, 0, 2f);
        soundNote.SetActive(true);
        missionText.GetComponent<Text>().text = "두드리면 소리가 나요!";


        checktime = 0;
        missionNum = 6;
        Check.GetComponent<Toggle>().isOn = false;
    }
    //7. 음악을 저장하기 버튼 클릭
    void SaveSound()
    {
        missionText.GetComponent<Text>().text = "내가 낸 소리들 저장하기!";

        checktime = 0;
        missionNum = 7;
        Check.GetComponent<Toggle>().isOn = false;
    }
    //8. 들어보기
    void PlaySound()
    {
        missionText.GetComponent<Text>().text = "내가 낸 소리들 들어보기!";

        checktime = 0;
        missionNum = 8;
        Check.GetComponent<Toggle>().isOn = false;
    }
    //9. 포탈로 이동! 게임시작
    void StartGame()
    {
        missionText.GetComponent<Text>().text = "자 이제 여행을 시작할까요?";

        checktime = 0;
        missionNum = 9;
        Check.GetComponent<Toggle>().isOn = false;
    }
    void LoadScene()
    {
        SceneManager.LoadScene(2);
    }

    //미션 성공 여부 판단
    void CheckMission()
    {
        Vector2 leftFoot = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 rightFoot = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        float LFVertical = leftFoot.y;
        float LFHorizontal = leftFoot.x;
        float RFVertical = rightFoot.y;
        float RFHorizontal = rightFoot.x;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //앞으로 이동하라
        if (missionNum == 1 &&  v > 0 && h==0)
        {
            Debug.Log("잘하는중");
            checktime += Time.deltaTime;
            //시간 2초 이상
            if (checktime > 2)
            {
                Check.GetComponent<Toggle>().isOn = true;
            }
        }
        if (missionNum == 1 && LFVertical > 0 && RFVertical > 0)
        {
            checktime += Time.deltaTime;
            //시간 2초 이상
            if (checktime > 2)
            {
                Check.GetComponent<Toggle>().isOn = true;
            }
        }
        //왼쪽으로 돌아라
        if (missionNum == 2 && h < 0 && v == 0)
        {
            Debug.Log("잘하는중");
            checktime += Time.deltaTime;
            //시간 2초 이상
            if (checktime > 2)
            {
                Check.GetComponent<Toggle>().isOn = true;
            }
        }
        if (missionNum == 2 && LFVertical == 0 && RFVertical > 0)
        {
            if (checktime > 2)
            {
                Check.GetComponent<Toggle>().isOn = true;
            }
            Check.GetComponent<Toggle>().isOn = true;
        }

        //오른쪽으로 돌아라
        if (missionNum == 3 && h > 0 && v == 0)
        {
            Debug.Log("잘하는중");
            checktime += Time.deltaTime;
            //시간 2초 이상
            if (checktime > 2)
            {
                Check.GetComponent<Toggle>().isOn = true;
            }
        }
        //앞으로 이동하면서 회전하기
        if (missionNum == 4 && h != 0 && v > 0)
        {
            Debug.Log("잘하는중");
            checktime += Time.deltaTime;
            //시간 2초 이상
            if (checktime > 3)
            {
                Check.GetComponent<Toggle>().isOn = true;
            }
        }
        //점프하기
        if (missionNum == 5)
        {
            Debug.Log("잘하는중");
            checktime += Time.deltaTime;
            //시간 2초 이상
            if (checktime > 3)
            {
                Check.GetComponent<Toggle>().isOn = true;
            }
        }
        //물체 두드려서 소리내기
        if (missionNum == 6 && Touch.makeSound == true)
        {
            Check.GetComponent<Toggle>().isOn = true;
        }

        if (missionNum == 7)
        {
            Check.GetComponent<Toggle>().isOn = true;
        }
        if (missionNum == 8)
        {
            Check.GetComponent<Toggle>().isOn = true;
        }
        if (missionNum == 9)
        {
            Check.GetComponent<Toggle>().isOn = true;
        }
    }

}
