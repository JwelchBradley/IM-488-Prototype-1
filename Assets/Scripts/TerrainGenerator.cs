using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public float startSafeRangeAsteroid;

    public float asteroidSpawnRange;
    public float amountToSpawnAsteroid1;
    public float amountToSpawnAsteroid2;
    public float amountToSpawnAsteroid3;

    public GameObject asteroid1;
    public GameObject asteroid2;
    public GameObject asteroid3;

    public float startSafeRangeHealStation;

    public float healStationSpawnRange;
    public float amountToSpawnHealStation;

    public GameObject healStation;

    public float startSafeRangeTurret;

    public float turretSpawnRange;
    public float amountToSpawnTurretComp;
    public float amountToSpawnTurretHeavy;
    public float amountToSpawnTurretMissile;
    public float amountToSpawnTurretV2;

    public GameObject turretComp;
    public GameObject turretHeavy;
    public GameObject turretMissile;
    public GameObject turretV2;

    private Vector3 spawnPoint;
    private List<GameObject> objectsToPlaceAsteroid1 = new List<GameObject>();
    private List<GameObject> objectsToPlaceAsteroid2 = new List<GameObject>();
    private List<GameObject> objectsToPlaceAsteroid3 = new List<GameObject>();
    private List<GameObject> objectsToPlaceHealStation = new List<GameObject>();
    private List<GameObject> objectsToPlaceTurretComp = new List<GameObject>();
    private List<GameObject> objectsToPlaceTurretHeavy = new List<GameObject>();
    private List<GameObject> objectsToPlaceTurretMissile = new List<GameObject>();
    private List<GameObject> objectsToPlaceTurretV2 = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountToSpawnAsteroid1; i++)
        {
            PickSpawnPointAsteroid();

            //pick new spawn point if too close to player start
            while (Vector3.Distance(spawnPoint, Vector3.zero) < startSafeRangeAsteroid)
            {
                PickSpawnPointAsteroid();
            }

            objectsToPlaceAsteroid1.Add(Instantiate(asteroid1, spawnPoint, Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f))));
            objectsToPlaceAsteroid1[i].transform.parent = this.transform;
        }

        for (int i = 0; i < amountToSpawnAsteroid2; i++)
        {
            PickSpawnPointAsteroid();

            //pick new spawn point if too close to player start
            while (Vector3.Distance(spawnPoint, Vector3.zero) < startSafeRangeAsteroid)
            {
                PickSpawnPointAsteroid();
            }

            objectsToPlaceAsteroid2.Add(Instantiate(asteroid2, spawnPoint, Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f))));
            objectsToPlaceAsteroid2[i].transform.parent = this.transform;
        }

        for (int i = 0; i < amountToSpawnAsteroid3; i++)
        {
            PickSpawnPointAsteroid();

            //pick new spawn point if too close to player start
            while (Vector3.Distance(spawnPoint, Vector3.zero) < startSafeRangeAsteroid)
            {
                PickSpawnPointAsteroid();
            }

            objectsToPlaceAsteroid3.Add(Instantiate(asteroid3, spawnPoint, Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f))));
            objectsToPlaceAsteroid3[i].transform.parent = this.transform;
        }

        for (int i = 0; i < amountToSpawnHealStation; i++)
        {
            PickSpawnPointHealStation();

            //pick new spawn point if too close to player start
            while (Vector3.Distance(spawnPoint, Vector3.zero) < startSafeRangeHealStation)
            {
                PickSpawnPointHealStation();
            }

            objectsToPlaceHealStation.Add(Instantiate(healStation, spawnPoint, Quaternion.Euler(0f, 0f, 0f)));
            objectsToPlaceHealStation[i].transform.parent = this.transform;
        }

        for (int i = 0; i < amountToSpawnTurretComp; i++)
        {
            PickSpawnPointTurret();

            //pick new spawn point if too close to player start
            while (Vector3.Distance(spawnPoint, Vector3.zero) < startSafeRangeTurret)
            {
                PickSpawnPointTurret();
            }

            objectsToPlaceTurretComp.Add(Instantiate(turretComp, spawnPoint, Quaternion.Euler(0f, 0f, 0f)));
            objectsToPlaceTurretComp[i].transform.parent = this.transform;
        }

        for (int i = 0; i < amountToSpawnTurretHeavy; i++)
        {
            PickSpawnPointTurret();

            //pick new spawn point if too close to player start
            while (Vector3.Distance(spawnPoint, Vector3.zero) < startSafeRangeTurret)
            {
                PickSpawnPointTurret();
            }

            objectsToPlaceTurretHeavy.Add(Instantiate(turretHeavy, spawnPoint, Quaternion.Euler(0f, 0f, 0f)));
            objectsToPlaceTurretHeavy[i].transform.parent = this.transform;
        }

        for (int i = 0; i < amountToSpawnTurretMissile; i++)
        {
            PickSpawnPointTurret();

            //pick new spawn point if too close to player start
            while (Vector3.Distance(spawnPoint, Vector3.zero) < startSafeRangeTurret)
            {
                PickSpawnPointTurret();
            }

            objectsToPlaceTurretMissile.Add(Instantiate(turretMissile, spawnPoint, Quaternion.Euler(0f, 0f, 0f)));
            objectsToPlaceTurretMissile[i].transform.parent = this.transform;
        }

        for (int i = 0; i < amountToSpawnTurretV2; i++)
        {
            PickSpawnPointTurret();

            //pick new spawn point if too close to player start
            while (Vector3.Distance(spawnPoint, Vector3.zero) < startSafeRangeTurret)
            {
                PickSpawnPointTurret();
            }

            objectsToPlaceTurretV2.Add(Instantiate(turretV2, spawnPoint, Quaternion.Euler(0f, 0f, 0f)));
            objectsToPlaceTurretV2[i].transform.parent = this.transform;
        }

        /*
        asteroid1.SetActive(false);
        asteroid2.SetActive(false);
        asteroid3.SetActive(false);
        healStation.SetActive(false);
        turretComp.SetActive(false);
        turretHeavy.SetActive(false);
        turretMissile.SetActive(false);
        turretV2.SetActive(false);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickSpawnPointAsteroid()
    {
        spawnPoint = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f));

        if (spawnPoint.magnitude > 1)
        {
            spawnPoint.Normalize();
        }

        spawnPoint *= asteroidSpawnRange;
    }

    public void PickSpawnPointHealStation()
    {
        spawnPoint = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f));

        if (spawnPoint.magnitude > 1)
        {
            spawnPoint.Normalize();
        }

        spawnPoint *= healStationSpawnRange;
    }

    public void PickSpawnPointTurret()
    {
        spawnPoint = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f));

        if (spawnPoint.magnitude > 1)
        {
            spawnPoint.Normalize();
        }

        spawnPoint *= turretSpawnRange;
    }
}
