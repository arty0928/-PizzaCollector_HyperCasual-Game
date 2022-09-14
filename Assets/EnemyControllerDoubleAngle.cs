using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyControllerDoubleAngle : MonoBehaviour
{
	//Raycast
	public FieldOfView1 firstRayCast;
	public FieldOfView2 SecondRayCast;
	public PlayerController playercontroller;

	private Transform player;

	public float playerDistance;
	public float awareAI = 5f;
	public float OriginalAIMoveSpeed;
	//public float SpeedUpAIMoveSpeed;
	public float damping = 6.0f;

	//enemy patrol, chase

	//public Transform[] navPoint;
	public Vector3[] navPoint;
	//private Transform navPos;
	public Vector3 navPos;

	public UnityEngine.AI.NavMeshAgent agent;

	public int destPoint = 0;
	private int randPos;
	public bool range;


	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;

		//if (GameManager.I.isPlay == true && GameManager.I.isDead == false && GameManager.I.LevlSet == true)
		{

			UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

			Debug.Log("EnemyControllerAngle Start()");
			//GameManager.I.isPlay = true;
			Debug.Log("isPlay: " + GameManager.I.isPlay);
			Debug.Log("isDead: " + GameManager.I.isDead);

			var navPoints = GameObject.FindGameObjectsWithTag("patrolPoint").Select(ItemToPut => ItemToPut.transform.position).ToArray();
			navPoints = navPoints.OrderBy(navPoint => Random.Range(-1.0f, 1.0f)).ToArray();
			Debug.Log("navPoints.length: " + navPoints.Length);

			for (var i = 0; i < navPoints.Length; i++)
			{
				if (i == 6)
					//Debug.Log("navPoints"+i + "EnemyControllerAngle" +navPoints[i]);
					navPoint[i] = navPoints[i];
				Debug.Log("navPoints" + i);
			}


			randPos = Random.Range(0, navPoint.Length + 1);
			destPoint = (randPos) % navPoint.Length;
			agent.destination = navPoint[destPoint];


			agent.autoBraking = false;


			GameManager.I.LevlSet = false;

			Debug.Log("EnemyControllerAngle Start() after LevelSet");
			Debug.Log("isPlay: " + GameManager.I.isPlay);
			Debug.Log("isDead: " + GameManager.I.isDead);


		}

	}


	void FixedUpdate()
	{

		if (firstRayCast.isSeen1 == true)
		{
			Chase();
		}
		else if (firstRayCast.isSeen1 == false && SecondRayCast.isSeen2 == true && playercontroller.isHide == true)
		{
			GotoNextPoint();
		}
		else if (firstRayCast.isSeen1 == false && SecondRayCast.isSeen2 == true && playercontroller.isHide == false)
		{
			Chase();
		}

		float distanceFormGoal = Vector3.Distance(transform.position, agent.destination);

		//Debug.Log(distanceFormGoal + "ÀÌ°ÅÀÓ" + 0.1f);

		//if (distanceFormGoal < 0.1f)
		if (agent.remainingDistance < 0.5f)
		{
			//Debug.Log(distanceFormGoal + "GotoNext");
			GotoNextPoint();
			/*checkPos = true;
			StartCoroutine(WaitForIt());*/
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

		//agent.destination = navPoint[destPoint];

		//int xcount = Random.Range(1, 6);
		//int randPos = Random.Range(0, navPoint.Length + 1);
		//destPoint = (randPos) % navPoint.Length;

		randPos = Random.Range(0, navPoint.Length + 1);
		destPoint = (randPos) % navPoint.Length;
		agent.destination = navPoint[destPoint];

	}

	void Chase()
	{
		agent.destination = player.position;
		//transform.Translate(player.position * AIMoveSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			GotoNextPoint();
		}
	}

}