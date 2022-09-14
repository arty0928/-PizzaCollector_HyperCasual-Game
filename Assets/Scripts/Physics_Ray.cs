using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics_Ray : MonoBehaviour
{
 
    public bool follow; //���� �ߴ���

    //Ʈ�������� ���� ����
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

    // Start is called before the first frame update
    void Start()
    {
        //Ʈ�������� �޾ƿ´�
        m_tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.I.isDead == false && GameManager.I.time == true)
        {
            //���� ����
            Ray ray = new Ray();

            //������ ����
            ray.origin = m_tr.position;

            //���� ����
            ray.direction = m_tr.forward;

            //��� ���

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
