using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobParent : MonoBehaviour
{
    private bool dead;
    private float dotDamage;
    //�ִ� ü��
    private float maxHp;
    //���� ü��
    private float hp=0;

    //������Ƽ
    //���� ������
    public bool Dead { get { return this.dead; }set { this.dead = value; } }
    //���� ������
    public float DotDamage { get { return this.dotDamage; }set { this.dotDamage = value; } }
    //�ִ� ü�� ������Ƽ
    public float MaxHP { get { return this.maxHp; } set { this.maxHp = value; } }
    //���� ü�� ������Ƽ
    public float HP { get { return this.hp; } set { this.hp = value; } }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletCtrl bulletInfo = collision.gameObject.GetComponent<BulletCtrl>();
            HP = HP + bulletInfo.damage;
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
            bool caneffect = true;
            TriggerCollison effectInfo = other.gameObject.GetComponent<TriggerCollison>();
            if(caneffect)
            {
                caneffect = false;
                GameObject effect = GameManager.instance.effectPoolManger.Get(effectInfo.effectcode - 1);
                effect.transform.position = other.ClosestPoint(transform.position);
                
            }
            
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
