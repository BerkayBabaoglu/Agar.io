using System.Collections;
using UnityEngine;

public class StartGame : MonoBehaviour
{

    public GameObject sceneGameplay;

    public void Starting()
    {
        StartCoroutine(Wait());

        sceneGameplay.SetActive(true);

        //karakter oldugunde menuyu tekrardan aktif ederiz.

    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        this.gameObject.SetActive(false);
    }
}
