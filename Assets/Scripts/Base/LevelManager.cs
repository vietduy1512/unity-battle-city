using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    int smallTanksInput = 0,
        fastTanksInput = 0,
        bigTanksInput = 0,
        armoredTanksInput = 0,
        stageNumber = 0;

    [SerializeField]
    float spawnRateInput = 5;

    public static int smallTanks,
        fastTanks,
        bigTanks,
        armoredTanks;

    public static float spawnRate { get; private set; }

    private void Awake()
    {
        GamePlayManager.stageNumber = stageNumber;
        smallTanks = smallTanksInput;
        fastTanks = fastTanksInput;
        bigTanks = bigTanksInput;
        armoredTanks = armoredTanksInput;
        spawnRate = spawnRateInput;
    }
}