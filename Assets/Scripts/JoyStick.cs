using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    //움직일 캐릭터
    public Transform player;
    Vector3 move;
    public float moveSpeed;

    //조이스틱이 밖으로 나가지 못하게 감옥
    public RectTransform pad;

    public void OnDrag(PointerEventData eventData)
    {
        //드래그하면 조이스틱이 움직임
        transform.position = eventData.position;
        //조이스틱이 밖으로 못나가게 
        transform.localPosition = Vector2.ClampMagnitude(eventData.position - (Vector2)pad.position, pad.rect.width * 0.5f);

        //조이스틱을 드래그하는 동안 방향을 저장
        move = new Vector3(transform.localPosition.x,0,transform.localPosition.y).normalized;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //터치하지 않으면 조이스틱이 원래 자리로 돌아감
        transform.localPosition = Vector3.zero;
        //터치하지 않으면 방향 없앰
        move = Vector3.zero;
        //조이스틱을 터치하지 않으면 엔진이 멈춤
        StopCoroutine("PlayerMove");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //조이스틱을 터치하면 캐릭터를 움직이는 엔진이 작동
        StartCoroutine("PlayerMove");
    }

    //캐릭터를 움직일 수 있는 엔진
    IEnumerator PlayerMove()
    {
        while (true)
        {
            //조이스틱이 움직이는 곳으로 캐릭터가 움직임
            //player.Translate(move * moveSpeed * Time.deltaTime, Space.World);
                player.position = player.position + (move * moveSpeed * Time.deltaTime);

            //플레이어 포지션의 x,z중 하나가 0.5초동안 동일하면 움직이지 않는 부분 뺀 다른 방향의 값을 크게 해주기


            //조이스틱이 움직이는 곳으로 캐릭터가 바라봄
            if (move != Vector3.zero)
            {
                player.rotation = Quaternion.Slerp(player.rotation,Quaternion.LookRotation(move),5*Time.deltaTime);
            }
            yield return null;
        }
    }
}
