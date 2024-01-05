using UnityEngine;

public class PulsatingQuestionMark : MonoBehaviour
{
    public float glowIntensityMin = 0.5f; // Minimum glow intensity
    public float glowIntensityMax = 1.5f; // Maximum glow intensity
    public float sizeMultiplierMin = 0.9f; // Minimum size
    public float sizeMultiplierMax = 1.1f; // Maximum size
    public float pulsateSpeed = 3f; // Speed of the pulsation

    private Light questionMarkLight; // The light component for glow
    private Vector3 originalScale; // Original scale of the question mark

    void Start()
    {
        // Assign the light component. Make sure your question mark has a Light component attached for glow.
        questionMarkLight = GetComponent<Light>();
        originalScale = transform.localScale; // Store the original scale

        if (questionMarkLight == null)
        {
            Debug.LogWarning("Light component not found. Please attach a light component for the glow effect.");
        }
    }

    void Update()
    {
        // Calculate the sine value for the current time
        float sineWave = (Mathf.Sin(Time.time * pulsateSpeed) + 1) / 2; // Normalized to range 0 to 1

        // Update the light intensity for the glow effect
        if (questionMarkLight != null)
        {
            questionMarkLight.intensity = Mathf.Lerp(glowIntensityMin, glowIntensityMax, sineWave);
        }

        // Update the scale for the size pulsation effect
        float sizeMultiplier = Mathf.Lerp(sizeMultiplierMin, sizeMultiplierMax, sineWave);
        transform.localScale = originalScale * sizeMultiplier;
    }
}
