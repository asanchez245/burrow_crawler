using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManaPoolVisuals : MonoBehaviour
{
    [Header("UI Containers")]
    public Transform manaHorizontalContainer;
    public Transform orbsTarget;
    public GameObject orbPrefab;

    [Header("Orb Mappings")]
    public List<OrbParticleSystem> orbParticleSystems = new();

    [Header("Orbs Parameters")]
    public float baseSpeed = 5f;
    public float maxSpeed = 30f;
    public float minDistance = 0.5f;
    public float maxFloatDelay = 1.0f;

    [System.Serializable]
    public class OrbParticleSystem
    {
        public ManaType type;
        public ParticleSystem particleSystem;
        public Sprite orbSprite;

        public OrbParticleSystem(ParticleSystem ps, ManaType orbType, Sprite sprite)
        {
            particleSystem = ps;
            type = orbType;
            orbSprite = sprite;
        }
    }

    public void Initialize()
    {
        foreach (Transform child in manaHorizontalContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddMana(ManaType type)
    {
        Sprite sprite = GetSpriteForType(type);
        if (sprite == null) return;

        int currentCount = ManaPoolController.Instance.poolInfo.GetCurrentManaCount(type);

        if (currentCount < 4)
        {
            GameObject newInstance = Instantiate(orbPrefab, manaHorizontalContainer);
            newInstance.GetComponent<Image>().sprite = sprite;
            ToggleOrbText(newInstance, false, string.Empty);
        }
        else if (currentCount == 4)
        {
            RemoveAllOrbsOfSprite(sprite);

            GameObject newInstance = Instantiate(orbPrefab, manaHorizontalContainer);
            newInstance.GetComponent<Image>().sprite = sprite;
            ToggleOrbText(newInstance, true, currentCount.ToString());
        }
        else
        {
            GameObject groupedOrb = FindOrbBySprite(sprite);

            if (groupedOrb != null)
            {
                ToggleOrbText(groupedOrb, true, currentCount.ToString());
            }
            else
            {
                GameObject newInstance = Instantiate(orbPrefab, manaHorizontalContainer);
                newInstance.GetComponent<Image>().sprite = sprite;
                ToggleOrbText(newInstance, true, currentCount.ToString());
            }
        }

        SortOrbsInContainer();
    }

    public void RemoveMana(ManaType type, PlayType playType)
    {
        Sprite sprite = GetSpriteForType(type);
        if (sprite == null) return;

        int currentCount = ManaPoolController.Instance.poolInfo.GetCurrentManaCount(type);

        if (currentCount >= 4)
        {
            GameObject groupedOrb = FindOrbBySprite(sprite);
            if (groupedOrb != null)
            {
                ToggleOrbText(groupedOrb, true, currentCount.ToString());
            }
        }
        else if (currentCount == 3)
        {
            RemoveAllOrbsOfSprite(sprite);

            for (int i = 0; i < 3; i++)
            {
                GameObject newInstance = Instantiate(orbPrefab, manaHorizontalContainer);
                newInstance.GetComponent<Image>().sprite = sprite;
                ToggleOrbText(newInstance, false, string.Empty);
            }
        }
        else
        {
            GameObject lastOrb = FindOrbBySprite(sprite);
            if (lastOrb != null)
            {
                lastOrb.transform.SetParent(null);
                Destroy(lastOrb);
            }
        }
        int soundSelected = -1;
        switch (playType)
        {
            case PlayType.Attack:
                soundSelected = 0;
                break;
            case PlayType.Heal:
                soundSelected = 1;
                break;
            case PlayType.Defense:
                soundSelected = 2;
                break;
        }
        if (soundSelected != -1)
        {
            PlaySound(sounds[soundSelected]);
        }
        SortOrbsInContainer();
    }
    public AudioClip[] sounds;
    void PlaySound(AudioClip sound)
    {
        if (sound != null)
        {
            Vector3 spawnPosition = transform.position;
            GameObject objectSound = new GameObject("TemporaryAudioObject");
            objectSound.transform.position = spawnPosition;

            AudioSource audioSource = objectSound.AddComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.volume = 0.2f;
            audioSource.Play();

            Destroy(objectSound, sound.length);
        }
    }
    private void RemoveAllOrbsOfSprite(Sprite sprite)
    {
        for (int i = manaHorizontalContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = manaHorizontalContainer.GetChild(i);
            Image image = child.GetComponent<Image>();

            if (image != null && image.sprite == sprite)
            {
                child.SetParent(null);
                Destroy(child.gameObject);
            }
        }
    }

    private void ToggleOrbText(GameObject orbInstance, bool active, string textValue)
    {
        TextMeshProUGUI tmpText = orbInstance.GetComponentInChildren<TextMeshProUGUI>(true);
        if (tmpText != null)
        {
            tmpText.text = "x " + textValue;
            tmpText.gameObject.SetActive(active);
            return;
        }
    }

    private GameObject FindOrbBySprite(Sprite sprite)
    {
        foreach (Transform child in manaHorizontalContainer)
        {
            if (child.parent != manaHorizontalContainer) continue;

            Image image = child.GetComponent<Image>();
            if (image != null && image.sprite == sprite)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public void SortOrbsInContainer()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in manaHorizontalContainer)
        {
            if (child.parent == manaHorizontalContainer)
            {
                children.Add(child);
            }
        }
        children.Sort((a, b) => GetOrbValue(b).CompareTo(GetOrbValue(a)));
        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }

    private int GetOrbValue(Transform orbTransform)
    {
        Image img = orbTransform.GetComponent<Image>();
        if (img != null && img.sprite != null)
        {
            ManaType? type = GetTypeForSprite(img.sprite);
            if (type.HasValue)
            {
                int count = ManaPoolController.Instance.poolInfo.GetCurrentManaCount(type.Value);
                if (count >= 4)
                {
                    return count;
                }
            }
        }
        return 1;
    }

    private ManaType? GetTypeForSprite(Sprite sprite)
    {
        OrbParticleSystem mapping = orbParticleSystems.Find(orb => orb.orbSprite == sprite);
        return mapping != null ? mapping.type : null;
    }

    private Sprite GetSpriteForType(ManaType type)
    {
        OrbParticleSystem mapping = orbParticleSystems.Find(orb => orb.type == type);
        return mapping != null ? mapping.orbSprite : null;
    }

    public ParticleSystem GetParticleSystemForType(ManaType type)
    {
        OrbParticleSystem mapping = orbParticleSystems.Find(orb => orb.type == type);
        return mapping != null ? mapping.particleSystem : null;
    }
}