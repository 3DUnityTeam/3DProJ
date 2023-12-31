using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
    public enum WeaponType
    {
        bullet,beam
    }

    [Header("외부 오브젝트")]
    public AudioManager.Sfx sound;
    private WeaponControl parent;
    private AimManager aimManager;
    public int bulletID;
    public Transform firePos;
    public AudioClip fireSfx;
    private new AudioSource audio;
    //총알 발사 타입 무기일 때 쓰는거
    public MeshRenderer muzzelFlash;
    public GameObject muzzlefire;
    public Light muzzlelight;
    //빔 타입일 때 쓰는 거
    public GameObject Beam;
    private Gunmove gunmove;

    [Header("내부 수치")]
    public WeaponType weaponType = WeaponType.bullet;
    public float rapidspeed = 0.7f;
    private float ctime = 0f;
    //총알 발사 타입일 때 쓰는 거
    public float bulletspeed = 2000f;
    //빔 타입일 때 쓰는 거
    public float beamtime = 0.3f;
    public float damage = 20f;
    

    private void Awake()
    {
        parent = gameObject.GetComponentInParent<WeaponControl>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gunmove = GetComponent<Gunmove>();
        aimManager = GameManager.instance.AimManager;
        ctime = rapidspeed;
        //audio = GetComponent<AudioSource>();
        //muzzelFlash.enabled = false;
    }

    private void OnEnable()
    {
        if(muzzlefire)
        {
            muzzlefire.SetActive(false);
        }
        if(muzzlelight)
        {
            muzzlelight.enabled = false;
        }
    }

    private void Update()
    {
        ctime -= Time.deltaTime;
        if (ctime <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                if (parent.state == WeaponControl.State.normal || parent.state == WeaponControl.State.shooted)
                {
                    Fire();
                    ctime = rapidspeed;
                } 
            }
        }
        
    }
    // Update is called once per frame
    void Fire()
    {
        switch(weaponType)
        {
            case WeaponType.bullet:
                GameObject firedbullet = GameManager.instance.bulletPoolManger.Get(bulletID - 1);
                Vector3 direction;

                if (firedbullet != null)
                {
                    // 오브젝트 풀에서 나온 오브젝트의 초기 상태 설정
                    firedbullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    firedbullet.transform.position = firePos.transform.position;
                    firedbullet.transform.rotation = firePos.transform.rotation;

                    if (aimManager.aimingTarget != null)
                    {
                        // 타겟을 향하도록 회전 설정
                        Vector3 center = aimManager.aimingTarget.GetComponent<Collider>().bounds.center;
                        firedbullet.transform.LookAt(center);
                        direction = (center - firePos.transform.position).normalized;
                    }
                    else
                    {
                        // 타겟이 없을 경우 초기 방향 설정
                        firedbullet.transform.forward = firePos.transform.forward;
                        direction = (gunmove.basic.position - firePos.transform.position).normalized;
                    }

                    firedbullet.GetComponent<BulletCtrl>().damage = damage;
                    firedbullet.GetComponent<Rigidbody>().AddForce(direction * bulletspeed);
                    
                }
                //audio.PlayOneShot(fireSfx, 1.0f);
                if (muzzelFlash != null)
                {
                    StartCoroutine(ShowMuzzleFlash());
                }
                else if (muzzlefire != null)
                {
                    muzzlelight.enabled = true;
                    muzzlefire.SetActive(true);
                    StartCoroutine(ShowMuzzleFire());
                }
                break;

            case WeaponType.beam:
                StartCoroutine(Beamshow(beamtime));
                break;
        }
        GameManager.instance.AudioManager.PlaySfx(sound);
        parent.shoot();
    }

    IEnumerator ShowMuzzleFlash()
    {
        Vector3 localscale = muzzelFlash.transform.localScale;

        float angle = Random.Range(0, 360);
        muzzelFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);

        float scale = Random.Range(0.5f, 1.2f);
        muzzelFlash.transform.localScale = localscale * scale;

        muzzelFlash.enabled = true;
        muzzlelight.enabled = true;

        yield return new WaitForSeconds(rapidspeed/4);
        muzzelFlash.transform.localScale = localscale;
        muzzelFlash.enabled = false;
        muzzlelight.enabled = false;
    }
    IEnumerator ShowMuzzleFire()
    {
        yield return new WaitForSeconds(rapidspeed - 0.2f);
        muzzlefire.SetActive(false);
        muzzlelight.enabled = false;

    }

    IEnumerator Beamshow(float time)
    {
        Beam.SetActive(true);
        Beam.GetComponent<TriggerCollison>().damage = damage;
        yield return new WaitForSeconds(time);
        Beam.SetActive(false);
    }
}
