using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotgunFire : MonoBehaviour
{
    [SerializeField] private float bulletRange;
    [SerializeField] private float maxBulletOffset;    
    [SerializeField] private int numBullets;

    [SerializeField] private AudioClip shotgunBlast;
    [SerializeField] private LayerMask canHitMask;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private TrailRenderer trail;

    private Transform cam;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        inputManager = InputManager.instance;    
    }

    public void Shoot()
    {
        if(inputManager.PlayerFired())
        {
            AudioManager.instance.Play3DAudio(shotgunBlast, bulletSpawn);
            Vector3 [] bulletDir = new Vector3[numBullets];
            for(int i = 0; i < bulletDir.Length; i++)
            {
                bulletDir[i] = cam.transform.TransformDirection(Vector3.forward);
                bulletDir[i] = new Vector3
                (
                    bulletDir[i].x + Random.Range(-maxBulletOffset,maxBulletOffset),
                    bulletDir[i].y + Random.Range(-maxBulletOffset,maxBulletOffset),
                    bulletDir[i].z + Random.Range(-maxBulletOffset,maxBulletOffset)
                );

                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, bulletDir[i], out hit, bulletRange, canHitMask))
                {
                    Debug.Log(hit.collider.gameObject);
                }
                TrailRenderer bulletTrail = Instantiate(trail,bulletSpawn.transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(bulletTrail, hit, bulletDir[i]));
            }    
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit, Vector3 dir)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;
        Vector3 endPosition = hit.point;
   
        if(endPosition.magnitude <= 0)
        {
            endPosition = cam.position + dir * bulletRange;
        }

        while(time < 1f)
        {
            trail.transform.position = Vector3.Lerp(startPosition,endPosition,time);
            time += Time.deltaTime /trail.time;
            yield return null;
        }
        trail.transform.position = endPosition;
        Destroy(trail.gameObject, trail.time);

    }
    
}