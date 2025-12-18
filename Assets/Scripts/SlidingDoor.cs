using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    private Animator _animator;
    private bool _isOpen = false;

    void Start()
    {
        // 自分のオブジェクト、または子にあるAnimatorを取得
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    // FirstPersonControllerのTryInteractから呼ばれる関数
    public void Interact(PlayerKeyInventory inventory)
    {
        // 状態を入れ替える（trueならfalse、falseならtrue）
        _isOpen = !_isOpen;

        if (_animator != null)
        {
            // アニメーターの「isOpen」というBool値を更新
            _animator.SetBool("isOpen", _isOpen);
            Debug.Log(_isOpen ? "ドアを開けました" : "ドアを閉めました");
        }
    }
}