using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobParent : MonoBehaviour
{
    //최대 체력
    private int maxHp;
    //현재 체력
    private int hp;

    //최대 체력 프로퍼티
    public int MaxHP { get { return this.maxHp; } set { this.maxHp = value; } }
    //현재 체력 프로퍼티
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
