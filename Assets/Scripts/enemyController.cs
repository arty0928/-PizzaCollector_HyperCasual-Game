//patrol, chase (ok) +raycast(ok) + ��δ�� rotate(ok) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
	//raycast
	//public bool follow; //���� �ߴ���

	//Ʈ�������� ���� ����
	//public Rigidbody m_ro;
	public Transform m_tr;

	//���� ���̸� ������ ����
	public float distance = 10.0f;

	//�浹 ������ ������ �����ɽ�Ʈ ��Ʈ
	public RaycastHit hit;

	//���̾� ����ũ�� ������ ����\
	//-1: ��� obj
	public LayerMask m_layerMask = -1;

	//�浹 ������ ������ ���� ����ĳ��Ʈ ��Ʈ �迭
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
		//Ʈ�������� �޾ƿ´�
		m_tr = GetComponent<Transform>();
		//m_ro = GetComponent<Rigidbody>();
	}

	void Update()
	{
		transform.LookAt(agent.velocity + transform.position);

		//if (GameManager.I.isDead == false && GameManager.I.time == true)
		{
			//���� ����
			Ray ray = new Ray();

			//������ ����
			ray.origin = m_tr.position;

			//���� ����
			ray.direction = m_tr.forward;

			//��� ���

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