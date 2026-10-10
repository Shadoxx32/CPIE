using TMPro;
using UnityEngine;

public class CurrentToolUI : MonoBehaviour
{
    [SerializeField] TMP_Text currentTool;
    [SerializeField] GridPicker grid;
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        currentTool.text = FormaterToolUI(grid.currTool);
    }

    string FormaterToolUI(OutilFerme tool)
    {
        if (tool == OutilFerme.SELECTION)
        {
           return "Sélectionner"; 
        }

        else if (tool == OutilFerme.LABOURER)
        {
           return "Labourer"; 
        }

        else if (tool == OutilFerme.PLANTER)
        {
           return "Planter"; 
        }

        else if (tool == OutilFerme.ARROSER)
        {
           return "Arroser"; 
        }
        else
        {
           return "Récolter"; 
        }
    }
}
