using System.Collections;
using FX;
using TMPro;
using UnityEngine;

public class TextFx : Fx
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _speed = 1f;
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    public override void Execute(object arg = null)
    {
        string textString = arg as string;
        _text.text = textString;
        StartCoroutine(TextDriftAndFadeCoroutine());
    }
    
    private IEnumerator TextDriftAndFadeCoroutine()
    {
        float timeElapsed = 0f;
        Vector2 moveDirection = Quaternion.Euler(0, 0, Random.Range(0f, 360f)) * Vector2.one;
        transform.Translate(moveDirection * 0.5f);
        while (timeElapsed < _duration)
        {
            timeElapsed += Time.deltaTime;
            float t = 1 - timeElapsed / _duration;
            
            var color = _text.color;
            color.a = t;
            _text.color = color;

            transform.Translate(moveDirection * (_speed * Time.deltaTime));

            yield return null;
        }
        Release();
    }

    protected override void CleanUp()
    {
        StopAllCoroutines();
    }
}
