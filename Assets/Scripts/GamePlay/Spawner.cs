using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    GameObject[] tanks;
    GameObject tank;

    [SerializeField]
    bool isPlayer;

    [SerializeField]
    GameObject smallTank, fastTank, bigTank, armoredTank;

    enum TankType
    {
        SmallTank, FastTank, BigTank, ArmoredTank
    };

    void Start()
    {
        if (isPlayer)
        {
            tanks = new GameObject[1] { smallTank };
        }
        else
        {
            tanks = new GameObject[4] { smallTank, fastTank, bigTank, armoredTank };
        }
    }

    public void StartSpawning()
    {
        if (isPlayer)
        {
            tank = Instantiate(tanks[0], transform.position, transform.rotation);
        }
        else
        {
            SpawnEnemyTank();
        }
    }

    public void SpawnNewTank()
    {
        if (tank != null) tank.SetActive(true);
    }

    private void SpawnEnemyTank()
    {
        List<int> tankToSpawn = new List<int>();
        tankToSpawn.Clear();

        if (LevelManager.smallTanks > 0) tankToSpawn.Add((int)TankType.SmallTank);
        if (LevelManager.fastTanks > 0) tankToSpawn.Add((int)TankType.FastTank);
        if (LevelManager.bigTanks > 0) tankToSpawn.Add((int)TankType.BigTank);
        if (LevelManager.armoredTanks > 0) tankToSpawn.Add((int)TankType.ArmoredTank);

        int tankID = tankToSpawn[Random.Range(0, tankToSpawn.Count)];
        tank = Instantiate(tanks[tankID], transform.position, transform.rotation);

        if (tankID == (int)TankType.SmallTank) LevelManager.smallTanks--;
        else if (tankID == (int)TankType.FastTank) LevelManager.fastTanks--;
        else if (tankID == (int)TankType.BigTank) LevelManager.bigTanks--;
        else if (tankID == (int)TankType.ArmoredTank) LevelManager.armoredTanks--;

        GamePlayManager GPM = GameObject.Find(Constants.GameObject.STAGE_ESSENTIAL).GetComponent<GamePlayManager>();
        GPM.RemoveTankReserve();
    }
}
