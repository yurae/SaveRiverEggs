using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    private bool isClicked = false;

    public GameObject happy;
    public GameObject idle;
    public GameManager.OnClickEgg EventOnClickedEgg;

    private float origY = 0.9f;
    private float speed = 0f;
    private float updownRate = 1f;
    private float updownSpeed = 0.5f;

    public void OnMouseDown() {
        if (isClicked) return;
        StartCoroutine(ClickedCoroutine());
	}

    public void Init(int eggIdx, float spd, string name) {
        origY = GameManager.EggPos[eggIdx];
        speed = spd;
        transform.position = new Vector3(transform.position.x, origY, transform.position.z);

        SpriteRenderer idleSprite = idle.GetComponent<SpriteRenderer>();
        SpriteRenderer happySprite = happy.GetComponent<SpriteRenderer>();

        idleSprite.sprite = Resources.Load<Sprite>(name + "1");
        happySprite.sprite = Resources.Load<Sprite>(name + "2");

        switch (eggIdx) {
            case 0:
                idleSprite.sortingLayerName = "mid2";
                happySprite.sortingLayerName = "mid2";
                break;
            case 1:
                idleSprite.sortingLayerName = "mid3";
                happySprite.sortingLayerName = "mid3";
                break;
            case 2:
                idleSprite.sortingLayerName = "mid4";
                happySprite.sortingLayerName = "mid4";
                break;
            default:
                break;
		}

        updownRate = Random.Range(1.0f, 1.5f);
        updownSpeed = Random.Range(0.3f, 0.5f);
    }

    private float jumpAmount = 1.5f;
    private float jumpSpeed = 10f;
    private float timerUpdown = 0f;
    float updown = -1f;
    void Update() {
        Vector3 pos = transform.position;
        if (pos.x > 8) {
            Destroy(gameObject);
            return;
        }

        if (!isClicked) {
            transform.Translate(new Vector3(Time.deltaTime * speed, updown*updownSpeed*Time.deltaTime, 0f));
		}
        else {
            transform.position = new Vector3(pos.x, Mathf.Min(pos.y + Time.deltaTime * jumpSpeed, origY + jumpAmount), pos.z);
		}

        timerUpdown += Time.deltaTime;
        if (timerUpdown > updownRate) {
            timerUpdown = 0f;
            updown *= -1;
		}
    }

    private IEnumerator ClickedCoroutine() {
        isClicked = true;
        EventOnClickedEgg.Invoke();
        GetComponent<BoxCollider2D>().enabled = false;
        happy.SetActive(true);
        idle.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
	}

	public void OnDestroy() {

    }

}
