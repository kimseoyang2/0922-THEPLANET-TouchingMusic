using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveFox : MonoBehaviour
{
    // 여우 캐릭터의 상태 정의
    public enum State
    {
        Follow,
        Rhythm,
        Touch,
        Listen
    }

    // 여우가 돌아다닐 지점들을 저장하기 위한 List 변수.
    public List<Transform> wayPoints;
    // 다음 순찰 지점의 배열의 Index.
    public int nextIdx;
    private readonly float patrolSpeed = 0.7f;
    private readonly float traceSpeed = 1.2f;

    // 여우의 행동 랜덤을 위한 변수
    public int randomInt;

    // 랜덤 변수가 할당 되었는가?
    public bool IsRan = false;

    public int moveInt;

    // 회전할때 속도를 조절하는 계수
    private float damping = 1.0f;

    public FoxAI foxAI;


    // NavMesh 컴포넌트를 저장할 변수
    private NavMeshAgent agent;
    // Player의 Transform 컴포넌트를 저장할 변수
    private Transform FoxTr;

    // 순찰 여부를 판단하는 변수
    private bool _partrolling;
    // partrolling 프로퍼티 정의 (getter,setter)

    public bool partrolling
    {
        get { return _partrolling; }
        set
        {
            _partrolling = value;
            if (_partrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f;
                MoveWayPoint();
            }
        }
    }
    // 추적 대상의 위치를 저장하는 변수
    private Vector3 _traceTarget;
    // traceTarget 프로퍼티 정의(getter,setter)
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            // 추적상태의 회전계수
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }
    // Navmesh의 이동 속도에 대한 프로퍼티 정의(getter)
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }
    // Start is called before the first frame update
    void Start()
    {
        // 여우 캐릭터의 Transform 컴포넌트 추출
        FoxTr = GetComponent<Transform>();
        // NavMeshAgent 컴포넌트를 추출한 후 변수에 저장
        agent = GetComponent<NavMeshAgent>();
        // 목적지에 가까워질수록 속도를 줄이는 옵션을 비활성화
        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.speed = patrolSpeed;

        // 하이라키 뷰의 돌아다닐 WayPointGroup 오브젝트 추출
        var group = GameObject.Find("WayPointsGroup");
        if(group != null)
        {
            // WayPointGroup 하위에 있는 모든 Transform 컴포넌트를 추출한 후.
            // List 타입의 wayPoints 배열에 추가.
            group.GetComponentsInChildren<Transform>(wayPoints);
            // 배열의 첫 번째 항목을 삭제한다.
            wayPoints.RemoveAt(0);
        }
        MoveWayPoint();
    }

    // 다음 목적지까지 이동명령을 내리는 함수
    void MoveWayPoint()
    {
        // 최단거리, 경로 계신이 끝나지 않았으면 다음을 수행하지 않음
        if (agent.isPathStale) return;

        // 다음 목적지를 wayPoints 배열에서 추출한 위치로 다음 목적지를 지정
        agent.destination = wayPoints[nextIdx].position;
        // 내비게이션 기능을 활성화해서 이동을 시작함.
        agent.isStopped = false;
    }

    // 플레이어를 쫓아가는 함수
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }

    public void stop()
    {
        agent.isStopped = true;
     
        // 바로 정지하기 위해 속도를 0으로 설정
        agent.velocity = Vector3.zero;
       
      
        _partrolling = false;
    }
    // Update is called once per frame
    void Update()
    {
        
          

        // 적 캐릭터가 이동 중일때만 회전
        if (agent.isStopped == false)
        {
            //navmeshAgent가 가야할 방향 벡터를 쿼터니언 타입의 각도로 변환
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //  보간함수로 점진적으로 회전한다
            FoxTr.rotation = Quaternion.Slerp(FoxTr.rotation, rot, Time.deltaTime * damping);
            if (nextIdx % 2 == 0)
            {
                moveInt = 1;
                Debug.Log("리듬타");
            }
            if (nextIdx % 2 == 1)
            {
                moveInt = 2;
                Debug.Log("가만있어");
            }
        }
        // 순찰모드가 아닐때는 이후 로직을 실행하지 않는다.
        if (!_partrolling) return;
        // NavMeshAgent가 이동하고 있고 목적지에 도착했는지 여부를 계산
        if(agent.velocity.sqrMagnitude>=0.2f*0.2f
            && agent.remainingDistance <= 0.5f)
        {
            // 다음 목적지의 배열 첨자를 계산
            nextIdx = ++nextIdx % wayPoints.Count;
            // 다음 목적지로 이동 명령을 수행
            MoveWayPoint();
        }

    }
}
