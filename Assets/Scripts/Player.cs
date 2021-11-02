using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityVersion 2021.1.9f1
public class Player : MonoBehaviour
{
    //Animations, Movement, Cam
    public Animator anim;
    public Rigidbody2D rb2d;
    public Joystick joystick;
    public Transform cam;

    //HitBox Area, Area Atk, Inimigos Layer 
    public Transform hitbox;
    public float atkRange = 0.5f;
    public LayerMask enemiesLayers;


    //move, timeAnimAtk
    public float speedMove = 3, moveX, meleeA1Duration, meleeA2Duration, meleeA3Duration, delayAtk;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        AnimClipTime();
    }
    void AnimClipTime()
    {
        //Passamos para clipsAnim todas as animações do player(obj)
      AnimationClip[] clipsAnim = anim.runtimeAnimatorController.animationClips;

        //pegamos o valor de duração das anims de atk e passamos para as var de duração
        foreach(AnimationClip clip in clipsAnim)
        {
            switch (clip.name)
            {
                case "meleeA1":
                    meleeA1Duration = clip.length;
                    break;
                case "meleeA2":
                    meleeA2Duration = clip.length;
                    break;
                case "meleeA3":
                    meleeA3Duration = clip.length;
                    break;
            }
        }
    }
    private void FixedUpdate(){
        //Camera
        cam.transform.position = new Vector3(transform.position.x, transform.position.y + 2, -10f);

        //Quando o delay do atk finalizar, muda o paramento isAtk para false
        if (delayAtk > 0) delayAtk -= Time.deltaTime;
        else anim.SetBool("isAtk", false);
        
        //Quando o paramentro isAtk das anim é false, libera os controles de movimentação
        if(!anim.GetBool("isAtk")) Move();

        //Combo utilizando o teclado
        KeyboardMeleeAtk();
    }

    void Move()
    {
        
        moveX = joystick.Horizontal; //Mobile
        float movePC = Input.GetAxisRaw("Horizontal"); //Keyboard
        
        if (moveX > 0 || movePC > 0)
        {
            anim.SetBool("isRuning", true);
            transform.eulerAngles = new Vector3(0, 0, 0);
            transform.position += new Vector3(2 * speedMove, 0, 0) * Time.deltaTime;
        }
        else if(moveX < 0 || movePC < 0)
        {

            anim.SetBool("isRuning", true);
            transform.eulerAngles = new Vector3(0, 180, 0);
            transform.position += new Vector3(-2 * speedMove, 0, 0) * Time.deltaTime;
        }
        else
        {
            anim.SetBool("isRuning", false);
        }
    }

    #region mobileAtk
    //botão para executar o combo
    public void BtnMeleeAtkCombo()
    {
        int i = Random.Range(0, 3);
        BtnMeleeAtk(i);
    }

    //botão para executar os atks de forma individual
    public void BtnMeleeAtk(float n){
        if (!anim.GetBool("isAtk"))
        {
            if (n == 0) AnimControll(true, n, meleeA1Duration);
            else if (n == 1) AnimControll(true, n, meleeA2Duration);
            else if (n == 2) AnimControll(true, n, meleeA3Duration);
        }
    }

    //Define isAtk como verdadeiro, qual atk é executado, e por quanto tempo isAtk ficara como true
    void AnimControll(bool state, float atkNum, float duration){
        anim.SetBool("isAtk", state);
        anim.SetFloat("atkMelee", atkNum);
        delayAtk = duration;
        Invoke("HitBox", 0.1f);
    }
    #endregion

    #region pcAtk
    void KeyboardMeleeAtk()
    {
        if (!anim.GetBool("isAtk"))
        {
            if (Input.GetKey(KeyCode.Alpha1)) AnimControll(true, 0, meleeA1Duration);
            else if (Input.GetKey(KeyCode.Alpha2)) AnimControll(true, 1, meleeA2Duration);
            else if (Input.GetKey(KeyCode.Alpha3)) AnimControll(true, 2, meleeA3Duration);
            else if (Input.GetKey(KeyCode.Space)) BtnMeleeAtkCombo();
        }
    }
    #endregion

    void HitBox()
    {
        //Detectar inimigos na range do atk
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitbox.position, atkRange, enemiesLayers);
        foreach(Collider2D e in hitEnemies)
        {
            e.GetComponent<Enemy>().TakeDMG(10);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hitbox.position, atkRange);
    }
}
