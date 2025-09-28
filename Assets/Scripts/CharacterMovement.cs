using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private PlayerData data;

    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float crouchSpeed = 1f;

    [Header("Deteccion de suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float radioSphereCheck = 0.1f;
    private bool isGrounded;

    [SerializeField]
    private LayerMask npcMask;

    //atributos de logica
    private float startVelocity;
    private float moveH, moveV;
    private Vector3 playerMovement;
    private Quaternion rotationMovement;
    private Vector3 normalizedDirection;
    private Rigidbody rb;
    private Animator anim;
    private bool isCrouched = false;
    

    [Header("Control de la camara")]
    [SerializeField] Transform playerCamera;

    private bool isInDialogue;

    public bool IsInDialogue { get { return isInDialogue; } set {  isInDialogue = value; } } 

   
    void Start()
    {
        data = GetComponent<PlayerData>(); 
        startVelocity = data.MovementSpeed;
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    { 
        if (!isInDialogue)
        {
            moveH = Input.GetAxisRaw("Horizontal");
            moveV = Input.GetAxisRaw("Vertical");
            playerMovement = moveV * playerCamera.forward + moveH * playerCamera.right;
        }
        else
        {
            playerMovement = Vector3.zero;
        }

        if (playerMovement.magnitude > 0.1f)
        {
            anim.SetBool("playerMovement", true);
        }
        else
        {
            anim.SetBool("playerMovement", false);
        }

        //Logica de salto con un checkSphere.
        isGrounded = Physics.CheckSphere(groundCheck.position, radioSphereCheck, groundLayer);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetTrigger("jump");
            rb.AddForce(new Vector3(0, data.JumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
        }


        //movimiento sigilo + sprint, prioridades.
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouched = !isCrouched;
            anim.SetBool("crouched", isCrouched);
        }
        if (!isCrouched && Input.GetKey(KeyCode.LeftShift) && playerMovement.magnitude > 0.1f)
        {
            anim.SetBool("sprint", true);
            data.MovementSpeed = data.SprintSpeed;
        }
        else
        {
            anim.SetBool("sprint", false);
            data.MovementSpeed = isCrouched ? crouchSpeed : startVelocity;
        }

    }

    private void FixedUpdate()
    {
        //movimiento con fisicas, normalizado e interpolacion para movimiento suave
        if (playerMovement.magnitude > 0.1f)
        {
            normalizedDirection = playerMovement.normalized;
            normalizedDirection.y = 0;
            rb.MovePosition(rb.position + normalizedDirection * data.MovementSpeed * Time.fixedDeltaTime);
            rotationMovement = Quaternion.LookRotation(normalizedDirection);           
            Quaternion smoothRotation = Quaternion.RotateTowards(rb.rotation, rotationMovement, rotationSpeed * Time.fixedDeltaTime);  
            rb.MoveRotation(smoothRotation);
        }
    }
}
