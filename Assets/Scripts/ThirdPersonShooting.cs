using Unity.Cinemachine;
using Unity.Hierarchy;
using UnityEngine;

public class thirdPersonShooting : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineCamera vCam;
    [SerializeField] private float normalVision = 60f;
    [SerializeField] private float zoomVision = 40f;
    [SerializeField] private float zoomLerp = 10f;

    
    private bool isAiming = false;
    private float lastShootTime;
    private float shootCooldown = 0.5f;
    public float LastShootTime {get { return lastShootTime; } set{ lastShootTime = value; } }
    private Animator anim;
    public bool IsAiming => isAiming;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        // Apuntar con click derecho
        if (Input.GetMouseButtonDown(1)) isAiming = true;
        if (Input.GetMouseButtonUp(1)) isAiming = false;

        float targetFOV = isAiming ? zoomVision : normalVision;
        vCam.Lens.FieldOfView = Mathf.Lerp(vCam.Lens.FieldOfView, targetFOV, Time.deltaTime * zoomLerp);

        // Disparar con click izquierdo
        if (isAiming && Input.GetMouseButtonDown(0) && Time.time > lastShootTime + shootCooldown)
        {
            anim.SetTrigger("shoot");
            lastShootTime = Time.time;
        }
    }
}
