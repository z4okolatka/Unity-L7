using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] public Score topScore;
    private Animator animator;
    private TMP_Text text;
    public int score { get; private set; } = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        text = GetComponent<TMP_Text>();
    }

    public void setScore(int value)
    {
        score = value;
        text.text = score.ToString();
    }

    public void addScore(int value)
    {
        score += value;
        text.text = score.ToString();
    }

    public void setTrigger(string name)
    {
        animator.SetTrigger(name);
    }
    public void resetTrigger(string name)
    {
        animator.ResetTrigger(name);
    }
}
