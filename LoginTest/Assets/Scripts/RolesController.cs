using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RolesController : MonoBehaviour
{
     public GameObject Panel, PanelFW, PanelAV, PanelBD, PanelQM, PanelNMT, PanelVirus, PanelPhishing, PanelSpyware, PanelDatabase;

      public void OpenOptions()
    {
        Panel.SetActive(true);
        PanelFW.SetActive(false);
        PanelAV.SetActive(false);
        PanelBD.SetActive(false);
        PanelQM.SetActive(false);
        PanelNMT.SetActive(false);
        PanelVirus.SetActive(false);
        PanelPhishing.SetActive(false);
        PanelSpyware.SetActive(false);
        PanelDatabase.SetActive(false);

    }
     public void OpenPanelFW()
    {
        Panel.SetActive(false);
        PanelFW.SetActive(true);
        PanelAV.SetActive(false);
        PanelBD.SetActive(false);
        PanelQM.SetActive(false);
        PanelNMT.SetActive(false);
        PanelVirus.SetActive(false);
        PanelPhishing.SetActive(false);
        PanelSpyware.SetActive(false);
        PanelDatabase.SetActive(false);

    }
     public void OpenPanelAV()
    {
        Panel.SetActive(false);
        PanelFW.SetActive(false);
        PanelAV.SetActive(true);
        PanelBD.SetActive(false);
        PanelQM.SetActive(false);
        PanelNMT.SetActive(false);
        PanelVirus.SetActive(false);
        PanelPhishing.SetActive(false);
        PanelSpyware.SetActive(false);
        PanelDatabase.SetActive(false);

    }
     public void OpenPanelBD()
    {
        Panel.SetActive(false);
        PanelFW.SetActive(false);
        PanelAV.SetActive(false);
        PanelBD.SetActive(true);
        PanelQM.SetActive(false);
        PanelNMT.SetActive(false);
        PanelVirus.SetActive(false);
        PanelPhishing.SetActive(false);
        PanelSpyware.SetActive(false);
        PanelDatabase.SetActive(false);

    }
     public void OpenPanelQM()
    {
        Panel.SetActive(false);
        PanelFW.SetActive(false);
        PanelAV.SetActive(false);
        PanelBD.SetActive(false);
        PanelQM.SetActive(true);
        PanelNMT.SetActive(false);
        PanelVirus.SetActive(false);
        PanelPhishing.SetActive(false);
        PanelSpyware.SetActive(false);
        PanelDatabase.SetActive(false);

    }
     public void OpenPanelNMT()
    {
        Panel.SetActive(false);
        PanelFW.SetActive(false);
        PanelAV.SetActive(false);
        PanelBD.SetActive(false);
        PanelQM.SetActive(false);
        PanelNMT.SetActive(true);
        PanelVirus.SetActive(false);
        PanelPhishing.SetActive(false);
        PanelSpyware.SetActive(false);
        PanelDatabase.SetActive(false);

    }
     public void OpenPanelVirus()
    {
        Panel.SetActive(false);
        PanelFW.SetActive(false);
        PanelAV.SetActive(false);
        PanelBD.SetActive(false);
        PanelQM.SetActive(false);
        PanelNMT.SetActive(false);
        PanelVirus.SetActive(true);
        PanelPhishing.SetActive(false);
        PanelSpyware.SetActive(false);
        PanelDatabase.SetActive(false);
    }
        public void OpenPanelPhishing()
    {
        Panel.SetActive(false);
        PanelFW.SetActive(false);
        PanelAV.SetActive(false);
        PanelBD.SetActive(false);
        PanelQM.SetActive(false);
        PanelNMT.SetActive(false);
        PanelVirus.SetActive(false);
        PanelPhishing.SetActive(true);
        PanelSpyware.SetActive(false);
        PanelDatabase.SetActive(false);
    }
        public void OpenPanelSpyware()
    {
        Panel.SetActive(false);
        PanelFW.SetActive(false);
        PanelAV.SetActive(false);
        PanelBD.SetActive(false);
        PanelQM.SetActive(false);
        PanelNMT.SetActive(false);
        PanelVirus.SetActive(false);
        PanelPhishing.SetActive(false);
        PanelSpyware.SetActive(true);
        PanelDatabase.SetActive(false);
    }
        public void OpenPanelDatabase()
    {
        Panel.SetActive(false);
        PanelFW.SetActive(false);
        PanelAV.SetActive(false);
        PanelBD.SetActive(false);
        PanelQM.SetActive(false);
        PanelNMT.SetActive(false);
        PanelVirus.SetActive(false);
        PanelPhishing.SetActive(false);
        PanelSpyware.SetActive(false);
        PanelDatabase.SetActive(true);
    }
    
}
