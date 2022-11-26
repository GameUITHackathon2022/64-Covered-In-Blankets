using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public enum GameResult
{
    None,
    Win,
    Lose,
}
public class Player : MonoBehaviour
{
    List<GameObject> collected;
    public float velo = 0.05f;
    public Animator animator;
    public float trashOffset = 2f;
    public Rigidbody2D rb;

    public TextMeshProUGUI organicCountText;
    public int organicCount = 6;
    public GameObject organicPrefab;

    public TextMeshProUGUI plasticCountText;
    public int plasticCount = 6;
    public GameObject plasticPrefab;

    public GameObject gameWonCanvas;
    TrashType trashHolding = TrashType.None;

    public GameResult _result = GameResult.None;
    float horizontal;
    float vertical;
    bool walkingSoundPlaying = false;
    public bool canControl = true;
    protected
    // Start is called before the first frame update
    void Start()
    {
        collected = new List<GameObject>();
        organicCountText.text = organicCount.ToString() + " x";
        plasticCountText.text = plasticCount.ToString() + " x";

    }

    // Update is called once per frame
    void Update()
    {

            Move();
        CollectedRender();
        CheckWin();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            // CheckWin();
        }
#endif
    }
    void CheckWin()
    {
        if (_result == GameResult.None && organicCount == 0 && plasticCount == 0)
        {
            //win
            _result = GameResult.Win;
            gameWonCanvas.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void Move()
    {
        if (!canControl && Time.deltaTime > 0)
        {
            horizontal = vertical = 0f;
        }
        else
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        float speed = horizontal * horizontal + vertical * vertical;
        animator.SetFloat("Speed",Mathf.Sqrt(speed));
        rb.velocity = new Vector2(horizontal,vertical)*velo;
        if (speed > 0.1 && !walkingSoundPlaying)
        {
            StartCoroutine(PlayMoveSound());
            walkingSoundPlaying = true;
        }
        if (speed <= 0.1)
        {
            FindObjectOfType<AudioManager>().Stop("DiChuyen_1");
            walkingSoundPlaying = false;

        }
    }
    IEnumerator PlayMoveSound()
    {
        FindObjectOfType<AudioManager>().Play("DiChuyen_1");
        yield return new WaitForSeconds(1.776f);
        walkingSoundPlaying=false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<OrganicTrash>())
        {
            //organicCount--;
            //organicCountText.text = organicCount.ToString() + " x";
            //collision.gameObject.SetActive(false);
            collected.Add(collision.gameObject);
            FindObjectOfType<AudioManager>().Play("NhatRac_1");
        }
        if (collision.gameObject.GetComponent<PlasticTrash>())
        {
            //plasticCount--;
            //plasticCountText.text = plasticCount.ToString() + " x";
            //collision.gameObject.SetActive(false);
            collected.Add(collision.gameObject);
            FindObjectOfType<AudioManager>().Play("NhatRac_1");
        }

        if (collision.gameObject.GetComponent<Tornado>())
        {
            //rb.AddForce(-rb.velocity * 100);
            if (____torador != null)
                StopCoroutine(____torador);

            ____torador = _tornado(rb, rb.velocity);
            StartCoroutine(____torador);
            //rb.velocity = Vector2.zero;
        }

    }

    protected IEnumerator ____torador;
    protected IEnumerator _tornado(Rigidbody2D player, Vector2 vel)
    {
        canControl = false;

        float _time = .25f;
        float _currentTime = 0;
        while (_currentTime < _time)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime < _time * .9f)
                player.AddForce(-vel * .75f, ForceMode2D.Impulse);
            else
                player.AddForce(vel * .75f, ForceMode2D.Impulse);
            yield return null;
        }

        canControl = true;
    }
    void CollectedRender()
    {

        for (int i =0; i<collected.Count; i++)
        {
            if (i==0)
                collected[0].transform.position = new Vector2(transform.position.x, transform.position.y + 2);
            else
                collected[i].transform.position = new Vector2(transform.position.x, transform.position.y + trashOffset * (i+1));
        }
    }

    protected void remove(PlasticBin bin)
    {
        int _flashticCount = 0;
        for (int i = 0; i < collected.Count; i++)
        {
            if (collected[i].GetComponent<PlasticTrash>())
            {
                FindObjectOfType<AudioManager>().Play("BoRac");
                _flashticCount++;
                Destroy(collected[i].gameObject);
                collected.RemoveAt(i);
                i--;
            }
            else
            {
                break;
            }
        }
        plasticCount-= _flashticCount;
        plasticCountText.text = plasticCount.ToString() + " x";

    }
    protected void remove(OrganicBin bin)
    {
        int _flashticCount = 0;
        for (int i = 0; i < collected.Count; i++)
        {
            if (collected[i].GetComponent<OrganicTrash>())
            {
                FindObjectOfType<AudioManager>().Play("BoRac");
                _flashticCount++;
                Destroy(collected[i].gameObject);
                collected.RemoveAt(i);
                i--;
            }
            else
                break;
        }
        organicCount -= _flashticCount;
        organicCountText.text = organicCount.ToString() + " x";

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collected.Count > 0)
        {
            var orgBin = collision.gameObject.GetComponent<OrganicBin>();
            if (orgBin)
            {
                remove(orgBin);
                

            }
            var flsBin = collision.gameObject.GetComponent<PlasticBin>();
            if (flsBin)
            {
                remove(flsBin);
                

            }
        }
    }
}
