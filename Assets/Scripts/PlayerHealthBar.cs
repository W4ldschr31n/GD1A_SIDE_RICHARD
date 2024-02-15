using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoseHealth()
    {
        animator.SetTrigger("LoseHealth");
    }

    public void GainHealth()
    {
        animator.SetTrigger("GainHealth");
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }
}
