//patrol, chase (ok) +raycast(ok) + 경로대로 rotate(ok) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
	//raycast
	//public bool follow; //감지 했는지

	//트랜스폼을 담을 변수
	//public Rigidbody m_ro;
	public Transform m_tr;

	//레이 길이를 지정할 변수
	public float distance = 10.0f;

	//충돌 정보를 가져올 레이케스트 히트
	public RaycastHit hit;

	//레이어 마스크를 지정할 변수\
	//-1: 모든 obj
	public LayerMask m_layerMask = -1;

	//충돌 정보를 여러개 담을 레이캐스트 히트 배열
	public RaycastHit[] hits;


	public Transform player;
	public float playerDistance;
	public float awareAI = 5f;
	public float AIMoveSpeed;
	public float damping = 6.0f;
	//public float damping = 2.0f;

	public Transform[] navPoint;
	public UnityEngine.AI.NavMeshAgent agent;
	public int destPoint = 0;
	public Transform goal;
	private int randPos;

	void Start()
	{
		//enemyHealth = 100;
		UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.destination = goal.position;

		agent.autoBraking = false;

		randPos = Random.Range(0, navPoint.Length + 1);
		destPoint = (randPos) % navPoint.Length;
		agent.destination = navPoint[destPoint].position;

		//raycast
		//트랜스폼을 받아온다
		m_tr = GetComponent<Transform>();
		//m_ro = GetComponent<Rigidbody>();
	}

	void Update()
	{
		transform.LookAt(agent.velocity + transform.position);

		//if (GameManager.I.isDead == false && GameManager.I.time == true)
		{
			//레이 세팅
			Ray ray = new Ray();

			//시작점 세팅
			ray.origin = m_tr.position;

			//방향 설정
			ray.direction = m_tr.forward;

			//사용 방법

			if (Physics.Raycast(transform.position, transform.forward, out hit, 20f))
			{
				if (hit.transform.tag == "Player")
				{
					//follow = true;
					Debug.DrawLine(m_tr.position, m_tr.position + m_tr.forward * hit.distance, Color.red);
					LookAtPlayer();
					Debug.Log("Seen Player");
					Chase();

				}
				else
				{
					//follow = false;
					Debug.DrawLine(m_tr.position, m_tr.position + m_tr.forward * this.distance, Color.white);
					if (Vector3.Distance(transform.position, agent.destination) < 1)
					{
						GotoNextPoint();
					}

				}
			}
		}
	}

	void LookAtPlayer()
	{
		transform.LookAt(player);
	}


	void GotoNextPoint()
	{
		if (navPoint.Length == 0)
			return;

		agent.destination = navPoint[destPoint].position;

		//int xcount = Random.Range(1, 6);
		int randPos = Random.Range(0, navPoint.Length + 1);
		destPoint = (randPos) % navPoint.Length;


	}


	void Chase()
	{
		agent.destination = player.position;
		//transform.Translate(player.position * AIMoveSpeed * Time.deltaTime);
	}


}