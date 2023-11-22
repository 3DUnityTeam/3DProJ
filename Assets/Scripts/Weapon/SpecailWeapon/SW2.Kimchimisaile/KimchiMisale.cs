using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimchiMisale : MonoBehaviour
{
    public AudioManager.Sfx sound;
    private WeaponControl parent;
    public Transform muzzle;
    public int bulletID;
    private new AudioSource audio;
    public GameObject muzzlefire;
    public Light muzzlelight;

    [Header("내부 수치")]
    public float rapidspeed = 0.7f;
    public float ctime = 0f;
    public float bulletspeed = 2000f;
    public float damage = 20f;
    public int shootCount = 4;


    private void Awake()
    {
        parent = gameObject.GetComponentInParent<WeaponControl>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ctime = rapidspeed;
        //audio = GetComponent<AudioSource>();
        //muzzelFlash.enabled = false;
    }

    private void OnEnable()
    {
        if (muzzlefire)
        {
            muzzlefire.SetActive(false);
        }
        if (muzzlelight)
        {
            muzzlelight.enabled = false;
        }
    }

    private void Update()
    {
        if (ctime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (parent.state == WeaponControl.State.normal || parent.state == WeaponControl.State.shooted)
                {
                    StartCoroutine(Shoot());
                    ctime = rapidspeed;
                }
            }
        }

    }
    private void FixedUpdate()
    {
        if (ctime > 0)
        {
            ctime -= Time.fixedDeltaTime;
        }
    }
    // Update is called once per frame
    void Fire()
    {
        GameObject firedbullet = GameManager.instance.bulletPoolManger.Get(bulletID - 1);
        firedbullet.transform.position = muzzle.position;
        firedbullet.transform.rotation = muzzle.rotation;


        if (firedbullet != null)
        {
            firedbullet.GetComponent<Misale>().damage = damage;
            firedbullet.GetComponent<Misale>().bulletspeed = bulletspeed;
            firedbullet.GetComponent<Rigidbody>().AddForce(Vector3.up * bulletspeed);
        }
        //audio.PlayOneShot(fireSfx, 1.0f);
        if(muzzlelight != null)
        {
            muzzlelight.enabled = true;
            muzzlefire.SetActive(true);
        }

        GameManager.instance.AudioManager.PlaySfx(sound);
        StartCoroutine(ShowMuzzleFire());

    }

    IEnumerator ShowMuzzleFire()
    {
        yield return new WaitForSeconds(rapidspeed - 0.2f);
        muzzlefire.SetActive(false);
        muzzlelight.enabled = false;

    }

    IEnumerator Shoot()
    {
        parent.shoot();
        for (int i = 0; i < shootCount; i++)
        {
            Fire();
            yield return new WaitForSeconds(0.42f);
        }
    }
}
