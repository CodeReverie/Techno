using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Itemshop : MonoBehaviour
{
   public GameObject mainshop, background, hats, visors, skins, pets;
   public GameObject topupmenu, specialoffers, bytecoins, freemos;

   public void OpenBackground(){
    background.SetActive(true);
    hats.SetActive(false);
    visors.SetActive(false);
    skins.SetActive(false);
    pets.SetActive(false);
   }
   
    public void OpenHats(){
    background.SetActive(false);
    hats.SetActive(true);
    visors.SetActive(false);
    skins.SetActive(false);
    pets.SetActive(false);
   }
    public void OpenVisors(){
    background.SetActive(false);
    hats.SetActive(false);
    visors.SetActive(true);
    skins.SetActive(false);
    pets.SetActive(false);
   }

    public void OpenSkins(){
    background.SetActive(false);
    hats.SetActive(false);
    visors.SetActive(false);
    skins.SetActive(true);
    pets.SetActive(false);
   }

    public void OpenPets(){
    background.SetActive(false);
    hats.SetActive(false);
    visors.SetActive(false);
    skins.SetActive(false);
    pets.SetActive(true);
   }


    
    public void OpenSpecial(){
    background.SetActive(false);
    hats.SetActive(false);
    visors.SetActive(false);
    skins.SetActive(false);
    pets.SetActive(false);
    mainshop.SetActive(false);
    topupmenu.SetActive(true);
    specialoffers.SetActive(true);
    bytecoins.SetActive(false);
    freemos.SetActive(false);
   }
   public void OpenByte(){
   background.SetActive(false);
    hats.SetActive(false);
    visors.SetActive(false);
    skins.SetActive(false);
    pets.SetActive(false);
    mainshop.SetActive(false);
    topupmenu.SetActive(true);
    //=============
    specialoffers.SetActive(false);
    bytecoins.SetActive(true);
    freemos.SetActive(false);
     
   }
   public void OpenFree(){
    background.SetActive(false);
    hats.SetActive(false);
    visors.SetActive(false);
    skins.SetActive(false);
    pets.SetActive(false);
    mainshop.SetActive(false);
    topupmenu.SetActive(true);
    //=============
    specialoffers.SetActive(false);
    bytecoins.SetActive(false);
    freemos.SetActive(true);
   }
    public void TopUpExit(){
    mainshop.SetActive(true);
    topupmenu.SetActive(false);
    specialoffers.SetActive(false);
    bytecoins.SetActive(false);
    freemos.SetActive(false);
   }




}
