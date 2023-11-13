using System.Collections;
using UnityEngine;

public class Blueberry : MonoBehaviour
{
    Transform trans_;
    public GameObject fx;
    GameObject spawnPoz;
    Rigidbody rigid_;

    bool go = false;

    private void Awake()
    {
        trans_ = GetComponent<Transform>();
        string[] thisName = this.gameObject.name.Split();
        spawnPoz = GameObject.Find("MeteoPoz " + thisName[1]);
        rigid_ = GetComponent<Rigidbody>();
        rigid_.useGravity = false;
    }


    private void Update()
    {
        go = GameObject.Find("Dragon").GetComponent<DragonController>().meteoAll;
        if (!go)
            Debug.Log("go is missing");
        else
            Debug.Log(go);
        if (go)
        {
            Debug.Log("Fall");
            trans_.Translate(spawnPoz.transform.position * 3f * Time.deltaTime);
            rigid_.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Poz"))
        {
            Destroy(this.gameObject, 1.2f);
            fx.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Land"))
        {
            Destroy(this.gameObject, 1.2f);
            fx.SetActive(true);
        }
    }
}
