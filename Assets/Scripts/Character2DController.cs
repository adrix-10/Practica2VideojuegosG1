using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character2DController : MonoBehaviour
{
    const string STAR = "star";
    const string KEY = "key";

    [Header("Movement")]
    [SerializeField]
    float walkSpeed = 500.0F;

    [SerializeField]
    [Range(0.01F, 0.3F)]
    float smoothSpeed = 0.3F;

    [SerializeField]
    bool isFacingRight = true;

    [Header("Jump")]
    [SerializeField]
    float jumpForce = 200.0F;

    [SerializeField]
    float fallMultiplier = 5.0F;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    float jumpGraceTime = 0.25F;

    [Header("Animation")]
    [SerializeField]
    Animator animator;

    IDictionary<string, int> collectibles;

    public UnityEvent<int> OnStarCountChanged;
    public UnityEvent<string, int> OnInteract;

    Rigidbody2D rb;

    Vector2 gravity;
    Vector2 velocityZero = Vector2.zero;

    float inputX;
    float lastTimeJumpPressed;
    bool isMoving;
    bool isJumpPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gravity = -Physics2D.gravity;

        //se lleva el conteo de las cosas que van incrementando, como las estrellas que se agarran en el juego.
        collectibles = new Dictionary<string, int>
        {
            { STAR, 0 },
            { KEY, 0 }
        };
    }
    /// <summary>
    /// se ejecuta cuadro a cuadro
    /// </summary>
    private void Update()
    {
        HandleInputs();
    }
    private void FixedUpdate()
    {
        HandleJump();
        HandleMove();
        HandleFlipX();
    }

    /// <summary>
    /// Verifica las entradas del jugador por cada cuadro
    /// </summary>
    private void HandleInputs()
    {
        // Movimiento Horizontal
        inputX = Input.GetAxisRaw("Horizontal");
        isMoving = inputX != 0.0F;
        isJumpPressed = Input.GetButtonDown("Jump");
        if (isJumpPressed) 
        {
            lastTimeJumpPressed = Time.time;
        }
    }
    /// <summary>
    /// Mueve el personaje horizontalmente
    /// </summary>
    void HandleMove() 
    {   // calcula la velocidad en X
        float velocityX = inputX * walkSpeed * Time.fixedDeltaTime;
        animator.SetFloat("speed", Mathf.Abs(velocityX));

        // Crea el vector de direccion y asigna la velocidad
        Vector2 direction = new Vector2(velocityX, rb.velocity.y);
        //rb.velocity = direction;
        rb.velocity =
            Vector2.SmoothDamp
                (rb.velocity, direction, ref velocityZero, smoothSpeed);
    }
    /// <summary>
    /// Gira el personaje de izquierda a derecha o viceversa cuando en movimiento
    /// </summary>
    void HandleFlipX() 
    { 
        if (isMoving) 
        {
            // Calcula hacia donde esta mirando el personaje
            bool facingRight = inputX > 0.0F;
            if (isFacingRight != facingRight) 
            {
                // Rota el personaje en el eje Y
                isFacingRight = facingRight;
                Vector2 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }

        }
    }

    void HandleJump() 
    {
        if (lastTimeJumpPressed > 0.0F && Time.time - lastTimeJumpPressed <= jumpGraceTime)
        {
            isJumpPressed = true;
        }
        else 
        {
            lastTimeJumpPressed = 0.0F;        
        }
        if (isJumpPressed && IsGrounded()) 
        {
            rb.velocity += Vector2.up * jumpForce * Time.fixedDeltaTime;
            return;
        }

        if (rb.velocity.y < -0.01F) 
        {
            rb.velocity -= gravity * fallMultiplier * Time.fixedDeltaTime;
        }
    }

    bool IsGrounded() 
    {
        // Parametros del return: 1. posicion, 2. tamaño, direccion, 4. angulo y 5. mascara
        return
            Physics2D.OverlapCapsule
            (groundCheck.position, new Vector2(0.63F, 0.4F),
            CapsuleDirection2D.Horizontal, 0.0F, groundMask);
    }

    public void IncreaseCollectible(string collectibleType, int value) 
    {
        collectibleType = collectibleType.ToLower();
        
        //esto pasa si recolectamos algo no reconocido
        if (!collectibles.ContainsKey(collectibleType))
        {
            return;
        }

        //Si lo encontramos, entonces le sumamos lo que nos dice el valor
        collectibles[collectibleType] += value;

        //hacemos un switch para ver que hacemos en cada caso con lo que recogimos
        switch (collectibleType)
        {
            case STAR:
                if (OnStarCountChanged != null)
                {
                    OnStarCountChanged.Invoke(collectibles[collectibleType]);
                }
                break;
        }
    }

    public bool ContainsCollectible(string collectibleType, int value = 1)
    {
        collectibleType = collectibleType.ToLower();

        //si llave no existe se sale
        if (!collectibles.ContainsKey(collectibleType))
        {
            return false;
        }

        return collectibles[collectibleType] >= value;
    }

    public void ReduceCollectible(string collectibleType, int value)
    {
        collectibleType = collectibleType.ToLower();

        //esto pasa si recolectamos algo no reconocido
        if (!collectibles.ContainsKey(collectibleType))
        {
            return;
        }

        //Si lo encontramos, entonces le sumamos lo que nos dice el valor
        collectibles[collectibleType] -= value;

        //hacemos un switch para ver que hacemos en cada caso con lo que recogimos
        switch (collectibleType)
        {
            case KEY:
                if (OnInteract != null)
                {
                    OnInteract.Invoke(KEY, collectibles[collectibleType]);
                }
                break;
        }
    }
}
