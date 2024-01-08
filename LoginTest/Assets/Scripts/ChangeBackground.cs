using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBackground : MonoBehaviour
{
    public Image canvasImage;
    public Sprite newImage;

    // Attach this method to the button's OnClick event in the Inspector
    public void ChangeImageOnClick()
    {
        if (canvasImage != null && newImage != null)
        {
            // Change the image of the Canvas
            canvasImage.sprite = newImage;
        }
        else
        {
            Debug.LogWarning("Image reference or new sprite not set. Please assign the Image and new sprite in the inspector.");
        }
    }
}
