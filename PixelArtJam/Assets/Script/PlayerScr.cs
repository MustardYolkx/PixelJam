using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerScr : MonoBehaviour
{
    private Camera cam;
    private SpriteRenderer sprite;
    public SpriteRenderer gunSprite;
    /// <summary>
    /// Die
    /// </summary>
    public GameObject playerSpawnPoint;
    [HideInInspector] public bool groundUpCheck;
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
    public bool isOnGround;
    /*[HideInInspector]*/public bool isMovable;
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
    public GameObject waterCapcityLargeTargetPos;

    public GameObject waterCapSprite;
    public GameObject waterCapSpriteCom;
    public GameObject waterCapLargeSprite;
    public GameObject waterCapLargeSpriteCom;
    private Vector2 waterLargeStorageOriginPos;
    private Vector2 waterStorageOriginPos;
    private bool canAbsorb;
    private float uptakeReadyTimeCount;
    public float uptakeReadyTime;
    public float uptakeCoolDownTime;
    private float uptakeCoolDownTimeCount = 99;
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
    private float singleBulletTimeCount;
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
        UptakeWater,
    }
    public PlayerState currentState;

    /// <summary>
    /// Animation
    /// </summary>
    [Header("Animation")]
    Animator anim;
    public Animator gunAnim;

    private bool isAlive = true;
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
        waterLargeStorageOriginPos = waterCapLargeSprite.transform.localPosition;
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
        if(isAlive)
        {
            MyInput();
            CheckWaterStorage();
            MousePosCheck();
            CheckMoveState();
            ChangeState();
            ChangeAnim();
            canAbsorb = !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && moveDireciton == Vector2.zero && uptakeCoolDownTimeCount > uptakeCoolDownTime;
            gunAnim.SetBool("Shooting", Input.GetMouseButton(0));
            //anim.SetBool("UptakeWater", canAbsorb&&Input.GetKey(KeyCode.R));
            WaterUptake();
            TimeCount();
            ChangeWaterSpriteCapcity();
            //gunSprite.gameObject.SetActive(currentState != PlayerState.UptakeWater && isAlive);

            if (currentHP <= 0)
            {
                Die();
            }
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
        
    }
    private void TimeCount()
    {
        ///Uptake Water////
        if(Input.GetKeyDown(KeyCode.R))
        {
            uptakeReadyTimeCount = 0;
            anim.SetTrigger("UptakeWaterTrigger");
        }
        if(Input.GetKey(KeyCode.R))
        {
            
            uptakeReadyTimeCount += Time.deltaTime;
        }
        if(Input.GetKeyUp(KeyCode.R))
        {
            uptakeCoolDownTimeCount = 0;
        }
        uptakeCoolDownTimeCount += Time.deltaTime;
        timeCount += Time.deltaTime;
        singleBulletTimeCount+= Time.deltaTime;
    }
    private void CheckMoveState()
    {
        //isMovable = !Input.GetMouseButton(1);
        if (singleBulletTimeCount > singleBulletTime/2)
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
        else if (Input.GetKey(KeyCode.R))
        {
            currentState = PlayerState.UptakeWater;
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
    #region AnimTrigger
    public void SetWaterCapSpriteTrue()
    {
        waterCapSpriteCom.gameObject.SetActive(true);
    }
    public void SetWaterCapSpriteFalse()
    {
        waterCapSpriteCom.gameObject.SetActive(false);
    }
    public void SetWaterLargeCapSpriteTrue()
    {
        waterCapLargeSpriteCom.gameObject.SetActive(true);
    }
    public void SetWaterLargeCapSpriteFalse()
    {
        waterCapLargeSpriteCom.gameObject.SetActive(false);
    }

    public void SetGunSpriteFalse()
    {
        gunSprite.gameObject.SetActive(false);
    }
    public void SetGunSpriteTrue()
    {
        gunSprite.gameObject.SetActive(true);
    }
    #endregion
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
                        anim.SetBool("UptakeWater", true);
                        
                        if (uptakeReadyTimeCount>uptakeReadyTime)
                        {
                            
                            if (nearbyWater.storage > 0 && waterStorage < waterStorageCap)
                            {
                                waterStorage += waterUptakeSpeed * Time.deltaTime;
                                nearbyWater.storage -= waterUptakeSpeed * Time.deltaTime;
                            }
                        }
                        
                    }
                    else
                    {

                        
                        anim.SetBool("UptakeWater", false);
                        
                    }
                }
                else
                {
                    
                    anim.SetBool("UptakeWater", false);
                }

            }
            else
            {
                
                anim.SetBool("UptakeWater", false);
            }
        }
        else
        {
            
            anim.SetBool("UptakeWater", false);
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
            if (singleBulletTimeCount > singleBulletTime)
            {
                
                GameObject waterBullet = Instantiate(singleWaterBullet, transform.position, Quaternion.identity);
                Vector2 pushDirection = (new Vector2(mouseFollow.transform.position.x, mouseFollow.transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
                waterBullet.GetComponent<WaterBulletSingle>().direction = pushDirection;
                waterStorage -= singleBulletWaterConsume;
                rb.AddForce(-pushDirection * singleBulletRecoil,ForceMode2D.Force);
                singleBulletTimeCount = 0;
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
        anim.SetTrigger("UnderAtk");
        StartCoroutine(TakeDmgProcess());
    }

    public void Die()
    {
       StartCoroutine(DieProcess());
    }
    IEnumerator DieProcess()
    {
        isAlive = false;
        isMovable = false;
        anim.SetTrigger("Die");
        gunSprite.gameObject.SetActive(false);
        waterCapSpriteCom.SetActive(false);

        yield return new WaitForSeconds(1.6f);
        currentHP = maxHP;
        transform.position = playerSpawnPoint.transform.position;

        isAlive = true;
        isMovable = true;
        gunSprite.gameObject.SetActive(true);
        waterCapSpriteCom.SetActive(true);
        anim.SetTrigger("Alive");
    }
    IEnumerator FallingProcess()
    {
        isAlive = false;
        isMovable = false;
        anim.SetTrigger("FallingDown");
        if (groundUpCheck)
        {
            sprite.sortingLayerName = "Default";
            sprite.sortingOrder = 0;
        }
              
        gunSprite.gameObject.SetActive(false);
        waterCapSpriteCom.SetActive(false);
        yield return new WaitForSeconds(4);


            transform.position = playerSpawnPoint.transform.position;
            sprite.sortingLayerName = "Player";
            sprite.sortingOrder = 10;
            isAlive = true;
            isMovable = true;
            gunSprite.gameObject.SetActive(true);
            waterCapSpriteCom.SetActive(true);
            anim.SetTrigger("Alive");
        
    }
    public void FallingDown()
    {
        if(isAlive)
        {
            StartCoroutine(FallingProcess());
        }     
       
    }
    IEnumerator TakeDmgProcess()
    {
        waterCapSprite.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        waterCapSprite.gameObject.SetActive(true);
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
       waterCapLargeSprite.transform.localPosition = Vector2.Lerp(waterCapcityLargeTargetPos.transform.localPosition, waterLargeStorageOriginPos, proportion);
        waterCapSprite.transform.localPosition = Vector2.Lerp(waterCapcityTargetPos.transform.localPosition, waterStorageOriginPos, proportion);
    }
}
