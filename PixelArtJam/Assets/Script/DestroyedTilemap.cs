using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyedTilemap : MonoBehaviour
{
    Tilemap destroyBody;
    Dictionary<Vector3Int, float> tileHealth;
    int totalTiles;
    public float singleBodyHP;

    public GameObject boomEffect;


    public float offsetX;
    public float offsetY;


    // Start is called before the first frame update
    void Start()
    {
        destroyBody = GetComponent<Tilemap>();
        tileHealth = new Dictionary<Vector3Int, float>();
        totalTiles = 0;

        foreach (var pos in destroyBody.cellBounds.allPositionsWithin)
        {
            if (destroyBody.HasTile(pos) && destroyBody.GetColliderType(pos) != Tile.ColliderType.None)
            {
                tileHealth.Add(pos, singleBodyHP);
                totalTiles++;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        WaterBullet bullet = collision.gameObject.GetComponentInChildren<WaterBullet>();
        WaterBulletSingle bulletSingle = collision.gameObject.GetComponentInChildren<WaterBulletSingle>();
        if (bullet != null)
        {

            float damage = bullet.damage;

            Vector3 hitPos = collision.ClosestPoint(bullet.transform.position);
            Destroy(collision.gameObject);

            Vector3Int[] tilePositions = new Vector3Int[]
            {
                destroyBody.WorldToCell(hitPos),
                destroyBody.WorldToCell(hitPos + new Vector3(offsetX, 0f, 0f)),
                destroyBody.WorldToCell(hitPos - new Vector3(offsetX, 0f, 0f)),
                destroyBody.WorldToCell(hitPos + new Vector3(0f, offsetY, 0f)),
                destroyBody.WorldToCell(hitPos - new Vector3(0f, offsetY, 0f)),
                destroyBody.WorldToCell(hitPos + new Vector3(offsetX, offsetY, 0f)),
                destroyBody.WorldToCell(hitPos + new Vector3(offsetX, -offsetY, 0f)),
                destroyBody.WorldToCell(hitPos - new Vector3(offsetX, -offsetY, 0f)),
                destroyBody.WorldToCell(hitPos - new Vector3(offsetX, offsetY, 0f))
            };


            foreach (Vector3Int tilePos in tilePositions)
            {
                TakeDamage(damage, tilePos, hitPos);
            }


        }
        if (bulletSingle != null)
        {
            //get damage value
            float damage = bulletSingle.damage;
            //collision point
            Vector3 hitPos = collision.ClosestPoint(bulletSingle.transform.position);
            Destroy(collision.gameObject);
            //expand check range
            Vector3Int[] tilePositions = new Vector3Int[]
            {
                destroyBody.WorldToCell(hitPos),
                destroyBody.WorldToCell(hitPos + new Vector3(offsetX, 0f, 0f)),
                destroyBody.WorldToCell(hitPos - new Vector3(offsetX, 0f, 0f)),
                destroyBody.WorldToCell(hitPos + new Vector3(0f, offsetY, 0f)),
                destroyBody.WorldToCell(hitPos - new Vector3(0f, offsetY, 0f)),
                destroyBody.WorldToCell(hitPos + new Vector3(offsetX, offsetY, 0f)),
                destroyBody.WorldToCell(hitPos + new Vector3(offsetX, -offsetY, 0f)),
                destroyBody.WorldToCell(hitPos - new Vector3(offsetX, -offsetY, 0f)),
                destroyBody.WorldToCell(hitPos - new Vector3(offsetX, offsetY, 0f))
            };


            foreach (Vector3Int tilePos in tilePositions)
            {
                TakeDamage(damage, tilePos, hitPos);
            }
        }
    }

    /// <summary>
    /// take damage
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="tilePos"></param>
    /// <param name="boomEffectPos"></param>
    public void TakeDamage(float damage, Vector3Int tilePos, Vector3 boomEffectPos)
    {

        if (destroyBody.HasTile(tilePos))
        {
            if (tileHealth.ContainsKey(tilePos))
            {
                tileHealth[tilePos] -= damage;

                if (tileHealth[tilePos] <= 0 && destroyBody.GetTile(tilePos) != null)
                {
                    //Remove tilemap in this pos
                    destroyBody.SetTile(tilePos, null);

                    totalTiles--;
                    //AudioManager.Instance.PlaySound(AudioName.Sound_EnemyDead);
                    //Play effect
                    //GameObject effect = Instantiate(boomEffect, boomEffectPos, Quaternion.identity);
                    //Destroy(effect, 1.5f);
                    //Remove From Dic
                    tileHealth.Remove(tilePos);
                }
            }
        }
    }


}
