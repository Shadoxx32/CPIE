using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class FarmGrid : MonoBehaviour
{
    [SerializeField] private GameObject plotPrefab;
    [SerializeField] private int largeur = 3;
    [SerializeField] private int profondeur = 2;
    private List<PlotView> plots;

    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        plots = new List<PlotView>();
        for (int x = 0; x < largeur; x++)
        {
            for (int z = 0; z < profondeur; z++)
            {
                Vector3 position = new Vector3(x, 0.05f, z);

                GameObject nouvelleCase = Instantiate(
                    plotPrefab, position, Quaternion.identity, transform
                );

                PlotView vue = nouvelleCase.GetComponent<PlotView>();
                vue.Initialiser(x, z);
                plots.Add(vue);
                
            }
        }
        Debug.Log(plots.Count);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public List<PlotData> ExporterDonnees()
    {
        List<PlotData> plotDatas;
        plotDatas = new List<PlotData>();
        foreach (PlotView plot in plots) {
            PlotData export = plot.ExporterDonnees();
            plotDatas.Add(export);
        }
        return plotDatas;
    }

    public PlotView TrouverCase(int x, int z)
    {
        foreach (PlotView plot in plots)
        {
            if(x == plot.GetX() && z == plot.GetZ())
            {
                return plot;
            }
        }
        return null;
    }

    public void ImporterDonnees(List<PlotData> plotData)
    {
        if(plotData == null){
            return;
            }
        foreach(PlotData data in plotData)
        {

            PlotView caseTrouvee = TrouverCase(data.xData, data.zData);
            if(caseTrouvee != null)
            {
                caseTrouvee.ImporterDonnees(data);
            } else {Debug.Log("Coordonnées introuvables");}

        }
    }

    public bool ValiderDonnees(List<PlotData> plot)
    {
        if (plot == null || plot.Count != plots.Count)
        {
            return false;
        }
        List<PlotView> doublon;
        doublon = new List<PlotView>();
        foreach (PlotData data in plot)
        {
            if (data == null)
            {
                return false;
            }
            
            PlotView caseCheckee = TrouverCase(data.xData, data.zData);
            if (caseCheckee == null) {return false;}
            if(doublon.Contains(caseCheckee)) {return false;} 
            else {doublon.Add(caseCheckee);}
            if(!(data.etatSolData == EtatSol.Herbe ||data.etatSolData == EtatSol.Labouree||data.etatSolData == EtatSol.Plantee||data.etatSolData == EtatSol.Mature))
            {
                return false;
            }
            if(data.growthTimerData < 0) {return false;}
        }
        
        return true; 
    }
}
