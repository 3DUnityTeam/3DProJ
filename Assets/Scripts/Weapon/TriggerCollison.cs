using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollison : MonoBehaviour
{
    public enum WeaponCode
    {
        explosion, beam
    };

    [Header ("공격의 타입 결정")]
    public WeaponCode weapontype;
    public float effectTime;
    private SphereCollider myCD;
    [Header ("공격력과 이펙트")]
    public float damage;
    public GameObject effect;
    public int effectcode;
    public float width = 2;
    // Start is called before the first frame update

    private void OnEnable()
    {
        //effect = GameManager.instance.effectPoolManger.Get(effectcode - 1);
        if(weapontype == WeaponCode.explosion)
        {
            StartCoroutine(Effectcontrol());
            myCD = GetComponent<SphereCollider>();
            myCD.radius = 0.1f;
        }
        

    }
    // Update is called once per frame
    void Update()
    {
        if (weapontype == WeaponCode.explosion)
        {
            while (myCD.radius <= width)
            {
                myCD.radius = myCD.radius + Time.deltaTime;
            }
        }
        
    }

    IEnumerator Effectcontrol()
    {
        yield return new WaitForSeconds(effectTime);
        gameObject.SetActive(false);
    }
}
