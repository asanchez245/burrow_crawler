using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TESTZONE : MonoBehaviour
{
    public Transform spawnPoint;

    public void EmmitNeutralMana(int amount)
    {
        ManaPoolController.Instance.GenerateMana(OrbType.NeutralMana, spawnPoint.position, amount);
    }

    public void EmmitRedMana(int amount)
    {
        ManaPoolController.Instance.GenerateMana(OrbType.RedMana, spawnPoint.position, amount);
    }

    public void EmmitBlueMana(int amount)
    {
        ManaPoolController.Instance.GenerateMana(OrbType.BlueMana, spawnPoint.position, amount);
    }

    public void EmmitYellowMana(int amount)
    {
        ManaPoolController.Instance.GenerateMana(OrbType.YellowMana, spawnPoint.position, amount);
    }

    public void UseNeutralMana(int amount)
    {
        ManaPoolController.Instance.OnOrbUsed(OrbType.NeutralMana, amount);
    }

    public void UseRedMana(int amount)
    {
        ManaPoolController.Instance.OnOrbUsed(OrbType.RedMana, amount);
    }

    public void UseBlueMana(int amount)
    {
        ManaPoolController.Instance.OnOrbUsed(OrbType.BlueMana, amount);
    }

    public void UseYellowMana(int amount)
    {
        ManaPoolController.Instance.OnOrbUsed(OrbType.YellowMana, amount);
    }

    #region ____________________TEST ZONE CONTROLS____________________
    public GameObject testPanel;
    public Button panelActiver;
    int pannelButtonCount;

    private Coroutine resetClickCountCoroutine;

    public void OnPanelActiverClicked()
    {
        pannelButtonCount++;

        if (resetClickCountCoroutine != null)
        {
            StopCoroutine(resetClickCountCoroutine);
        }

        if (pannelButtonCount >= 10)
        {
            pannelButtonCount = 0;
            TogglePanelState();
        }
        else
        {
            resetClickCountCoroutine = StartCoroutine(ResetClickCountAfterDelay(2f));
        }
    }

    private void TogglePanelState()
    {
        if (testPanel == null) return;

        CanvasGroup canvasGroup = testPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = testPanel.AddComponent<CanvasGroup>();
        }

        bool isCurrentlyInteractable = canvasGroup.interactable && testPanel.activeSelf;

        if (isCurrentlyInteractable)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    private IEnumerator ResetClickCountAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pannelButtonCount = 0;
    }

    public void TogglePanel()
    {
        testPanel.gameObject.SetActive(!testPanel.gameObject.activeSelf);
    }
    #endregion
}