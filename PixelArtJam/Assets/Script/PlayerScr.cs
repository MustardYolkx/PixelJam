using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerScr : MonoBehaviour
{
    private Camera cam;
    private SpriteRenderer sprite;
    public SpriteRenderer gunSprite;
    
    private float localScaleX;
    /// <summary>
    /// Player Attribute
    /// </summary>
    [Header("Attribute")]
    public float currentHP;
    public float maxHP;
    /// <summary>
    /// Player Movement Value
    /// </summary>
    [Header("Movement")]
    public float moveSpeed; // player Speed
    public float goundDrag;

    [HideInInspector]public bool isMovable;
    float horizonInput;
    float verticalInput;

    Vector2 moveDireciton;
    [HideInInspector]public Rigidbody2D rb;
    Transform playerTrans;

    /// <summary>
    /// Water Uptake
    /// </summary>
    [Header("WaterUptake")]
    public LayerMask waterLayer;
    public float waterStorage;
    public float waterStorageCap;
    public float waterUptakeSpeed;
    public GameObject waterCapcityTargetPos;
    public GameObject waterCapSprite;
    private Vector2 waterStorageOriginPos;
    private bool canAbsorb;
    /// <summary>
    /// Shoot
    /// </summary>
    [Header("Shoot")]
    public GameObject singleWaterBullet;
    public GameObject waterBullt;
    private float shootDelayTimeCount;
    public float shootDelay;
    public float bulletTime;
    public float singleBulletTime;
    public float bulletWaterConsume;
    public float singleBulletWaterConsume;
    public float singleBulletRecoil;
    private Vector2 mousePos;
    private float timeCount;
    private List<GameObject> waterBulletPos = new List<GameObject>();
    public GameObject shootVFX;
    public GameObject mouseFollow;
    /// <summary>
    /// Generate Line Render
    /// </summary>

    public GameObject targetLineRender;
    private LineRenderer line;
    [SerializeField] private float lineWidth = 1f;
    [SerializeField] private Material lineMaterial;

    public enum PlayerState
    {
        Idle,
        Move,
        Shoot,
    }
    public PlayerState currentState;

    /// <summary>
    /// Animation
    /// </summary>
    [Header("Animation")]
    Animator anim;
    public Animator gunAnim;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        localScaleX = sprite.transform.localScale.x;
        anim = GetComponentInChildren<Animator>();
        cam = FindObjectOfType<Camera>();
        rb = GetComponentInParent<Rigidbody2D>();
        playerTrans = GetComponentInParent<Transform>();
        if (!targetLineRender.GetComponent<LineRenderer>())
        {
            line = targetLineRender.gameObject.AddComponent<LineRenderer>();
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.material = lineMaterial;
        }
        else
        {
            line = targetLineRender. GetComponent<LineRenderer>();
        }
        waterStorageOriginPos = waterCapSprite.transform.localPosition;
    }

    private void FixedUpdate()
    {
        if(isMovable)
        {
            MovePlayer();
            SpriteFaceDirection();
        }
        GunFollowMouse();
    }
    // Update is called once per frame
    void Update()
    {
        MyInput();
        CheckWaterStorage();
        MousePosCheck();
        CheckMoveState();
        ChangeState();
        ChangeAnim();
        canAbsorb = !Input.GetMouseButton(0)&&!Input.GetMouseButton(1)&&moveDireciton==Vector2.zero;
        gunAnim.SetBool("Shooting",Input.GetMouseButton(0));
        
        WaterUptake();
        
        ChangeWaterSpriteCapcity();

        if (Input.GetMouseButtonDown(0))
        {
            shootDelayTimeCount = 0;
            gunAnim.SetTrigger("Shoot");
        }

        if (Input.GetMouseButton(0))
        {
            shootDelayTimeCount += Time.deltaTime;
            CameraShake.Instance.ShakeCameraCustom();
            Shoot();
            //GenerateLineRender();
        }
        timeCount += Time.deltaTime;
         rb.drag = goundDrag;
        if (Input.GetMouseButtonDown(1))
        {
            ShootRightMouse();
            CameraShake.Instance.ShakeCamera(1f);
            gunAnim.SetTrigger("ShootSingle");
        }
        GenerateLineRender();
        shootVFX.SetActive(Input.GetMouseButton(0));
        shootVFX.transform.LookAt(mouseFollow.transform.position);
    }
    private void CheckMoveState()
    {
        isMovable = !Input.GetMouseButton(0);
        if (timeCount > singleBulletTime/2)
        {
            isMovable = true;
        }
        else
        {
            isMovable = false;
        }
    }
    private void ChangeAnim()
    {
        anim.SetBool("isMove", currentState == PlayerState.Move);
        gunAnim.SetBool("Shooting", currentState == PlayerState.Shoot);
        
    }
    private void ChangeState()
    {
        if ((horizonInput != 0 || verticalInput != 0)&&isMovable)
        {
            currentState = PlayerState.Move;
        }
        else if (Input.GetMouseButton(0))
        {
            currentState = PlayerState.Shoot;
        }
        else
        {
            currentState = PlayerState.Idle;
        }
    }
    private void MyInput()
    {
        horizonInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    private void MovePlayer()
    {
        moveDireciton = playerTrans.up * verticalInput + playerTrans.right * horizonInput;
        rb.AddForce(moveDireciton.normalized * moveSpeed * 10f, ForceMode2D.Force);
    }
    private void SpriteFaceDirection()
    {
        if (horizonInput < 0)
        {
            sprite.transform.localScale = new Vector2(-localScaleX,sprite.transform.localScale.y);
        }
        else if(horizonInput > 0)
        {
            sprite.transform.localScale = new Vector2(localScaleX, sprite.transform.localScale.y);
        }
    }
    private void CheckWaterStorage()
    {
        if (waterStorage > waterStorageCap)
        {
            waterStorageCap = waterStorage;
        }
        else if (waterStorage < 0)
        {
            waterStorage = 0;
        }
    }
    private void WaterUptake()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.3f,waterLayer); // Get collider if nearby object is water
        if(collider != null)
        {
            Water nearbyWater = collider.gameObject.GetComponent<Water>(); //Get Water Script from this collider
            if (nearbyWater != null)
            {
                if (canAbsorb)                    
                {
                    if (Input.GetKey(KeyCode.R))                       
                    {
                        anim.SetBool("uptakeWater", true);
                        if (nearbyWater.storage > 0 && waterStorage < waterStorageCap)
                        {
                            waterStorage += waterUptakeSpeed * Time.deltaTime;
                            nearbyWater.storage -= waterUptakeSpeed * Time.deltaTime;
                        }
                    }
                    else
                    {
                        anim.SetBool("uptakeWater", false);
                    }
                }
                else
                {
                    anim.SetBool("uptakeWater", false);
                }

            }
            else
            {
                anim.SetBool("uptakeWater", false);
            }
        }
        else
        {
            anim.SetBool("uptakeWater", false);
        }
    }

    private void MousePosCheck()
    {

       mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

    }
    private void Shoot()
    {        
        if (waterStorage > 0)
        {
            if(shootDelayTimeCount>shootDelay)
            {
                if (timeCount > bulletTime)
                {
                    GameObject waterBullet = Instantiate(waterBullt, targetLineRender.transform.position, Quaternion.identity);
                    waterBullet.GetComponent<WaterBullet>().direction = (new Vector2(mouseFollow.transform.position.x, mouseFollow.transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
                    waterBulletPos.Add(waterBullet);
                    waterStorage -= bulletWaterConsume * Time.deltaTime;
                    timeCount = 0;
                }
            }
            
            
        }
    }
    private void ShootRightMouse()
    {
        if (waterStorage > 0)
        {
            if (timeCount > singleBulletTime)
            {
                
                GameObject waterBullet = Instantiate(singleWaterBullet, transform.position, Quaternion.identity);
                Vector2 pushDirection = (new Vector2(mouseFollow.transform.position.x, mouseFollow.transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
                waterBullet.GetComponent<WaterBulletSingle>().direction = pushDirection;
                waterStorage -= singleBulletWaterConsume;
                rb.AddForce(-pushDirection * singleBulletRecoil,ForceMode2D.Force);
                timeCount = 0;
            }
            
        }
    }
    public void BulletPosDelete(GameObject obj)
    {
        if (waterBulletPos.Contains(obj))
        {
            waterBulletPos.Remove(obj);
        }              
    }
    private void GenerateLineRender()
    {
        Vector3[] pos = new Vector3[waterBulletPos.Count];
        for (int i = 0; i < waterBulletPos.Count; i++)
        {
            if (waterBulletPos[i] != null)
            {
                pos[i] = waterBulletPos[i].transform.position;
            }
            
        }
        line.positionCount = waterBulletPos.Count;
        line.SetPositions(pos);
        
    }

    public void TakeDamage(float damage)
    {
        CameraShake.Instance.ShakeCamera(2);
        currentHP -= damage;
    }

    public void GunFollowMouse()
    {
        Vector2 direction = (mousePos - new Vector2(gunSprite.transform.position.x, gunSprite.transform.position.y)).normalized;
        float angle = Vector2.Angle(direction, Vector2.right);

        if (mouseFollow.transform.position.y> gunSprite.transform.position.y)
        {
           
            gunSprite.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
           
            gunSprite.transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
        if(mouseFollow.transform.position.x> gunSprite.transform.position.x)
        {
            gunSprite.transform.localScale = new Vector3(1, 1, 1);
            
        }
        else if(mouseFollow.transform.position.x < gunSprite.transform.position.x-0.5f)
        {
            gunSprite.transform.localScale = new Vector3(1, -1, 1);
            
        }
    }
    public void ChangeWaterSpriteCapcity()
    {
        float proportion = waterStorage / waterStorageCap;
        
        waterCapSprite.transform.localPosition = Vector2.Lerp(waterCapcityTargetPos.transform.localPosition, waterStorageOriginPos, proportion);
    }
}
