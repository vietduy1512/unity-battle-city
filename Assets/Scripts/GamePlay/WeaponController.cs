using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    GameObject projectileObject;

    GameObject bullet, fire;
    Projectile projectile;

    public AudioSource shotSFX;

    [SerializeField]
    int speed;
    void Start()
    {
        shotSFX = GetComponent<AudioSource>();
        bullet = Instantiate(projectileObject, transform.position, transform.rotation) as GameObject;
        projectile = bullet.GetComponent<Projectile>();
        projectile.speed = speed;
        fire = transform.GetChild(0).gameObject;
        bullet.SetActive(false);
    }
    public void Fire()
    {
        if (bullet.activeSelf == false)
        {
            if (shotSFX)
            {
                shotSFX.PlayOneShot(shotSFX.clip);
            }
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            StartCoroutine(ShowFire());
            bullet.SetActive(true);
        }
    }
    IEnumerator ShowFire()
    {
        fire.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        fire.SetActive(false);
    }
    private void OnDestroy()
    {
        if (bullet != null) projectile.DestroyProjectile();
    }
}