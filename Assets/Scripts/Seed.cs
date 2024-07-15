using System.Collections;
using UnityEngine;
using Enums;
using UnityEngine.UI;
public class Seed : WorldItem
{
    public float GrowTime = 5f;
    private float GrowCountdown;
    private SeedState seedState = SeedState.None;
    [SerializeField] private Crop crop;
    [HideInInspector] public Farmland plantedFarm = null;
    public int HarvestCount = 1;
    public Image GrowIndicator;

    public void BeginGrow()
    {
        GrowCountdown = 0;
        StartCoroutine(CoBeginGrow());
    }

    private IEnumerator CoBeginGrow()
    {
        seedState = SeedState.Growing;
        yield return new WaitForSeconds(GrowTime);
        seedState = SeedState.GrewUp;
    }

    private void Update()
    {
        if (seedState == SeedState.Growing)
        {
            GrowCountdown += Time.deltaTime;
            GrowIndicator.fillAmount = GrowCountdown / GrowTime;
        }
        else if (seedState == SeedState.GrewUp)
        {
            GrowIndicator.color = Color.green;
        }
    }

    public bool HarvestCrop(int ToolEfficiency)
    {
        switch (seedState)
        {
            case SeedState.GrewUp:
                StartCoroutine(Harvest(ToolEfficiency));
                return true;
            default:
                Debug.Log("NotReadyForHarvest");
                return false;
        }
    }

    private IEnumerator Harvest(int ToolEfficiency)
    {
        plantedFarm.seeds.Remove(this);
        for (int i = 0; i < HarvestCount * ToolEfficiency; i++)
        {
            GameObject temp = Instantiate(crop).gameObject;
            temp.transform.position = transform.position + Vector3.up;
            temp.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(10, 15), Random.Range(10, 15), Random.Range(10, 15)));
            yield return new WaitForSeconds(.5f);
        }
        Destroy(gameObject);
    }
}
