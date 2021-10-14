using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    //LifeControll
    public int maxLife = 1000, lifeRec = 1;
    public int currentLife;
    public float delayRecovery = 0.1f;

    //Hud LifeBar
    public GameObject slider;
    public float delayLifeBar = 0;

    //Spawns
    public GameObject hitFX;

    private void Start()
    {
        slider.SetActive(false);
        slider.GetComponent<Slider>().maxValue = maxLife;
        currentLife = maxLife;
    }
    private void Update()
    {
        RecoveryLife();
        Slider();
    }

    public void TakeDMG(int dmg)
    {
        delayLifeBar = 5;
        currentLife -= dmg;
        Instantiate(hitFX, transform.position, Quaternion.identity);
        //SpawnHits
    }

    //Recupera x de vida acada X segundos
    void RecoveryLife()
    {
        if (currentLife < maxLife) delayRecovery -= Time.deltaTime;
        if (delayRecovery <= 0){
            currentLife += lifeRec; 
            delayRecovery = 1;
        }
    }

    //Controle da barra de vida
    void Slider()
    {
        slider.GetComponent<Slider>().value = currentLife;
        //O delay controla o tempo de exibição da barra de vida, 
        //quando o player parar de bater no inimigo, a barra desaparece em x segundos
        if(delayLifeBar > 0){ 
            slider.SetActive(true); 
            delayLifeBar -= Time.deltaTime; 
        }
        else{ 
            slider.SetActive(false); 
        }
    }
}
