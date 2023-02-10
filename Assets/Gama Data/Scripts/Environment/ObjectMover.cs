using UnityEngine;
using UnityEngine.Events;
using BCTSTool.Editor;

public class ObjectMover : Interacteble
{
    [SerializeField] private Transform m_MovePosition;
    [SerializeField] private float m_MoveSpeed = 5f;

    public UnityEvent OnIteract;

    private Vector3 m_OriginPosition;

    private const float c_SphereRadus = 0.2f;
    private const float c_ArrowDistence = 0.8f;

    private void Awake()
    {
        m_OriginPosition = transform.position;
    }

    private void Update()
    {
        Move();
    }

    public override void Interact()
    {
        if (p_IsInteracteble == true)
        {
            base.Interact();
            p_IsInteracteble = false;
            OnIteract?.Invoke();
        }
    }

    private void Move()
    {
        if(p_IteractCount > 0)
        {
            transform.position = Vector3.Lerp(transform.position, m_MovePosition.position, m_MoveSpeed * Time.deltaTime);
        }
    }

    public override void Interact(Slot slot) { }

    private void OnDrawGizmosSelected()
    {
        if (m_MovePosition == null) return;

        if(Application.IsPlaying(this) == false)
        {
            m_OriginPosition = transform.position;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_OriginPosition, c_SphereRadus);

        Gizmos.DrawLine(m_OriginPosition, m_MovePosition.position);

        float distence = Vector3.Distance(m_OriginPosition, m_MovePosition.position);
        Vector3 direction = (m_MovePosition.position - m_OriginPosition).normalized;

        for (int i = 0; i < (int)(distence / c_ArrowDistence); i++)
            CustemGizmos.DrowArrow(m_OriginPosition + direction * (c_ArrowDistence * i), direction, 0.5f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(m_MovePosition.position, c_SphereRadus);

    }
}
