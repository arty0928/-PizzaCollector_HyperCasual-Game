using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //이 아이템은 어떤 타입인지 명시
    //enum: 열거형 타입
    //아이템들 열거하기
    public enum Type { LunchItem, SaveItem, TimeItem };
    public Type type;
    public int value;

    //아이템 회전
    //20: 회전속도
    private void Update()
    {
        //transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }

}
