using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public float preySpeed, predatorSpeed, playerSpeed, breedRate, predSpawnRate;

    [SerializeField] float hungerRate, hunger;

    public List<Transform> predators = new List<Transform>(), prey = new List<Transform>();

    [SerializeField] int score;

    [SerializeField] Animator playerAnim;

    [SerializeField] PlayerController playerCont;

    public bool playing;

    string loseReason;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        if (playing)
        {
            if (prey.Count == 0)
            {
                loseReason = "Triceratops  went  extinct !";
                GameOver();
            }
            else if(hunger == 0)
            {
                loseReason = "You  starved  to  death !";
                GameOver();      
            }

            PositionChecks();

            DifficultyUpdate();

            hunger = Mathf.Clamp(hunger - (hungerRate * Time.deltaTime), 0, 100);

            UIUpdate();
        }    
    }

    public void DeleteDinos()
    {
        for (int i = 0; i < predators.Count; i++)
        {
            Destroy(predators[i].gameObject);
        }

        for (int i = 0; i < prey.Count; i++)
        {
            Destroy(prey[i].gameObject);
        }
    }

    public void Restart()
    {
        predators = new List<Transform>();
        prey = new List<Transform>();

        Spawner.instance.lastPredSpawn = Time.timeSinceLevelLoad;
        Spawner.instance.SpawnPrey(Vector2.zero);
        Spawner.instance.SpawnPrey(new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f)));

        score = 0;
        hunger = 100;

        playerCont.enabled = true;

        MusicManager.instance.NewSong();

        StartCoroutine("startDelay");
    }

    void GameOver()
    {
        if(!PlayerPrefs.HasKey("High Score") || PlayerPrefs.GetInt("High Score") < score)
        {
            PlayerPrefs.SetInt("High Score", score);
        }

        playerCont.enabled = false;
        playerAnim.SetTrigger("Die");
        playerAnim.SetBool("Idle", true);

        SFXManager.instance.Lose();

        MusicManager.instance.StopPlaying();

        playing = false;

        UIManager.instance.GameOver(loseReason);
    }

    void PositionChecks()
    {
        for (int i = 0; i < prey.Count; i++)
        {
            if(prey[i] != null)
            {
                PreyController script = prey[i].GetComponent<PreyController>();

                if (script != null)
                {
                    script.nearestPredator = nearestPredator(prey[i]);
                }
            }  
        }

        for (int i = 0; i < predators.Count; i++)
        {
            if(predators[i] != null)
            {
                PredatorController script = predators[i].GetComponent<PredatorController>();

                if (script != null)
                {
                    script.nearestPrey = nearestPrey(predators[i]);
                }
            }  
        }
    }

    void DifficultyUpdate()
    {
        breedRate = Mathf.Clamp(5 + ((prey.Count - 4) * 4), 5, 100);
        predSpawnRate = Mathf.Clamp(3 - ((score - 90) / 260), 0.8f, 3);
        predatorSpeed = Mathf.Clamp(1 + ((score - 120) / 360), 1, 1.5f);
    }

    void UIUpdate()
    {
        UIManager.instance.score = score;
        UIManager.instance.hunger = (int)hunger;
    }

    Transform nearestPredator(Transform prey)
    {
        if(predators.Count == 0)
        {
            return null;
        }

        Transform nearestPred = predators[0];
        float shortestDist = 100;

        for (int x = 0; x < predators.Count; x++)
        {
            float distance = Vector3.Magnitude(prey.position - predators[x].position);

            if(distance < shortestDist)
            {
                shortestDist = distance;
                nearestPred = predators[x];
            }
        }

        return nearestPred;
    }

    Transform nearestPrey(Transform predator)
    {
        if (prey.Count == 0)
        {
            return null;
        }

        Transform nearestPrey = prey[0];
        float shortestDist = 100;

        for (int x = 0; x < prey.Count; x++)
        {
            float distance = Vector3.Magnitude(predator.position - prey[x].position);

            if (distance < shortestDist)
            {
                shortestDist = distance;
                nearestPrey = prey[x];
            }
        }

        return nearestPrey;
    }

    public void PredatorDeath(Transform obj)
    {
        predators.Remove(obj);
        obj.gameObject.GetComponentInChildren<Animator>().SetTrigger("Die");
        obj.GetComponent<PredatorController>().enabled = false;

        SFXManager.instance.Eat();

        AddScore(15);
        UIManager.instance.AddScore(15, obj.position);
        AddHunger(1);
    }

    public void PreyDeath(Transform obj) 
    {
        prey.Remove(obj);
        obj.gameObject.GetComponentInChildren<Animator>().SetTrigger("Die");
        obj.GetComponent<PreyController>().enabled = false;

        SFXManager.instance.Eat();

        AddScore(-10);
        UIManager.instance.AddScore(-10, obj.position);
    }

    public void PreyEaten(Transform obj)
    {
        prey.Remove(obj);
        obj.gameObject.GetComponentInChildren<Animator>().SetTrigger("Die");
        obj.GetComponent<PreyController>().enabled = false;

        SFXManager.instance.Eat();

        AddScore(10);
        UIManager.instance.AddScore(10, obj.position);
        AddHunger(10);
    }

    public void PreyBorn(Transform obj)
    {
        prey.Add(obj);

        SFXManager.instance.Birth();

        AddScore(5);
        UIManager.instance.AddScore(5, obj.position);
    }

    public void PredatorBorn(Transform obj)
    {
        predators.Add(obj);

        UIManager.instance.PredatorWarning(obj);
    }

    public void AddScore(int addition)
    {
        score = Mathf.Clamp(score + addition, 0, int.MaxValue);
    }

    void AddHunger(float addition)
    {
        hunger = Mathf.Clamp(hunger + addition, 0, 100);
    }

    IEnumerator startDelay()
    {
        yield return new WaitForEndOfFrame();

        playing = true;
    }
}
