using System;
using System.IO;
using UnityEngine;

public class SauvegardeScene : MonoBehaviour
{
    [SerializeField] InventaireFerme inventaireFerme;
    [SerializeField] FarmGrid farmGrid;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Sauvegarder()
    {
        
        InventoryData inventoryData = inventaireFerme.ExporterDonnees();
        FarmData farmData = new FarmData();
        farmData.inventory = inventoryData;
        farmData.cases = farmGrid.ExporterDonnees();
        string inv = JsonUtility.ToJson(farmData, true);
        string invChemin = Path.Combine(Application.persistentDataPath, "ferme.json");
        File.WriteAllText(invChemin, inv);
        return true;
    }

    public bool Charger()
    {
        string chemin = Path.Combine(Application.persistentDataPath, "ferme.json");
        if(!File.Exists(chemin))
        {
            Debug.Log("Aucune sauvegarde disponible");
            return false;
        }
        try{
        string data = File.ReadAllText(chemin);
        FarmData save = JsonUtility.FromJson<FarmData>(data);
        if(save == null)
            {
                Debug.Log("Sauvegarde non trouvée");
                return false;
            }
        if (save.inventory == null || save.cases == null)
            {
                Debug.Log("Données non trouvées");
                return false;
            }
        if (save.inventory.argent <0 || save.inventory.nombreGraines < 0 || save.inventory.nombreRecoltes < 0)
            {
                Debug.Log("Données non conformes");
                return false;
            }
        if(!farmGrid.ValiderDonnees(save.cases)) {Debug.Log("Données de grille invalides"); return false;}
        inventaireFerme.ImporterDonnees(save.inventory);
        farmGrid.ImporterDonnees(save.cases);
        }
        catch(System.Exception erreur){
            Debug.Log(erreur.Message);
        }
        return true;
    }

    
}
