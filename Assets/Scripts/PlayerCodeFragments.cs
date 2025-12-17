using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCodeFragments : MonoBehaviour
{
    [Tooltip("パスコードの断片を格納する配列 (0, 1, 2)")]
    // 収集していない場合は null または ""
    private string[] codeFragments = new string[3]; 

    [Tooltip("脱出に必要な断片の数")]
    public int requiredFragments = 3;
    
    private int fragmentsCollected = 0; // 収集した断片の数

    /// <summary>
    /// パスコードの断片を取得したときに呼ばれるメソッド
    /// </summary>
    /// <param name="index">断片のインデックス (0, 1, 2)</param>
    /// <param name="value">断片の値 (例: "4", "8", "73")</param>
    public void AddFragment(int index, string value)
    {
        // 既にその断片を収集済みでないかチェック
        if (codeFragments[index] == null || codeFragments[index] == "")
        {
            codeFragments[index] = value;
            fragmentsCollected++;
            Debug.Log($"パスコード断片 {index + 1} を取得しました: {value}");
        }
        
        if (fragmentsCollected == requiredFragments)
        {
            Debug.Log("全ての断片を揃えました！脱出ドアに向かってください。");
        }
        // ここでUIを更新する処理を追加できます
    }

    /// <summary>
    /// 現在収集している断片の数を取得
    /// </summary>
    public int GetFragmentsCount()
    {
        return fragmentsCollected;
    }
    
    /// <summary>
    /// 収集した断片の配列を取得
    /// </summary>
    public string[] GetFragments()
    {
        return codeFragments;
    }

    // ゲーム終了処理 (テスト用)
    public void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}