using UnityEngine;

public class SimpleDoor : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false; // ドアが開いているかどうかの状態

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // プレイヤーがEキーを押したときに呼ばれる
    public void Interact(PlayerKeyInventory inventory)
    {
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen; // 状態を反転させる

        if (animator != null)
        {
            // AnimatorのBoolパラメーター「isOpen」を切り替える
            animator.SetBool("isOpen", isOpen);
        }
    }
}