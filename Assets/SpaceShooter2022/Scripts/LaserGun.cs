using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class LaserGun : MonoBehaviour
{
    [SerializeField] private Animator laserAnimator;
    [SerializeField] private AudioClip laserSFX;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int curAmmo;
    [SerializeField] float timeReload = 1.5f;
    private bool isCanShoot = true;

    [SerializeField] private TMP_Text textAmmo;

    private AudioSource laserAudioSource;

    private RaycastHit hit;

    float timeElapsed; 
    float valueToLerp;

    public Recoil recoil;


    private void Awake()
    {
       laserAudioSource = GetComponent<AudioSource>();
        curAmmo = maxAmmo;
        textAmmo.text = curAmmo.ToString();
    }

    void Update()
    {
        if (!isCanShoot)
        {
            Reloading();
        }
    }

    public void LaserGunFired() 
    {
        if (isCanShoot)
        {
            curAmmo -= 1;

            laserAnimator.SetTrigger("Fire");

            //play laser gun SFX
            laserAudioSource.PlayOneShot(laserSFX);

            textAmmo.text = curAmmo.ToString();

            recoil.RecoilFire();

            //raycast
            if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, 800f))
            {
                if (hit.transform.GetComponent<AsteroidHit>() != null)
                {
                    hit.transform.GetComponent<AsteroidHit>().AsteroidDestroyed();
                }
                else if (hit.transform.GetComponent<IRaycastInterface>() != null)
                {
                    hit.transform.GetComponent<IRaycastInterface>().HitByRaycast();
                }

            }
            if (curAmmo <= 0)
            {
                isCanShoot = false;
                timeElapsed = 0;
            }
        }
    }

    private void Reloading()
    {
        if (timeElapsed < timeReload && !isCanShoot)
        {
            valueToLerp = Mathf.Lerp(curAmmo, maxAmmo, timeElapsed / timeReload);
            timeElapsed += Time.deltaTime;
            textAmmo.text = ((int)valueToLerp).ToString();
            textAmmo.color = Color.red;
        }
        else
        {
            valueToLerp = maxAmmo;
            curAmmo = maxAmmo;
            isCanShoot = true;
            textAmmo.text = curAmmo.ToString();
            textAmmo.color = Color.white;
        }
    }

    
}
