using UnityEngine;

public class InventaireFerme : MonoBehaviour
{
    [SerializeField] private MessageUI messageUI;
    [SerializeField] private int nbSeeds = 6; 
    [SerializeField] private int crops = 0;
    [SerializeField] private int money = 0;
    [SerializeField] private int seedsCost = 2;
    [SerializeField] private int cropsCost = 5;

    public InventoryData ExporterDonnees()
    {
        return new InventoryData
        {
            nombreGraines = nbSeeds,
            nombreRecoltes = crops,
            argent = money
        };
    }

    public void ImporterDonnees(InventoryData donnees)
    {
        if (donnees == null)
        {
            return;
        }

        nbSeeds = donnees.nombreGraines;
        crops = donnees.nombreRecoltes;
        money = donnees.argent;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool SellCrops()
    {
        if (crops > 0)
            {
                crops -= 1;
                money += cropsCost;
                return true;
            } else
            {
                return false;
            }
    }

    public bool BuySeeds()
    {
        if (money >= seedsCost)
            {
                nbSeeds += 1;
                money -= seedsCost;
                return true;
            } else
            {
                return false;
            }
    }
    
    public bool IsEnoughSeeds()
    {
        if (nbSeeds > 0)
        {
            return true;
        } else {
            return false;
        }
    }

    public void ConsumeSeeds()
    {
        nbSeeds -= 1;
    }

    public void AddCrop()
    {
        crops += 1;
    }

    public int GetNbSeeds()
    {
        return nbSeeds;
    }

    public int GetCrops()
    {
        return crops;
    }

    public int GetMoney()
    {
        return money;
    }
}
