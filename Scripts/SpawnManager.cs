using Litkey.Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public enum eCountry
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten


}

public enum eRegion
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Eleven,
    Twelve,
    Thirteen,
    Fourteen,
    Fifteen,
    Sixteen,
    Seventeen,
    Eighteen,
    Nineteen,
    Town,
    Boss,
}

public enum eHuntMode
{
    afk,

}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    [SerializeField] private float spawnDelay = 5f;

    [SerializeField] private Transform spawnPosition;

    [SerializeField] private eCountry currentCountry;
    [SerializeField] private eRegion currentRegion;

    // �ʿ����� �ٸ� ���͸� ��ȯ
    private Dictionary<string, Pool<Health>> monsterDict;

    // ������ ����մ����� ���� ������ ���Ͱ� �ٸ�, ���� �����̴ٸ�
    [SerializeField] private Health monsterPrefab;

    private Health spawnedMonster;

    // TODO
    public List<MobInfo> mobs;

    // �ʿ� �� ��� ���� �����յ��� �̸� Ǯ�� ���� �����Ѵ�. 
    // �÷��̾ �ش� ������ ���������� �� ������ ���͵��� Ǯ�� Ȱ��ȭ ��Ų��. 
    Pool<Health> enemyPool;

    [SerializeField] private MapManager mapManager;

    private float spawnTimer = 5f;
    private static float spawnCounter = 0f;

    public static bool stopTimer;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }
        monsterDict = new Dictionary<string, Pool<Health>>();
        StopTimer();
    }

    public void Spawn()
    {
        StopTimer();
        var monsterToSpawn = mapManager.CurrentArea.monsterTable.GetRandomMonster();
        //var monsterToSpawn = MobManager.Instance.GetEnemy(eRegion.One);

        // ������ ���Ͱ� Ǯ�� ������ Ǯ�� ���� ��ųʸ��� ����
        if (!monsterDict.TryGetValue(monsterToSpawn.Name, out Pool<Health> pool))
        {
            monsterDict[monsterToSpawn.Name] = Pool.Create(monsterToSpawn, 3).NonLazy();
            Debug.Log($"{monsterToSpawn.Name}�� Ǯ�� ���� ����");
        }
        var mons = monsterDict[monsterToSpawn.Name].Get();
        mons.transform.position = spawnPosition.position;
        //var mons = Instantiate(, spawnPosition.position, Quaternion.identity);
        mons.OnDeath += OnMonsterDeath;
        spawnedMonster = mons;
    }

    private void OnMonsterDeath(LevelSystem attacker)
    {
        // Check if spawnedMonster is not null before proceeding
        if (spawnedMonster != null)
        {
            // Check if the key exists in the dictionary
            if (monsterDict.ContainsKey(spawnedMonster.Name))
            {
                spawnedMonster.OnDeath -= OnMonsterDeath;
                Debug.Log("Monster returned to pool: " + spawnedMonster.Name);
                // monsterDict[spawnedMonster.Name].Take(spawnedMonster);
            }
            else
            {
                Debug.LogError("����" + spawnedMonster.Name + " Ű�� �����ϴ�");
            }

            spawnedMonster = null;
        }
        else
        {
            Debug.LogWarning("spawnedMonster is null. Skipping monster return to pool.");
        }

        StartTimer();
    }

    public void TakeToPool(Health targ)
    {
        monsterDict[targ.Name].Take(targ);
    }

    private IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        Spawn();
    }

    void Start()
    {
        //Spawn();
        //Resources.LoadAll<Health>
        //monsterDict = new Dictionary<eRegion, Transform> { eRegion.FirstRegion, }
    }

    // Update is called once per frame
    void Update()
    {

        spawnCounter += Time.deltaTime;
        if (spawnCounter >= spawnTimer && !stopTimer)
        {

            Spawn();
            spawnCounter = 0f;
        }
    }

    public static void StopTimer()
    {
        stopTimer = true;

    }

    public static void StartTimer()
    {
        spawnCounter = 0f;
        stopTimer = false;
    }
}

public class MobInfo
{
    // ������ �ʿ��Ѱ�
    // ����, ���� ���̵�, ����
    public eCountry Country;
    public Health Mob;
    public int Level;
    public string RewardID;

}

