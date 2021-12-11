using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun_Mecanics : MonoBehaviour
{
    //Gun stats
    [SerializeField] int damage, magazinSize, pelletsNumber;
    [SerializeField] float rateOfFire, spread, range, reloadTime;
    [SerializeField] bool allowButtonHold, isShotgun;
    int bulletsLeft, bulletsShot;
    bool shooting, readyToShoot, reloading;


    //aim down sight
    private Vector3 originalPosition;
    private float originalFOV;
    [SerializeField] private Vector3 aimPositon;
    [SerializeField] private float adsSpeed = 8f;
    [SerializeField] private float adsFOV = 30f;

    //Recoil
    [SerializeField] private float recoilX, recoilY, recoilZ;
    [SerializeField] private Vector3 gunMovementPosition;
    [SerializeField] private float shotRecoilSpeed = 1f;
    //[SerializeField] private GameObject CameraRecoil;
    private Gun_Recoil Recoil_Script;

    //PlayerCamera
    public Camera fpsCam;
    

    //Graphics
    public RaycastHit rayHit;
    public GameObject bulletHoleGraphic;
    public ParticleSystem muzzleFlash;
    GameObject text;
    GameObject Canvas;
    public AudioSource GunSound;

    private void Start()
    {
        //Canvas = GameObject.Find("Canvas");
       // text = Canvas.transform.GetChild(0).gameObject;

        //fpsCam = Camera.mai;
        bulletsLeft = magazinSize;
        readyToShoot = true;
        Recoil_Script = GetComponentInParent<Gun_Recoil>();
        originalPosition = transform.localPosition;
        originalFOV = fpsCam.fieldOfView;
    
    }
    private void Update()
    {
        MyInput();
        //text.GetComponent<TextMeshProUGUI>().SetText(bulletsLeft + " / " + magazinSize);
        ADS();
        if (reloading == true)
        {
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition + new Vector3(0,-0.4f,0), Time.deltaTime*8);
        }
    }

    private void MyInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazinSize && !reloading)
        {
            Reload();
        }

        // shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            Shoot();

        }

    }
    private void Shoot()
    {

        if (isShotgun == true)
        {
            allowButtonHold = false;
            for (int i = 0; i < pelletsNumber; i++)
            {
                FireBullets();
            }
        }
        else
        {
            FireBullets();
        }

        Recoil_Script.RecoilFire(recoilX, recoilY, recoilZ);
        GunMovementRecoil();
        muzzleFlash.Play();
        GunSound.Play();
        bulletsLeft--;
        Invoke("ResetShot", rateOfFire);
    }

    private void FireBullets()
    {
        readyToShoot = false;
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);
        //spreed
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, z);
        int layerMask = 1 << 9;
        layerMask = ~ layerMask;
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, layerMask))
        {
            if (rayHit.collider.CompareTag("Enemy"))
            {
                rayHit.collider.GetComponent<AbstractEnemyController>().TakeDamage(damage);
            }
        }
        GameObject bulletHole = Instantiate(bulletHoleGraphic, rayHit.point + rayHit.normal * 0.00001f, Quaternion.identity);
        bulletHole.transform.LookAt(rayHit.point + rayHit.normal);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        
        Invoke("ReloadFinished", reloadTime);

    }

    private void ReloadFinished()
    {
        bulletsLeft = magazinSize;
        reloading = false;
    }

    private void GunMovementRecoil()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, gunMovementPosition, Time.deltaTime * shotRecoilSpeed);
    }


    //ADS function
    private void ADS()
    {

        if (Input.GetKey(KeyCode.Mouse1) && !reloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPositon, Time.deltaTime * adsSpeed);
            fpsCam.fieldOfView = adsFOV;
        }
        else 
        {
            fpsCam.fieldOfView = originalFOV;
            if(!reloading)
            {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * adsSpeed);
            
            }
        }
    }


}
