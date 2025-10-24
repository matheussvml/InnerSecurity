using UnityEngine;
using UnityEngine.UI;

public class SanityBarUI : MonoBehaviour
{
    public Slider sanitySlider;
    public SanitySystem sanitySystem;

    private void Start()
    {
        if (sanitySlider != null)
            sanitySlider.maxValue = 100f;
    }

    private void Update()
    {
        if (sanitySystem != null && sanitySlider != null)
        {
            sanitySlider.value = sanitySystem.sanity;
        }
    }
}
