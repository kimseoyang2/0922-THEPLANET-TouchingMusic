using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAI : MonoBehaviour
{
    // 여우의 상태를 표현하기 위한 열거형 변수 정의
    public enum foxState
    {
        PATROL,
        FOLLOW,
        RHYTHM,
        TOUCH,
        LISTEN,
        Die
    }

    // 상태를 저장할 변수
    public foxState foxstate = foxState.PATROL;

    // 주인공의 위치를 저장할 변수
    private Transform playerTr;
    // 여우 캐릭터의 위치를 저장할 변수
    private Transform foxTr;
    // Animator 컴포넌트를 저장할 변수
    private Animator animator;

    // 듣기 사정거리
    public float listenDist = 5.0f;
    // 따라가기 사정거리
    public float followDist = 10.0f;

    // 코루틴 실행 여부
    public bool isEnd = false;

    // 코루틴에서 사용할 지연시간 변수
    private WaitForSeconds ws;
    // 이동을 제어하는 MoveFox 클래스를 저장할 변수
    private MoveFox moveFox;

    // 애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashMove = Animator.StringToHash("IsFollow");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashRhythm = Animator.StringToHash("Motion");

    private void Awake()
    {
        // 어린왕자 게임 오브젝트 추출
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        // 어린왕자 포지션 컴포넌트 추출
        if (player != null)
            playerTr = player.GetComponent<Transform>();
        // 여우의 포지션 컴포넌트 추출
        foxTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        // 이동을 제어하는 MoveFox 클래스 추출
        moveFox = GetComponent<MoveFox>();

        // 코루틴의 지연시간 생성
        ws = new WaitForSeconds(0.3f);

    }
    private void OnEnable()
    {
        //  CheckState 코루틴 함수 실행
        StartCoroutine(CheckState());
        // Action 코루틴 함수 실행
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        // 여우 캐릭터의 무한루트
        while (!isEnd)
        {
            // 여우 코루틴 함수 종료시킴
            if (foxstate == foxState.Die) yield break;

            // 플레이어와 여우간의 거리 계산
            float dist = (playerTr.position - foxTr.position).sqrMagnitude;

            // 듣기 사정거리 이내인 경우
            if (dist <= listenDist*listenDist)
            {
                foxstate = foxState.LISTEN;
            }
            // 추적 사정거리 이내인 경우
            else if (dist <= followDist*followDist)
            {
                foxstate = foxState.FOLLOW;
            }
            else
            {
                foxstate = foxState.PATROL;
            }
            // 0.3초동안 대기하는 동안 제어권을 양보
            yield return ws;
        }

    }
    // 상태에 따라 여우 캐릭터의 행동을 처리하는 코루틴 함수
    IEnumerator Action()
    {
        // 게임 끝날때까지 무한 루프
        while (!isEnd)
        {
            yield return ws;
            // 상태에 따라 분기처리
            switch (foxstate)
            {
                case foxState.PATROL:
                    // 순찰 모드를 활성화
                    moveFox.partrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case foxState.FOLLOW:
                    // 플레이어 위치 받아 추적모드로 변경
                    moveFox.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case foxState.LISTEN:
                    // 순찰 및 추적을 정지
                  
                    moveFox.stop();
         
                    animator.SetBool(hashMove, false);
                    break;
                case foxState.Die:
                    break;

            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // speed 파라미터에 이동속도를 전달
        animator.SetFloat(hashSpeed, moveFox.speed);
        animator.SetFloat(hashRhythm, moveFox.moveInt);
      
    }
}
