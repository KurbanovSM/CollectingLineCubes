using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Panel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void EnabledPanel()
    {
        StartCoroutine(Show(false));
    }

    public void DisabledPanel()
    {
        StartCoroutine(Show(true));
    }

    private IEnumerator Show(bool isEnagled)
    {
        float speed;
        bool isShowComplete = false;

        if (isEnagled)
        {
            canvasGroup.blocksRaycasts = false;
            speed = -3;
        }
        else
        {
            canvasGroup.blocksRaycasts = true;
            speed = 3;
        }

        while (!isShowComplete)
        {
            canvasGroup.alpha += Time.deltaTime * speed;
            yield return null;

            if (isEnagled && canvasGroup.alpha == 0 ||
                !isEnagled && canvasGroup.alpha == 1) isShowComplete = true;
        }
    }
}
