using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics_Ray : MonoBehaviour
{
 
    public bool follow; //감지 했는지

    //트랜스폼을 담을 변수
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

    // Start is called before the first frame update
    void Start()
    {
        //트랜스폼을 받아온다
        m_tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.I.isDead == false && GameManager.I.time == true)
        {
            //레이 세팅
            Ray ray = new Ray();

            //시작점 세팅
            ray.origin = m_tr.position;

            //방향 설정
            ray.direction = m_tr.forward;

            //사용 방법

            //if (Physics.Raycast(ray, out hit, m_layerMask))
           
            if(Physics.Raycast(transform.position, transform.forward, out hit, float.PositiveInfinity))
            {
                if(hit.transform.tag == "Player")
                {
                    follow = true;
                    Debug.DrawLine(m_tr.position, m_tr.position + m_tr.forward * hit.distance, Color.red);
                    
                }
                else
                {
                    follow = false;
                    Debug.DrawLine(m_tr.position, m_tr.position + m_tr.forward * this.distance, Color.white);
                }
            }

            //OnDrawRayLine();
        }


    }
    /*public void OnDrawRayLine()
    {
        if(hit.collider != null)
        {
            Debug.DrawLine(m_tr.position, m_tr.position + m_tr.forward * hit.distance, Color.red);
            follow = true;
        }
        else
        {
            Debug.DrawLine(m_tr.position, m_tr.position + m_tr.forward * this.distance, Color.white);
            follow=false;
        }
    }*/
}
