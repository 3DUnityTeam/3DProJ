using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobParent : MonoBehaviour
{
    public float dotDamage;
    //�ִ� ü��
    private int maxHp;
    //���� ü��
    private int hp;

    //������Ƽ
    //���� ������
    public float DotDamage { get { return this.dotDamage; }set { this.dotDamage = value; } }
    //�ִ� ü�� ������Ƽ
    public int MaxHP { get { return this.maxHp; } set { this.maxHp = value; } }
    //���� ü�� ������Ƽ
    public int HP { get { return this.hp; } set { this.hp = value; } }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HP = HP + 1;
        }
        //�÷��̾�� ������ �����°� �� ���� �ڵ�� 
        ///
        /// public new void OnCollisionEnter(Collision collision)  //�巡�� ���� ������
        ///{
        ///    base.OnCollisionEnter(collision);
        ///    #������ �ڵ� ����
        ///}
        ///
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.gameObject.GetComponent<TriggerCollison>() != null)
        {
            TriggerCollison trigger = other.gameObject.GetComponent<TriggerCollison>();
            GameObject effect = GameManager.instance.effectPoolManger.Get(trigger.effectcode - 1);
            effect.transform.position = other.ClosestPoint(transform.position);
            HP = HP + 1;
        }
    }

    public virtual void IsDead() {
        GameManager.instance.SpawnManager.spawnMob.Remove(gameObject.GetInstanceID());
    }

    public enum TimeType
    {
        Update,
        FixedUpdate
    }
    public IEnumerator IsLive(float damage)
    {
        DotDamage = damage;
        while(HP < MaxHP)
        {
            yield return new WaitForFixedUpdate();
            GameManager.instance.tofuFoolr.HP = GameManager.instance.tofuFoolr.HP - damage * Time.fixedDeltaTime;
        }
    }
}
