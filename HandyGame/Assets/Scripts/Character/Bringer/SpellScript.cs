using UnityEngine;

public class SpellScript : MonoBehaviour
{
    public EnemySpellScript spellScript;

    public Animator animator;

    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyDamage()
    {
        spellScript.ApplyDamageSpell();
    }

    public void SetActive(int inputValue)
    {
        gameObject.SetActive(inputValue != 0);
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }
}
