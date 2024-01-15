using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeScene2Image : MonoBehaviour
{
    public ImageData imageData;

    public void ChangeImageOnClick()
    {
        Debug.Log("Button Clicked - Changing Scene");
        
        ImageDataHolder imageDataHolder = GameObject.FindObjectOfType<ImageDataHolder>();

        if (imageDataHolder != null)
        {
            imageDataHolder.imageData = imageData;

            // Load the scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ITEMSHOP");

            // You can check the progress of the scene load
            StartCoroutine(WaitForSceneLoad(asyncLoad));
        }
        else
        {
            Debug.LogError("ImageDataHolder not found in the current scene.");
        }
    }

    IEnumerator WaitForSceneLoad(AsyncOperation asyncLoad)
    {
        while (!asyncLoad.isDone)
        {
            // Output the progress percentage to the console
            Debug.Log("Scene loading progress: " + (asyncLoad.progress * 100) + "%");
            yield return null; // Wait until the next frame
        }
    }
}
