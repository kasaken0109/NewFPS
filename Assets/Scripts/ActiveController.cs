using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActiveController : MonoBehaviour
{
    [SerializeField] float m_activeTime = 1;
    // Start is called before the first frame update
    private void OnEnable()
    {
        //StartCoroutine(nameof(Active));
        AActive();
        
    }

    public void AActive()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(transform.position.y + 0.5f, 0.2f))
                .AppendInterval(1f)
                .Append(transform.DOMoveY(transform.position.y - 2f, 3f))
                .AppendCallback(() =>
                {
                    Destroy(this.gameObject);
                });

        sequence.Play();

    }

    IEnumerator Active()
    {
        float timer = 0;
        while (timer <= m_activeTime)
        {
            this.transform.DOMoveY(transform.position.y - 0.02f,0.1f);
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }
        Destroy(this.gameObject);
    }
}
