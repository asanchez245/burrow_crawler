using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManaPoolVisuals : MonoBehaviour
{
    [Header("UI Containers / Targets")]
    public Transform neutralManaTarget;
    public Transform yellowManaTarget;
    public Transform blueManaTarget;
    public Transform redManaTarget;

    [Header("Holders")]
    public Transform neutralManaHolder;
    public Transform yellowManaHolder;
    public Transform blueManaHolder;
    public Transform redManaHolder;

    [Header("Mana Lists")]
    private List<GameObject> neutralManas = new();
    private List<GameObject> yellowManas = new();
    private List<GameObject> blueManas = new();
    private List<GameObject> redManas = new();

    [Header("Mana Prefabs")]
    public List<ManaParticleSystemMapping> manaParticleSystems = new List<ManaParticleSystemMapping>();

    [Header("Orbs Parameters")]
    public float baseSpeed = 5f;
    public float maxSpeed = 30f;
    public float minDistance = 0.5f;
    public float maxFloatDelay = 1.0f;

    public void Initialize()
    {
        GetImageObjectsFromHolder(neutralManaHolder, neutralManas);
        GetImageObjectsFromHolder(yellowManaHolder, yellowManas);
        GetImageObjectsFromHolder(blueManaHolder, blueManas);
        GetImageObjectsFromHolder(redManaHolder, redManas);
    }

    public void UpdateManaVisuals(OrbType type, int currentManaAmount)
    {
        List<GameObject> targetList = GetListForType(type);
        if (targetList == null || targetList.Count == 0) return;

        int totalSlots = targetList.Count;

        for (int i = 0; i < totalSlots; i++)
        {
            GameObject manaObject = targetList[i];
            if (manaObject == null) continue;

            bool shouldBeActive = currentManaAmount > i;

            if (manaObject.activeSelf != shouldBeActive)
            {
                manaObject.SetActive(shouldBeActive);
            }
        }

        GameObject bottomSlot = targetList[0];
        if (bottomSlot != null)
        {
            if (currentManaAmount > totalSlots)
            {
                int extraMana = currentManaAmount - totalSlots;
                SetTextActiveAndValue(bottomSlot, true, $"x{extraMana}");
            }
            else
            {
                SetTextActiveAndValue(bottomSlot, false, string.Empty);
            }
        }
    }

    private void SetTextActiveAndValue(GameObject slotObject, bool active, string textValue)
    {
        TextMeshProUGUI tmpText = slotObject.GetComponentInChildren<TextMeshProUGUI>(true);
        if (tmpText != null)
        {
            tmpText.text = textValue;
            tmpText.gameObject.SetActive(active);
            return;
        }

        Text legacyText = slotObject.GetComponentInChildren<Text>(true);
        if (legacyText != null)
        {
            legacyText.text = textValue;
            legacyText.gameObject.SetActive(active);
        }
    }

    public void UpdateAllManaVisuals(int neutral, int yellow, int blue, int red)
    {
        UpdateManaVisuals(OrbType.NeutralMana, neutral);
        UpdateManaVisuals(OrbType.YellowMana, yellow);
        UpdateManaVisuals(OrbType.BlueMana, blue);
        UpdateManaVisuals(OrbType.RedMana, red);
    }

    public void GetImageObjectsFromHolder(Transform holder, List<GameObject> imageObjectsList)
    {
        if (imageObjectsList == null) return;
        imageObjectsList.Clear();

        if (holder == null) return;

        Image[] images = holder.GetComponentsInChildren<Image>(true);

        foreach (Image img in images)
        {
            imageObjectsList.Add(img.gameObject);
        }

        imageObjectsList.Reverse();
    }

    private List<GameObject> GetListForType(OrbType type)
    {
        switch (type)
        {
            case OrbType.NeutralMana: return neutralManas;
            case OrbType.YellowMana: return yellowManas;
            case OrbType.BlueMana: return blueManas;
            case OrbType.RedMana: return redManas;
            default: return null;
        }
    }
}