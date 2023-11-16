using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public int effectID = 1;
    public float damage = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable()
    {
        StartCoroutine(ActiveFalse());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Mob") || collision.gameObject.CompareTag("Land"))
        {
            GameObject effect = GameManager.instance.effectPoolManger.Get(effectID - 1);
            effect.transform.position = collision.contacts[0].point;
            gameObject.SetActive(false);
        }
    }

    IEnumerator ActiveFalse()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    
}
