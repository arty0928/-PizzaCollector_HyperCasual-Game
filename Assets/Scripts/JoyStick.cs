using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    //������ ĳ����
    public Transform player;
    Vector3 move;
    public float moveSpeed;

    //���̽�ƽ�� ������ ������ ���ϰ� ����
    public RectTransform pad;

    public void OnDrag(PointerEventData eventData)
    {
        //�巡���ϸ� ���̽�ƽ�� ������
        transform.position = eventData.position;
        //���̽�ƽ�� ������ �������� 
        transform.localPosition = Vector2.ClampMagnitude(eventData.position - (Vector2)pad.position, pad.rect.width * 0.5f);

        //���̽�ƽ�� �巡���ϴ� ���� ������ ����
        move = new Vector3(transform.localPosition.x,0,transform.localPosition.y).normalized;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //��ġ���� ������ ���̽�ƽ�� ���� �ڸ��� ���ư�
        transform.localPosition = Vector3.zero;
        //��ġ���� ������ ���� ����
        move = Vector3.zero;
        //���̽�ƽ�� ��ġ���� ������ ������ ����
        StopCoroutine("PlayerMove");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //���̽�ƽ�� ��ġ�ϸ� ĳ���͸� �����̴� ������ �۵�
        StartCoroutine("PlayerMove");
    }

    //ĳ���͸� ������ �� �ִ� ����
    IEnumerator PlayerMove()
    {
        while (true)
        {
            //���̽�ƽ�� �����̴� ������ ĳ���Ͱ� ������
            //player.Translate(move * moveSpeed * Time.deltaTime, Space.World);
                player.position = player.position + (move * moveSpeed * Time.deltaTime);

            //�÷��̾� �������� x,z�� �ϳ��� 0.5�ʵ��� �����ϸ� �������� �ʴ� �κ� �� �ٸ� ������ ���� ũ�� ���ֱ�


            //���̽�ƽ�� �����̴� ������ ĳ���Ͱ� �ٶ�
            if (move != Vector3.zero)
            {
                player.rotation = Quaternion.Slerp(player.rotation,Quaternion.LookRotation(move),5*Time.deltaTime);
            }
            yield return null;
        }
    }
}
