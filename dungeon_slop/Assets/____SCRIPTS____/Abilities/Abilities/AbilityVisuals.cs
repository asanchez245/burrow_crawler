using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.UI;

public class AbilityVisuals : MonoBehaviour
{
    AbilityController abilityController;

    [Header("UI Texts")]
    TextMeshProUGUI abilityNameText;
    TextMeshProUGUI abilityDescriptionText;
    
    TextMeshProUGUI costText;

    [Header("UI References")]
    [SerializeField] Image abilityArtwork;

    //[Header("Particles")]
    //public ParticleSystem cardParticles;
    //public CanvasGroup cardDeliriumFlame;
    //public CanvasGroup chipDeliriumFlame;

    public void Initialize(AbilityData data)
    {
        abilityController = GetComponent<AbilityController>();

        //abilityNameText = CombatController.Instance.GetInfoName();
        //abilityDescriptionText = CombatController.Instance.GetInfoDesc();

        abilityArtwork.sprite = data.artwork;

        SetArtworkColor();

        //if (data.manaCost.Count() > 0 || !data.enemyAbility) PrintCost();
    }



    public void PrintCost()
    {
        string totalCostText = "";

        for (int i = 0; i < abilityController.GetCost(ManaType.NeutralMana); i++)
        {
            //totalCostText += "<style=Ammo></style>";
        }

        for (int i = 0; i < abilityController.GetCost(ManaType.YellowMana); i++)
        {
            //totalCostText += "<style=Rituals></style>";
        }

        for (int i = 0; i < abilityController.GetCost(ManaType.BlueMana); i++)
        {
            //totalCostText += "<style=Perception></style>";
        }

        for (int i = 0; i < abilityController.GetCost(ManaType.RedMana); i++)
        {
            //totalCostText += "<style=Perception></style>";
        }
        //////////////////////////////////cambiar a bolitas mana en la descriocipn (nuevo style)
        costText.text = totalCostText;
        costText.transform.parent.gameObject.SetActive(costText.text != "");
    }

    public void PrintAbilityInfo()
    {
        if(abilityNameText == null) abilityNameText = CombatController.Instance.GetInfoName();
        if(abilityDescriptionText == null) abilityDescriptionText = CombatController.Instance.GetInfoDesc();
        abilityNameText.text = abilityController.GetAbilityName();
        abilityDescriptionText.text = abilityController.GetAbilityDesc();
    }

    //******************************** VISUALS EVENTS ****************************************
    public void SetArtworkColor()
    {
        //aquie se podria aprovechar para comprobar si tienes mana
        //para una habilidad y si no tienes que la ponga gris

        //if (!abilityArtwork) return;
        //Color newColor = Color.white;
        //if (abilityController.GetCurrentState() == CardState.discarding)
        //{
        //    newColor = Color.gray;
        //}
        //abilityArtwork.color = newColor;
    }
    public void IncreaseImageSize(GameObject imagen)
    {
        Sequence show = DOTween.Sequence();
        show.Append(imagen.transform.DOScale(new Vector3(1.15f, 1.15f, 1f), .2f));
    }
    public void DecreaseImageSize(GameObject imagen)
    {
        Sequence show = DOTween.Sequence();
        show.Append(imagen.transform.DOScale(new Vector3(1, 1, 1), .2f));
    }
}
