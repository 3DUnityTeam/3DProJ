using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobParent : MonoBehaviour
{
    private bool dead;
    private float dotDamage;
    //최대 체력
    private float maxHp;
    //현재 체력
    private float hp=0;

    //프로퍼티
    //지속 데미지
    public bool Dead { get { return this.dead; }set { this.dead = value; } }
    //지속 데미지
    public float DotDamage { get { return this.dotDamage; }set { this.dotDamage = value; } }
    //최대 체력 프로퍼티
    public float MaxHP { get { return this.maxHp; } set { this.maxHp = value; } }
    //현재 체력 프로퍼티
    public float HP { get { return this.hp; } set { this.hp = value; } }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletCtrl bulletInfo = collision.gameObject.GetComponent<BulletCtrl>();
            HP = HP + bulletInfo.damage;
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
            TriggerCollison effectInfo = other.gameObject.GetComponent<TriggerCollison>();
            GameObject effect = GameManager.instance.effectPoolManger.Get(effectInfo.effectcode - 1);
            effect.transform.position = other.ClosestPoint(transform.position);
			HP = HP + effectInfo.damage;
        }
    }

    public virtual void OnEnable()
    {
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = true;
        HP = 0;
        Dead = false;
    }

    public virtual void IsDead() {
        Dead = true;
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
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
