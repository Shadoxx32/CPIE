using System;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

public class PlotView : MonoBehaviour
{
    [SerializeField] public int x;
    [SerializeField] public int z;
    [SerializeField] private GameObject surbrillance;
    [SerializeField] private EtatSol etatSol = EtatSol.Herbe;
    [SerializeField] private Renderer renduSol;
    [SerializeField] private Material materiauHerbe;
    [SerializeField] private Material materiauLaboure;
    [SerializeField] private Material materiauArrose;
    [SerializeField] private GameObject pousse;
    [SerializeField] private GameObject planteMature;
    [SerializeField] private float growthTime = 10;

    private bool iswatered = false;
    private float growthPercentage;
    
    private float growthTimer = 0;

    void Start()
    {
        ActualiserApparence();
    }

    // Update is called once per frame
    void Update()
    {
        if (etatSol == EtatSol.Plantee && iswatered)
        {
            growthTimer += Time.deltaTime;
            if (growthTimer >= growthTime)
            {
                etatSol = EtatSol.Mature;
                pousse.SetActive(false);
                planteMature.SetActive(true);
                Debug.Log("Culture prête a être recoltée!");
            }
        }
    }

    public void Initialiser(int nouvelleX, int nouvelleZ)
    {
        x = nouvelleX;
        z = nouvelleZ;
    }

    public void AfficherCoordonnees()
    {
        Debug.Log("Case sélectionnée : " + x + ", " + z);
    }

    public void AfficherSelection(bool visible)
    {
        surbrillance.SetActive(visible);
    }
    public void Labourer()
    {
        if (etatSol == EtatSol.Herbe) {
            etatSol = EtatSol.Labouree;
            ActualiserApparence();
            Debug.Log("Case labourée!");
        } else
        {
            Debug.Log("Cette case ne peut pas être labourée!");
        }
    }
    private void ActualiserApparence()
    {
        if (etatSol == EtatSol.Herbe)
        {
            renduSol.sharedMaterial = materiauHerbe;
            pousse.SetActive(false);
            planteMature.SetActive(false);
        }
        else if (etatSol == EtatSol.Labouree)
        {
            if (iswatered)
            {
                renduSol.sharedMaterial = materiauArrose;
            } else 
            {
                renduSol.sharedMaterial = materiauLaboure;
            }
            pousse.SetActive(false);
            planteMature.SetActive(false);
        } else if (etatSol == EtatSol.Plantee) {
            if (iswatered)
            {
                renduSol.sharedMaterial = materiauArrose;
            } else 
            {
                renduSol.sharedMaterial = materiauLaboure;
            }
            pousse.SetActive(true);
            planteMature.SetActive(false);
        } else if (etatSol == EtatSol.Mature) {
            if (iswatered)
            {
                renduSol.sharedMaterial = materiauArrose;
            } else 
            {
                renduSol.sharedMaterial = materiauLaboure;
            }
            planteMature.SetActive(true);
            pousse.SetActive(false);
        }

    }

    public bool Planter()
    {
        if (etatSol == EtatSol.Labouree)
        {
            etatSol = EtatSol.Plantee;
            pousse.SetActive(true);
            growthTimer = 0;
            iswatered = false;
            return true;
        }

        Debug.Log("Il faut une case labourée et vide pour planter.");
        return false;
    }

    public bool Recolter()
    {
        if (etatSol == EtatSol.Mature)
        {
            planteMature.SetActive(false);
            etatSol = EtatSol.Labouree;
            renduSol.sharedMaterial = materiauLaboure;
            return true;
        } else {
            Debug.Log("Cette case n'a pas de culture prête à être récoltée!");
            return false;
            }
    }

    public string AfficherInfosCase()
    {
        growthPercentage = growthTimer / growthTime * 100;
        if (etatSol == EtatSol.Plantee)
        {   
            if(iswatered){
                return "Case sélectionée :\n  X: " + x + "\n  Y : " + z + "\n  Etat: " + etatSol + "\n  Croissance : " + Math.Floor(growthPercentage) + "%";
            } else
            {return "Case sélectionée :\n  X: " + x + "\n  Y : " + z + "\n  Etat: " + etatSol + "\n  A arroser";}
        } else 
        {return "Case sélectionée :\n  X: " + x + "\n  Y : " + z + "\n  Etat: " + etatSol;}
    }

    public void Arroser()
    {
         if (etatSol == EtatSol.Plantee && !iswatered)
        {
            renduSol.sharedMaterial = materiauArrose;
            iswatered = true;
        } else {Debug.Log("On en peut arroser que sur une graine non arrosée et non mature!");}
    }

    public PlotData ExporterDonnees()
    {
        return new PlotData
        {
            xData = x,
            zData = z,
            etatSolData = etatSol,
            growthTimerData = growthTimer,
            isWateredData = iswatered
        };
    }

    public void ImporterDonnees(PlotData data)
    {
        if (data != null) {
        etatSol = data.etatSolData;
        growthTimer = data.growthTimerData;
        iswatered = data.isWateredData;
        ActualiserApparence();
        }
    }

    public int GetX()
    {
        return x;
    }

    public int GetZ()
    {
        return z;
    }

}
