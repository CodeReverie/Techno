using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Itemshop : MonoBehaviour
{
   public GameObject background, hats, visors, skins, pets;

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




}
