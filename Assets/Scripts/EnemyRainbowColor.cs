using UnityEngine;

public class EnemyRainbow : MonoBehaviour
{
    public float speed = 1f;

    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (rend == null)
        {
            Debug.LogError("Renderer が見つからない！");
        }
    }

    void Update()
    {
        float hue = Mathf.Repeat(Time.time * speed, 1f);
        Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);

        rend.material.color = rainbowColor;
    }
}
