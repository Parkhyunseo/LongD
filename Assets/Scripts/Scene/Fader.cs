using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes
{
    public class Fader : MonoBehaviour
    {
        public Sprite sprite;
        public new Collider2D collider;
        public float duration = 0.5f;

        void Start()
        {
           FadeOut(() =>
           {
               gameObject.SetActive(true);
           });
        }
        
        public void FadeIn(Action onFaded)
        {
            StartCoroutine(CFadeIn(onFaded));
        }

        public void FadeOut(Action onFaded)
        {
            StartCoroutine(CFadeOut(onFaded));
        }

        IEnumerator CFadeIn(Action onFaded)
        {
            // Sprite alpha 0
            yield return new WaitForSecondsRealtime(duration);
            if (onFaded != null)
                onFaded();
        }

        IEnumerator CFadeOut(Action onFaded)
        {
            // Sprite alpha 1
            yield return new WaitForSecondsRealtime(duration);
            if (onFaded != null)
                onFaded();
        }
    }

}
