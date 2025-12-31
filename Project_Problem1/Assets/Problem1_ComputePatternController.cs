using UnityEngine;

public class Problem1_ComputePatternController : MonoBehaviour
{
    public ComputeShader computeShader;
    public Renderer targetRenderer;

    public int textureSize = 256;
    public float interval = 1f;

    RenderTexture renderTexture;
    int kernelIndex;
    float timer = 0f;
    int patternIndex = 0;

    void Start()
    {
        if (computeShader == null || targetRenderer == null)
        {
            Debug.LogError("computeShader / targetRenderer 지정 필요");
            enabled = false;
            return;
        }

        kernelIndex = computeShader.FindKernel("CSMain");

        renderTexture = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGB32);
        renderTexture.enableRandomWrite = true;
        renderTexture.wrapMode = TextureWrapMode.Clamp;
        renderTexture.filterMode = FilterMode.Point;
        renderTexture.Create();

        targetRenderer.material.mainTexture = renderTexture;

        DispatchPattern();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer -= interval;
            patternIndex = 1 - patternIndex;
            DispatchPattern();
        }
    }

    void DispatchPattern()
    {
        computeShader.SetInt("pattern", patternIndex);
        computeShader.SetTexture(kernelIndex, "Result", renderTexture);

        int groups = textureSize / 32;
        computeShader.Dispatch(kernelIndex, groups, groups, 1);
    }

    void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            renderTexture = null;
        }
    }
}
