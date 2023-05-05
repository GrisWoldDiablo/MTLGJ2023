using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

//Class to periodically spawn lava fire balls from the sky
//Potentially leave pool of lava on the ground?
public class EruptionEventManager : MonoBehaviour
{

    private static EruptionEventManager _sInstance;

    [SerializeField] GameObject DefaultEruptionObject;
    [SerializeField] GameObject RockEruptionObject;
    
    private List<GameObject> activeEruptions = new List<GameObject>();

    //potentially have a formula based on elapsed game time (happens more frequently as we progress?)
    [SerializeField] float minTimeBetweenEvents = 5.0f;
    [SerializeField] float maxTimeBetweenEvents = 15.0f;

    private float randomTimeRequired = 0.0f;
    private float timeSinceLastEvent = 0.0f;

    [Header("Debug")]
    [SerializeField] private bool disableEruption;

    public List<GameObject> ActiveEruptions { get => activeEruptions; set => activeEruptions = value; }
    public static EruptionEventManager Get()
    {
        return _sInstance;
    }

    private GameObject GetEventForBiome()
    {
        GameObject SpawnedEvent = DefaultEruptionObject;
        EBiomeType currentBiome = ProceduralEnvGenerator.Get().GetCurrentBiomeType();
        switch (currentBiome)
        {
            case EBiomeType.Cave:
                SpawnedEvent = RockEruptionObject;
                break;
        }
        return SpawnedEvent;
    }

    private void OnBiomeChanged()
    {
        //destroy active eruptions as we switch biomes
        foreach(var eruption in ActiveEruptions)
        {
            Destroy(eruption.gameObject);
        }
        ActiveEruptions.Clear();
    }

    private void SpawnEruptionEvent()
    {
        if (DefaultEruptionObject != null)
        {
            GameObject BiomeEvent = GetEventForBiome();
            GameObject Event = Instantiate(BiomeEvent, GetRandomSpawnPos(), Quaternion.identity, gameObject.transform);
            EruptionSFXManager.Get().PlaySFX();
            ActiveEruptions.Add(Event);
        }
    }
    float GetRandomTimeForSpawn()
    {
        return Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents); 
    }

    Vector3 GetRandomSpawnPos()
    {
        float screenWidth = Screen.width;
        float screenWidthSlice = screenWidth / 3.0f;
        float screenHeight = Screen.height;

        // Dont consider first 1/3 of screen for spawning
        Vector3 leftEdge = Camera.main.ScreenToWorldPoint(new Vector3(screenWidthSlice, 0, 0));
        Vector3 rightEdge = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, 0, 0));
        Vector3 top = Camera.main.ScreenToWorldPoint(new Vector3(0, screenHeight, 0));

        // Get a random x-coordinate between the left and right edges of the screen
        float randomX = Random.Range(leftEdge.x, rightEdge.x);

        // Create a random position with the random x-coordinate and the current y- and z-coordinates
        Vector3 randomPosition = new Vector3(randomX, top.y, transform.position.z);
        return randomPosition;
    }

    private void Awake()
    {
        if (!_sInstance)
        {
            _sInstance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        randomTimeRequired = GetRandomTimeForSpawn();
        ProceduralEnvGenerator Gen = ProceduralEnvGenerator.Get();

        Gen.OnBiomeChange += OnBiomeChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (disableEruption)
        {
            return;
        }

        if(timeSinceLastEvent < randomTimeRequired)
        {
            timeSinceLastEvent += Time.deltaTime;
        }
        else
        {
            SpawnEruptionEvent();
            timeSinceLastEvent = 0.0f;
        }
    }
}
