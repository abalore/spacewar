using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject sunPrefab;
    [SerializeField] private Ufo ufoPrefab;
    [SerializeField] private Rocket rocket;
    [SerializeField] private Text text;
    [SerializeField] private Text text2;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Shot shotPrefab;

    private float turn = 0;
    private float thrust = 0;
    private GameObject expl;
    private int score;
    private bool alive;
    private bool win;
    private int deadCounter;
    private int ufoCounter;
    private int ufoTime;
    private int toKill;
    private int killed;
    private int winCounter;
    private float ufoSpeed;
    private int timeBonus;

    private List<Transform> suns;

    public static int Level;

    void Awake()
    {
        suns = new List<Transform>();
        Application.targetFrameRate = 60;
        score = 0;
        alive = true;
        win = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        switch(Level)
        {
            case 0:
                rocket.transform.position = new Vector3(0, 0, 0);
                ufoTime = 120;
                toKill = 25;
                ufoSpeed = 0.01f;
                break;
            case 1:
                ufoTime = 60;
                rocket.transform.position = new Vector3(0, 1, 0);
                GameObject sun = Instantiate(sunPrefab);
                sun.transform.position = new Vector3(0, -2.5f, 0);
                suns.Add(sun.transform);
                toKill = 25;
                ufoSpeed = 0.01f;
                break;
            case 2:
                rocket.transform.position = new Vector3(0, 0, 0);
                GameObject sun1 = Instantiate(sunPrefab);
                sun1.transform.position = new Vector3(0, 2, 0);
                suns.Add(sun1.transform);
                GameObject sun2 = Instantiate(sunPrefab);
                sun2.transform.position = new Vector3(0, -2, 0);
                suns.Add(sun2.transform);
                ufoTime = 30;
                toKill = 25;
                ufoSpeed = 0.015f;
                break;
            case 3:
                rocket.transform.position = new Vector3(0, 0, 0);
                ufoTime = 20;
                toKill = 100;
                ufoSpeed = 0.03f;
                break;
        }
        ufoCounter = 180;
        rocket.Death += Rocket_Death;
        killed = 0;
        winCounter = 0;
        timeBonus = 30;

        text2.text = killed.ToString("d3") + "/" + toKill.ToString("d3");
    }

    private void Rocket_Death()
    {
        if (!win)
        {
            expl = Instantiate(explosion);
            expl.transform.position = rocket.transform.position;
            rocket.gameObject.SetActive(false);
            alive = false;
            deadCounter = 180;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            timeBonus--;
            if (timeBonus<=0)
            {
                timeBonus = 30;
                score++;
            }

            if (win)
            {
                winCounter--;
                if (winCounter <= 0)
                {
                    Level++;
                    if (Level == 4)
                        Level = 0;
                    SceneManager.LoadScene("Game");
                }
            }

            foreach (Transform sun in suns)
            {
                Vector2 delta = sun.position - rocket.transform.position;
                Vector2 force = delta.normalized / (delta.magnitude * delta.magnitude) * 0.2f;
                rocket.AddForce(force);
            }

            rocket.SetAngularVelocity(turn);
            rocket.AddRelativeForce(new Vector2(0, thrust));

            Vector2 p = rocket.transform.position;
            if (p.y < -4.6f)
                p.y = 4.6f;
            else
                if (p.y > 4.6f)
                p.y = -4.6f;

            if (p.x < -3.3f)
                p.x = 3.3f;
            else
                if (p.x > 3.3f)
                p.x = -3.3f;

            rocket.transform.position = p;

            ufoCounter--;
            if (ufoCounter <= 0)
            {
                ufoCounter = ufoTime;

                float uy = Random.Range(0, 1f) > 0.5f ? -4.6f : 4.6f;
                float dy = -uy;
                float ux = Random.Range(-3.3f, 3.3f);
                float dx = Random.Range(-3.3f, 3.3f);

                if (Random.Range(0, 1f) > 0.5f)
                {
                    ux = Random.Range(0, 1f) > 0.5f ? -3.3f : 3.3f;
                    dx = -ux;
                    uy = Random.Range(-4.6f, 4.6f);
                    dy = Random.Range(-4.6f, 4.6f);
                }


                Vector2 s = new Vector2(dx - ux, dy - uy);
                s = s.normalized * 0.01f;
                Ufo ufo = Instantiate(ufoPrefab);
                ufo.Initialize(ux, uy, s.x, s.y);
                ufo.Death += Ufo_Death;
            }
        }
        else
        {
            deadCounter--;
            if (deadCounter <= 0)
                SceneManager.LoadScene("Menu");
        }
        text.text = score.ToString("d7");

    }

    private void Ufo_Death(Ufo ufo, bool natural)
    {
        Destroy(ufo.gameObject);
        GameObject expl = Instantiate(explosion);
        expl.transform.position = ufo.transform.position;
        if (!natural)
        {
            score += 25;
            if (killed < toKill)
            {
                killed++;
                if (killed == toKill)
                {
                    winCounter = 120;
                    score += 200;
                    win = true;
                }
            }
        }
        text2.text = killed.ToString("d3") + "/" + toKill.ToString("d3");
    }

    public void TurnRight()
    {
        turn = -100;
    }

    public void TurnLeft()
    {
        turn = 100;
    }

    public void TurnOff()
    {
        turn = 0;
    }

    public void ThrusterOn()
    {
        thrust = 20;
    }

    public void ThrusterOff()
    {
        thrust = 0;
    }

    public void Shoot()
    {
        if (alive)
        {
            Shot shot = Instantiate(shotPrefab);
            Vector3 speed = new Vector3(0, 0.1f, 0);
            speed = rocket.transform.rotation * speed;
            shot.Initialize(rocket.transform.position + speed * 2.5f + new Vector3(0, 0, -0.1f), speed);
        }
    }
}
