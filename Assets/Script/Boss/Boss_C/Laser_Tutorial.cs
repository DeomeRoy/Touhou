using UnityEngine;
using DG.Tweening;

public class Laser_Tutorial : MonoBehaviour
{
    [SerializeField] private float DefDistanceRay = 100;
    public Transform laserFirePoint;
    public LineRenderer m_lineRenderer;
    public bool StartShoot, HitDetection, back;
    public float HitTimer;
    Transform m_transform;
    public RaycastHit2D _hit;
    private PlayerController hitPlayer;

    void Awake()
    {
        m_transform = GetComponent<Transform>();
        StartShoot = false;
        Back();
    }

    void Update()
    {
        HitTimer += Time.deltaTime;
        if (StartShoot)
        {
            m_lineRenderer.enabled = true;
            int ignoreMask = LayerMask.GetMask("Boss", "D_Ball", "E_Block", "B_Block");
            int raycastMask = ~ignoreMask;
            _hit = Physics2D.Raycast(laserFirePoint.position, transform.right, DefDistanceRay, raycastMask);
            if (_hit.collider != null)
            {
                Draw2DRay(laserFirePoint.position, _hit.point);

                hitPlayer = _hit.collider.GetComponent<PlayerController>();
            }
            else
            {
                Draw2DRay(laserFirePoint.position, laserFirePoint.position + (Vector3)(transform.right * DefDistanceRay));
                hitPlayer = null;
            }
        }
        if (!StartShoot)
            m_lineRenderer.enabled = false;
        if (hitPlayer != null && HitTimer > 2.5f)
        {
            hitPlayer.LoseLife();
            HitTimer = 0;
        }
        if (back)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            transform.DOMove(new Vector3(0, 32, 0), 1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                back = false;
                GetComponent<SpriteRenderer>().enabled = false;
            });
        }
    }
    void Draw2DRay(Vector2 starPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, starPos);
        m_lineRenderer.SetPosition(1, endPos);
    }
    public void Back()
    {
        back = true;
    }
}
    