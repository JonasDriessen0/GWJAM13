using UnityEngine;

public class OscilloscopeWave : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int points = 100;

    public float minFrequency = 1f;
    public float maxFrequency = 10f;
    public float minAmplitude = 0.5f;
    public float maxAmplitude = 5f;
    public float speed = 2f;

    public float frequency = 2f;
    public float amplitude = 1f;
    public float timeOffset = 0f; // New: Replaces phase

    void Start()
    {
        lineRenderer.positionCount = points;
    }

    void Update()
    {
        DrawSineWave();
    }

    void DrawSineWave()
    {
        float time = Time.time * speed;

        for (int i = 0; i < points; i++)
        {
            float x = (float)i / (points - 1) * 2f - 1f; // Normalized X range
            float y = Mathf.Sin(2 * Mathf.PI * frequency * (x + time + timeOffset)) * amplitude; // Uses timeOffset

            lineRenderer.SetPosition(i, new Vector3(x * 5f, y, 0));
        }
    }
}