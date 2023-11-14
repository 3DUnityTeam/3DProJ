using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobParent : MonoBehaviour
{
    //�ִ� ü��
    private int maxHp;
    //���� ü��
    private int hp;

    //�ִ� ü�� ������Ƽ
    public int MaxHP { get { return this.maxHp; } set { this.maxHp = value; } }
    //���� ü�� ������Ƽ
    public int HP { get { return this.hp; } set { this.hp = value; } }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HP=HP+1;
        }

        if (collision.gameObject.CompareTag("Player"))
        {

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            HP = HP + 1;
        }
    }
}
