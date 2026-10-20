using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class EnemyVisuals : MonoBehaviour
{
    public GameObject enemyVisuals;

    public Image[] enemySpritesList;
    public Image enemySprite;
    public Animator enemyAnimator;
    public GameObject targetedIcon;
    public GameObject attackIntentionIcon;
    public TextMeshProUGUI attackIntentionValue;
    public GameObject healIntentionIcon;
    public TextMeshProUGUI healIntentionValue;
    public GameObject defenseIntentionIcon;
    public TextMeshProUGUI defenseIntentionValue;
    internal void InitializeEnemyVisuals(EnemyData enemyData)
    {
        //SetAnimatorController(enemyData.animatorController);
        int spriteChosed = enemyData.enemyName == "Lobo" ? 0 : 1;
        if (spriteChosed == 0)
        {
            enemySpritesList[1].gameObject.SetActive(false);
        }
        else
        {
            enemySpritesList[0].gameObject.SetActive(false);
        }
        enemySprite = enemySpritesList[spriteChosed];
        enemyAnimator = enemySprite.GetComponent<Animator>();
        targetedIcon.SetActive(false);
        SetIntentionsValues(enemyData);
    }

    void SetIntentionsValues(EnemyData enemyData)
    {
        attackIntentionValue.text = enemyData.damageAmount.ToString();
        healIntentionValue.text = enemyData.healingAmount.ToString();
        defenseIntentionValue.text = enemyData.shieldingAmount.ToString();
    }

    public void SetTargetedVisuals(bool isTargeted)
    {
        targetedIcon.SetActive(isTargeted);
        if (isTargeted)
        {
            enemySprite.transform.DOScale(1.15f, .2f);
        }
        else
        {
            enemySprite.transform.DOScale(1f, .2f);
        }
    }
    public void SetIntentionIcon(PlayType playType)
    {
        switch (playType)
        {
            case PlayType.Attack:
                attackIntentionIcon.SetActive(true);
                break;
            case PlayType.Heal:
                healIntentionIcon.SetActive(true);
                break;
            case PlayType.Defense:
                defenseIntentionIcon.SetActive(true);
                break;
        }
    }
    public void CleanAllIntentionIcons()
    {
        attackIntentionIcon.SetActive(false);
        attackIntentionValue.alpha = 0;
        healIntentionIcon.SetActive(false);
        healIntentionValue.alpha = 0;
        defenseIntentionIcon.SetActive(false);
        defenseIntentionValue.alpha = 0;
    }
    public IEnumerator TriggerAnim(PlayType playType)
    {
        string animation = playType.ToString();

        if (!string.IsNullOrEmpty(animation))
        {
            enemyAnimator.SetTrigger(animation);
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
        yield return null;
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
    public IEnumerator RevealIntention(PlayType playType)
    {
        GameObject actionSelected = null;
        TextMeshProUGUI actionValue = null;

        switch (playType)
        {
            case PlayType.Attack:
                actionSelected = attackIntentionIcon;
                actionValue = attackIntentionValue;
                break;
            case PlayType.Defense:
                actionSelected = defenseIntentionIcon;
                actionValue = defenseIntentionValue;
                break;
            case PlayType.Heal:
                actionSelected = healIntentionIcon;
                actionValue = healIntentionValue;
                break;
            case PlayType.Utility:
                yield break;
        }

        if (actionSelected != null && actionValue != null)
        {
            Sequence revealIntentionTween = DOTween.Sequence();
            revealIntentionTween.Append(actionSelected.transform.DOScaleX(0, .25f));
            revealIntentionTween.Append(actionSelected.transform.DOScaleX(1, .25f));
            revealIntentionTween.Join(actionValue.DOFade(1, .25f).From(0));
            revealIntentionTween.AppendInterval(.5f);

            yield return revealIntentionTween.WaitForCompletion();
        }
        CleanAllIntentionIcons();
    }
    public void SetAnimatorController(Animator animatorController)
    {
        enemyAnimator = animatorController;
    }
    public void EnemyDieVisuals()
    {
        enemySprite.DOKill();
        enemySprite.DOFade(0, 0.5f).OnComplete(() => enemyVisuals.SetActive(false));
    }

    internal void PointerEnterEnemy(bool enter)
    {
        if (enter)
        {
            this.transform.DOScale(1.05f, .2f);
        }
        else
        {
            this.transform.DOScale(1f, .2f);
        }
    }
}
