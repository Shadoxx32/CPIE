using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using UnityEngine.Rendering;
using System.IO;
using UnityPipeline.Microsoft.CodeAnalysis.CSharp.Syntax;

public class GridPicker : MonoBehaviour
{
    [SerializeField] private Camera cameraJeu;  
    private PlotView caseSelectionnee;
    [SerializeField] private SauvegardeScene save;
    [SerializeField] private InventaireFerme inventaireFerme;
    [SerializeField] private MessageUI messageUI;
    [SerializeField] private TMP_Text grainesUI;
    [SerializeField] private TMP_Text recoltesUI;
    [SerializeField] private TMP_Text monnaieUI;
    [SerializeField] private TMP_Text infosCase;
    [SerializeField] private TMP_Text toolUI;

    private InventoryData tempdata;
    public OutilFerme currTool = OutilFerme.SELECTION;


    void Start()
    {
        ActualiserInterface();
        currTool = OutilFerme.SELECTION;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame)
        {
            if (caseSelectionnee != null)
            {
                caseSelectionnee.Labourer();
            }
            else
            {
                messageUI.PrintMessage("Sélectionne une case avant de labourer.");
            }
        }

        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (caseSelectionnee != null)
            {
                if (inventaireFerme.IsEnoughSeeds()) 
                {
                    if (caseSelectionnee.Planter())
                    {
                        inventaireFerme.ConsumeSeeds();
                        ActualiserInterface();
                    }
                } else { messageUI.PrintMessage("Plus de graines!");}
            }
            else
            {
                messageUI.PrintMessage("Sélectionne une case avant de planter.");
            }
        }

        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (caseSelectionnee != null)
            {
                if (caseSelectionnee.Recolter())
                {
                    inventaireFerme.AddCrop();
                    ActualiserInterface();
                }
            } else {messageUI.PrintMessage("Aucune case n'est sélectionée!");}
        }

        if (Keyboard.current != null && Keyboard.current.vKey.wasPressedThisFrame)
        {
            if(inventaireFerme.SellCrops())
            {
                messageUI.PrintMessage("Récolte vendue!");
            } else {messageUI.PrintMessage("Vous n'avez plus de récoltes!");}
            ActualiserInterface();
        }

        if (Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame)
        {
            if (inventaireFerme.BuySeeds())
            {
                messageUI.PrintMessage("Graine vendue!");
            } else {messageUI.PrintMessage("Vous n'avez pas assez d'argent!");}
            ActualiserInterface();
        }

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (caseSelectionnee != null){
                caseSelectionnee.Arroser();
                }
        }

        if (Keyboard.current != null && Keyboard.current.f5Key.wasPressedThisFrame)
        {
            tempdata = inventaireFerme.ExporterDonnees();
            if(save.Sauvegarder()){messageUI.PrintMessage("Partie sauvegardée!");} else{messageUI.PrintMessage("Sauvegarde impossible");}
        }

        if (Keyboard.current != null && Keyboard.current.f9Key.wasPressedThisFrame)
        {
            if(save.Charger()) {messageUI.PrintMessage("Données restaurées!"); ActualiserInterface();} else {messageUI.PrintMessage("Erreur lors du chargement de la sauvegarde");}
            
               
        }

        if (Keyboard.current != null && Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currTool = OutilFerme.SELECTION;
            
               
        }

        if (Keyboard.current != null && Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currTool = OutilFerme.LABOURER;
               
        }

        if (Keyboard.current != null && Keyboard.current.digit3Key.wasPressedThisFrame)
        {
           currTool = OutilFerme.PLANTER;
               
        }

        if (Keyboard.current != null && Keyboard.current.digit4Key.wasPressedThisFrame)
        {
          currTool = OutilFerme.ARROSER;
            
               
        }

        if (Keyboard.current != null && Keyboard.current.digit5Key.wasPressedThisFrame)
        {
           currTool = OutilFerme.RECOLTER;
               
        }



        if (Mouse.current == null || cameraJeu == null)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 positionSouris = Mouse.current.position.ReadValue();

            if (caseSelectionnee != null)
                {
                    caseSelectionnee.AfficherSelection(false);
                    caseSelectionnee = null;
            
                }

            Ray rayon = cameraJeu.ScreenPointToRay(positionSouris);

            if (Physics.Raycast(rayon, out RaycastHit impact))
            {
                PlotView caseCliquee = impact.collider.GetComponent<PlotView>();

                if (caseCliquee != null)
                {
                    caseSelectionnee = caseCliquee;
                    caseSelectionnee.AfficherSelection(true);
                    caseSelectionnee.AfficherCoordonnees();
                    ExecuterOutil(currTool);
                    ActualiserInterface();
                }
            }
            
        }
        ActualiserInfosCase();

        
    }


    public void ActualiserInterface()
    {
        grainesUI.text = "Graines: " + inventaireFerme.GetNbSeeds();
        recoltesUI.text = "Plantes récoltées : " + inventaireFerme.GetCrops();
        monnaieUI.text = "Argent : " + inventaireFerme.GetMoney();
    }
    public void ActualiserInfosCase()
    {
        if (caseSelectionnee != null)
        {
            infosCase.text = caseSelectionnee.AfficherInfosCase();
        } else
        {
            infosCase.text = "Aucune case n'est sélectionnée";
        }
    }

public void ExecuterOutil(OutilFerme outil)
    {
        if (outil == OutilFerme.LABOURER)
        {
            caseSelectionnee.Labourer();
            return;
        } else if(outil == OutilFerme.ARROSER) {
            caseSelectionnee.Arroser();
            return;
        } else if(outil == OutilFerme.PLANTER)
        {
            Plant();
            return;
        }else if(outil == OutilFerme.RECOLTER)
        {
            Harvest();
            return;    
        }
        
        
    }

public void Harvest()
    {
        if (caseSelectionnee.Recolter())
                {
                    inventaireFerme.AddCrop();
                    ActualiserInterface();
                    
                } else { messageUI.PrintMessage("Il n'y a rien à récolter sur cette case!");}
    }

public void Plant()
    {
        if (inventaireFerme.IsEnoughSeeds()) 
                {
                    if (caseSelectionnee.Planter())
                    {
                        inventaireFerme.ConsumeSeeds();
                        ActualiserInterface();
                    }
                } else { messageUI.PrintMessage("Plus de graines!");}
    }


    
}
