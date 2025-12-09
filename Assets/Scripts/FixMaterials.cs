using UnityEngine;

public class FixMaterials : MonoBehaviour
{
    [ContextMenu("Fix All Materials")]
    void FixAll()
    {
        // 新しい Unity の推奨 API
        MeshRenderer[] renderers = 
            Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);

        int fixedCount = 0;

        foreach (var r in renderers)
        {
            if (r.sharedMaterials.Length > 1)
            {
                // 一番上のマテリアルだけ残す
                Material mat = r.sharedMaterials[0];
                r.sharedMaterials = new Material[] { mat };
                fixedCount++;
            }
        }

        Debug.Log("✔ 修正完了！ " + fixedCount + " 個のメッシュを統一しました。");
    }
}
