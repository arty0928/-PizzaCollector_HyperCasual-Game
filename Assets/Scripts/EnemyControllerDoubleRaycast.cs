using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyControllerDoubleRaycast : MonoBehaviour
{
	public bool isSeen2;

	//Raycast2
	public float viewRadius2;
	[Range(0,360)]
	public float viewAngle2;

	public Vector3 DirFromAngle2(float angleInDegrees2, bool angleIsGlobal2)
    {
        if (!angleIsGlobal2)
        {
			angleInDegrees2 += transform.eulerAngles.y;
        }
		return new Vector3(Mathf.Sin(angleInDegrees2 * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees2 * Mathf.Deg2Rad));
    }

	
	///////////////////
	/// </summary>
	//raycast
	//public bool follow; //감지 했는지
	public float meshResolution2;
	public int edgeResolveIterations2;
	public float edgeDstThreshold2;

	public MeshFilter viewMeshFilter2;
	Mesh viewMesh2;

	
	// 마스크 2종
	public LayerMask targetMask2, obstacleMask2;

	// Target mask에 ray hit된 transform을 보관하는 리스트
	public List<Transform> visibleTargets2 = new List<Transform>();

	void Start()
	{
	
			viewMesh2 = new Mesh();
			viewMesh2.name = "View mesh";
			viewMeshFilter2.mesh = viewMesh2;

			
			//raycast
			//트랜스폼을 받아온다
			StartCoroutine(FindTargetsWithDelay(0.2f));
			GameManager.I.LevlSet = false;

			Debug.Log("EnemyControllerAngle Start() after LevelSet");
			Debug.Log("isPlay: " + GameManager.I.isPlay);
			Debug.Log("isDead: " + GameManager.I.isDead);


		

	}

	void DrawFieldOfView()
	{
		int stepCount = Mathf.RoundToInt(viewAngle2 * meshResolution2);
		float stepAngleSize = viewAngle2 / stepCount;
		List<Vector3> viewPoints = new List<Vector3>();
		ViewCastInfo oldViewCast = new ViewCastInfo();
		for (int i = 0; i <= stepCount; i++)
		{
			float angle = transform.eulerAngles.y - viewAngle2 / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast(angle);

			if (i > 0)
			{
				bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold2;
				if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
				{
					EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
					if (edge.pointA != Vector3.zero)
					{
						viewPoints.Add(edge.pointA);
					}
					if (edge.pointB != Vector3.zero)
					{
						viewPoints.Add(edge.pointB);
					}
				}

			}


			viewPoints.Add(newViewCast.point);
			oldViewCast = newViewCast;
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		vertices[0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++)
		{
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

			if (i < vertexCount - 2)
			{
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}

		viewMesh2.Clear();

		viewMesh2.vertices = vertices;
		viewMesh2.triangles = triangles;
		viewMesh2.RecalculateNormals();
	}
	EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
	{
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (var i = 0; i < edgeResolveIterations2; i++)
		{
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo newViewCast = ViewCast(angle);
			bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold2;


			if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
			{
				minAngle = angle;
				minPoint = newViewCast.point;
			}
			else
			{
				maxAngle = angle;
				maxPoint = newViewCast.point;

			}
		}
		return new EdgeInfo(minPoint, maxPoint);

	}
	public struct ViewCastInfo
	{
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
		{
			hit = _hit;
			point = _point;
			dst = _dst;
			angle = _angle;
		}


	}

	public struct EdgeInfo
	{
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
		{
			pointA = _pointA;
			pointB = _pointB;
		}

	}

	ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 dir = DirFromAngle(globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast(transform.position, dir, out hit, viewRadius2, obstacleMask2))
		{
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			return new ViewCastInfo(false, transform.position + dir * viewRadius2, viewRadius2, globalAngle);
		}
	}

	IEnumerator FindTargetsWithDelay(float delay)
	{
		Debug.Log("FindTargetWithDelay");
		Debug.Log("isPlay: " + GameManager.I.isPlay);
		Debug.Log("isDead: " + GameManager.I.isDead);

		//if (GameManager.I.isPlay == true && GameManager.I.isDead==false)
		{
			while (true)
			{
				yield return new WaitForSeconds(delay);
				FindVisibleTargets();
			}
		}

	}

	void FindVisibleTargets()
	{
		visibleTargets2.Clear();
		// viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius2, targetMask2);

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;

			// 플레이어와 forward와 target이 이루는 각이 설정한 각도 내라면
			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle2 / 2)
			{
				float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

				// 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
				if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask2))
				{

					visibleTargets2.Add(target);
					isSeen2 = true;
				}

			}
			isSeen2 = false;
			/*if (transform.CompareTag("EnemySpeedUp"))
			//if (transform.tag == "EnemySpeedUp")
			{
				agent.speed = 3;
			}
*/
		}
	}

	// y축 오일러 각을 3차원 방향 벡터로 변환한다.
	// 원본과 구현이 살짝 다름에 주의. 결과는 같다.
	public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleDegrees += transform.eulerAngles.y;
		}

		return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
	}

	void LateUpdate()
	{
		DrawFieldOfView();

	}
}