using UnityEngine;
using Zenject;

public class SlotSelector : MonoBehaviour
{
    public enum SelectorType 
    {
        Both,
        Clickerd,
        Scroll_Mouse
    }

    [Inject] public readonly InputHandler InputHandler;

    public Storage Storage { get; private set; }
    public SlotSelectorModel SlotSelectorModel { get; private set; }
    public SlotSelectorView SlotSelectorView { get; private set; }

    [SerializeField] private SelectorType m_SelectorType;

    public SelectorType GetSelectorType() => m_SelectorType;

    public void Init(Storage storage)
    {
        Storage = storage;

        SlotSelectorModel = new SlotSelectorModel(this);
        SlotSelectorView = new SlotSelectorView(this);
    }

    private void Update()
    {
        SlotSelectorView.MoveSelector();
        SlotSelectorModel.Update();
    }
}
