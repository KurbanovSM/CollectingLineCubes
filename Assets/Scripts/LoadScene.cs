using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void Load()
    {
        StartCoroutine(LoadCorine());
    }

    IEnumerator LoadCorine()
    {
        ClickSound.Click.Invoke();
        PanelsController.Instance.DisabledPanel(1);
        PanelsController.Instance.DisabledPanel(3);
        PanelsController.Instance.DisabledPanel(4);

        yield return new WaitForSeconds(.5f);

        SceneManager.LoadScene(0);
    }
}
