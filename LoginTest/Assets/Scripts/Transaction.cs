using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Transaction : MonoBehaviour
{
    public TextMeshProUGUI freemoGems, byteCoin;
    public GameObject BGsold1,BGsold2,BGsold3,BGsold4,BGsold5,BGsold6,HATsold1,HATsold2,HATsold3,HATsold4,HATsold5,HATsold6;
    
    public int freemoValue = 0;
    public int byteValue = 0;


    public void OnFreemo1ButtonPress()
    {
        freemoValue += 20;
        UpdateUIText();
    }
     public void OnFreemo2ButtonPress()
    {
        freemoValue += 50;
        UpdateUIText();
    }
     public void OnFreemo3ButtonPress()
    {
        freemoValue += 100;
        UpdateUIText();
    }
     public void OnFreemo4ButtonPress()
    {
        freemoValue += 200;
        UpdateUIText();
    }
//================================================================

     public void OnByte1ButtonPress()
    {   
        byteValue += 1000;
        UpdateUIText();
    }
     public void OnByte2ButtonPress()
    {
        byteValue += 2000;
        UpdateUIText();
    } public void OnByte3ButtonPress()
    {
        byteValue += 5000;
        UpdateUIText();
    } public void OnByte4ButtonPress()
    {
        byteValue += 10000;
        UpdateUIText();
    }
// ITEM SHOP =======================================================================================

    public void PurchaseBG1Button()
    {   
        bool purchased1=false;
        if (byteValue>=5000){
       
        if (!purchased1){
        byteValue -= 5000;
        UpdateUIText();
        BGsold1.SetActive(true);
        purchased1=true;}
        }
    
    }
     public void PurchaseBG2Button()
    {   if (byteValue>=5000){
        bool purchased2=false;
        if (!purchased2){
        byteValue -= 5000;
        UpdateUIText();
        BGsold2.SetActive(true);
        purchased2=true;}
        }
    }
     public void PurchaseBG3Button()
    {   if (byteValue>=5000){
         bool purchased3=false;
        
        if (!purchased3){
            byteValue -= 5000;
        UpdateUIText();
        BGsold3.SetActive(true);
        purchased3=true;}
        }
    }
     public void PurchaseBG4Button()
    {   if (byteValue>=5000){
         bool purchased4=false;
       
        if (!purchased4){
        byteValue -= 5000;
        UpdateUIText();
        BGsold4.SetActive(true);
        purchased4=true;}
        }
    } 
    public void PurchaseBG5Button()
    {  
         if (byteValue>=5000){
        bool purchased5=false;
          if (!purchased5){
        byteValue -= 5000;
        UpdateUIText();
        BGsold5.SetActive(true);
        purchased5=true;}
        }
    } 
    
    
    public void PurchaseBG6Button()
    {   if (byteValue>=5000){
        bool purchased6=false;
       if (!purchased6){
        byteValue -= 5000;
        UpdateUIText();
        BGsold6.SetActive(true);
        purchased6=true;}
        }
    }
//HATSU=============================================================
    public void PurchaseHAT1Button()
    {   if (byteValue>=500){
        bool purchased7=false;
      
        if (!purchased7){
            byteValue -= 500;
            UpdateUIText();
            HATsold1.SetActive(true);
        purchased7=true;}
        }
    }
    public void PurchaseHAT2Button()
    {   if (byteValue>=500){
        bool purchased8=false;
              if (!purchased8){
        byteValue -= 500;
      UpdateUIText();
           HATsold2.SetActive(true);
        purchased8=true;}
        }
    }
    public void PurchaseHAT3Button()
    {   if (byteValue>=200){
        bool purchased9=false;
         if (!purchased9){
        byteValue -= 200;
       UpdateUIText();
           HATsold3.SetActive(true);
        purchased9=true;}
        }
    }
    public void PurchaseHAT4Button()
    {   if (freemoValue>=200){
        bool purchased10=false;
         if (!purchased10){
        freemoValue -= 200;
        UpdateUIText();
            HATsold4.SetActive(true);
        purchased10=true;}
        }
    }
    public void PurchaseHAT5Button()
    {   if (freemoValue>=200){
        bool purchased11=false;
        if (!purchased11){
        freemoValue -= 200;
        UpdateUIText();
        HATsold5.SetActive(true);
        purchased11=true;}
        }
    }
    public void PurchaseHAT6Button()
    {   if (freemoValue>=200){
        bool purchased12=false;
         if (!purchased12){
        freemoValue -= 200;
        UpdateUIText();
            HATsold6.SetActive(true);
        purchased12=true;}
        }
    }

    void UpdateUIText()
    {
       
        freemoGems.text = freemoValue.ToString();
        byteCoin.text = byteValue.ToString();
    }



    public static Transaction transaction;
    
}
