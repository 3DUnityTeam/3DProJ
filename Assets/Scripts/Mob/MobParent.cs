using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobParent : MonoBehaviour
{
    public float dotDamage;
    //최대 체력
    private int maxHp;
    //현재 체력
    private int hp;

    //프로퍼티
    //지속 데미지
    public float DotDamage { get { return this.dotDamage; }set { this.dotDamage = value; } }
    //최대 체력 프로퍼티
    public int MaxHP { get { return this.maxHp; } set { this.maxHp = value; } }
    //현재 체력 프로퍼티
    public int HP { get { return this.hp; } set { this.hp = value; } }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HP = HP + 1;
        }
        //플레이어에게 데미지 입히는건 각 몬스터 코드로 
        ///
        /// public new void OnCollisionEnter(Collision collision)  //드래곤 몸통 데미지
        ///{
        ///    base.OnCollisionEnter(collision);
        ///    #데미지 코드 삽입
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
