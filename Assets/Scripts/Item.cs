using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //�� �������� � Ÿ������ ���
    //enum: ������ Ÿ��
    //�����۵� �����ϱ�
    public enum Type { LunchItem, SaveItem, TimeItem };
    public Type type;
    public int value;

    //������ ȸ��
    //20: ȸ���ӵ�
    private void Update()
    {
        //transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }

}
