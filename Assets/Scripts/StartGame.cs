using System.Collections;
using UnityEngine;

public class StartGame : MonoBehaviour
{

    public GameObject sceneGameplay;
    public GameObject kostumler;

    public void Starting()
    {

        StartCoroutine(Wait());
        //sceneGameplay.SetActive(true);

        GameManager.Instance.GameLoad();

        

        if (GameManager.Instance.Menu != null)
        {
            GameManager.Instance.Menu.SetActive(false);
            Time.timeScale = 1f;
        }
            
        else
            this.gameObject.SetActive(false);
        //karakter oldugunde menuyu tekrardan aktif ederiz.

    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        this.gameObject.SetActive(false);
    }

    public void CustomButtonActive()
    {
        kostumler.SetActive(true);
    }

    public void CustomButtonDeactive()
    {
        kostumler.SetActive(false);
    }
}
