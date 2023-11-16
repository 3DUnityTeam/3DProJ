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

    public void DeleteDict()
    {
        GameManager.instance.SpawnManager.spawnMob.Remove(gameObject.GetInstanceID());
    }
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
        if (other.gameObject.gameObject.GetComponent<TriggerCollison>() != null)
        {
            TriggerCollison trigger = other.gameObject.GetComponent<TriggerCollison>();
            GameObject effect = GameManager.instance.effectPoolManger.Get(trigger.effectcode - 1);
            effect.transform.position = other.ClosestPoint(transform.position);
            HP = HP + 1;
        }
    }
}
