using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetLevels : MonoBehaviour
{
    public static SetLevels instance;
    public int site;
    public bool endDownload;
    bool rightArrowEnable = false;
    [SerializeField] Button leftArrow;
    [SerializeField] Button rightArrow;
    [SerializeField] Text numberSite;
    [SerializeField] GameObject loadingPanel;
    LevelManager[] lines;
    Animator animator;
    int count;

    private void Awake()
    {
        if(instance == null) instance = this;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        site = 1;
        count = 0;
        lines = GetComponentsInChildren<LevelManager>();
        LoadIconLevels();
    }

    public void LeftArrow()
    {
        animator.SetBool("Switch", true);
        site--;
        leftArrow.interactable = false;
        rightArrow.interactable = false;
        numberSite.text = site.ToString();
        Invoke(nameof(SwitchLoadingPanel), 0.5f);
    }
    public void RightArrow()
    {
        animator.SetBool("Switch", true);
        site++;
        leftArrow.interactable = false;
        rightArrow.interactable = false;
        numberSite.text = site.ToString();
        Invoke(nameof(SwitchLoadingPanel), 0.5f);
    }

    void LoadIconLevels()
    {
        ResetIconLevels();
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].level = (i + 1) + ((site-1) * lines.Length);
            if (lines[i].level == 1)
            {
                lines[i].unlocked = true;
            }
            StartCoroutine(lines[i].DownloadPictures());
            if (i >= 1 && PlayerPrefs.GetInt("WinLevel" + lines[i - 1].level, 0) == 1)
            {
                lines[i].unlocked = true;
            }
        }
    }

    public void VerificationIconLevels(int level)
    {
        rightArrowEnable = false;
        count++;
        if (count == 20)
        {
            if(!animator.enabled) animator.enabled = true;

            foreach (LevelManager line in lines)
            {
                if (line.loaded)
                {
                    line.LoadIcons();
                    if (line.level == 20 * site && !line.soon) rightArrowEnable = true;
                }
            }
            count = 0;
            Invoke(nameof(EndAnim), 0.5f);
        }
    }

    void EndAnim()
    {
        if (site > 1) leftArrow.interactable = true;
        rightArrow.interactable = rightArrowEnable;
        animator.SetBool("Switch", false);
        loadingPanel.SetActive(false);
    }

    void SwitchLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    void ResetIconLevels()
    {
        foreach(LevelManager line in lines)
        {
            line.ResetIcons();
        }
    }
}
