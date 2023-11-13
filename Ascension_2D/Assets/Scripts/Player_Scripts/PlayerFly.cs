using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerFly : MonoBehaviour
{
      //public Animator anim;
      public Rigidbody2D rb;
      public float flyForce = 10f;
      //public Transform feet;
      //public LayerMask groundLayer;
      //public LayerMask enemyLayer;
      public bool canFly = false;
      private bool isFlying = false;
      public float flyTimer;
      public GameObject timerCircle;
      public float maxFlyTime = 5f;
      public bool isAlive = true;
      private bool movingUp = false;
      private GameObject[] groundObjs;
      //public AudioSource FlySFX;

      void Start(){
            //anim = gameObject.GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();

            // temporary
            canFly = true;
            flyTimer = 0f;

            // set ground obj array
            groundObjs = FindGroundObjects();
            for (int i = 0; i < groundObjs.Length; i++) {
                  groundObjs[i].GetComponent<BoxCollider2D>().isTrigger = false;
            }
      }

      void Update() {
            if (!movingUp && rb.velocity.y > 0) {
                  movingUp = true;
                  for (int i = 0; i < groundObjs.Length; i++) {
                        groundObjs[i].GetComponent<BoxCollider2D>().isTrigger = true;
                  }
            } else if (movingUp && rb.velocity.y <= 0) {
                  movingUp = false;
                  for (int i = 0; i < groundObjs.Length; i++) {
                        groundObjs[i].GetComponent<BoxCollider2D>().isTrigger = false;
                  }
            }
            // update fly timer
            // if (isFlying) {
            //     if (flyTimer > 0f) {
            //         flyTimer -= Time.deltaTime;
            //     }
            // } else if (flyTimer < maxFlyTime) {
            //     flyTimer += Time.deltaTime;
            // }
            // Text flyTime = timerText.GetComponent<timerText>();
            // flyTime = "" + Mathf.Round(flyTimer);

           if ((Input.GetKeyDown("up") || Input.GetKeyDown("w")) && 
               (canFly) && (isAlive) && (flyTimer > 0f)) {
                  canFly = false;
                  Fly();
            }
      }

      void FixedUpdate() {
            // update fly timer
            if (isFlying) {
                if (flyTimer > 0f) {
                    flyTimer -= Time.deltaTime;
                    if (flyTimer < 0f) {
                        flyTimer = 0f;
                    }
                }
            } else if (flyTimer < maxFlyTime) {
                flyTimer += Time.deltaTime;
                if (flyTimer > maxFlyTime) {
                  flyTimer = maxFlyTime;
                }
            }

            timerCircle.GetComponent<Image>().fillAmount = flyTimer / maxFlyTime;
      }

      GameObject[] FindGroundObjects() {
            GameObject[] goArray = SceneManager.
                                   GetSceneByName(SceneManager.GetActiveScene().name).
                                   GetRootGameObjects();
            var goList = new List<GameObject>();

            for (int i = 0; i < goArray.Length; i++) {
                  if (goArray[i].layer == 3) {
                        goList.Add(goArray[i]);
                  }
            }

            if (goList.Count == 0) {
                  return null;
            } else {
                  return goList.ToArray();
            }
      }

      void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.layer == 3 /* ground */) {
                isFlying = false;
            }
      }

      public void Fly() {
            //flyTimer += 1;
            isFlying = true;
            gameObject.GetComponent<AudioSource>().Play();
            rb.velocity = Vector2.up * flyForce;
            StartCoroutine(Flap());
            // wait for wing flap
            
            // anim.SetTrigger("Jump");
            // JumpSFX.Play();

            //Vector2 movement = new Vector2(rb.velocity.x, jumpForce);
            //rb.velocity = movement;
      }

      IEnumerator Flap() {
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponent<AudioSource>().Stop();
            canFly = true;
      }

    //   public bool IsGrounded() {
    //         Collider2D groundCheck = Physics2D.OverlapCircle(feet.position, 2f, groundLayer);
    //         Collider2D enemyCheck = Physics2D.OverlapCircle(feet.position, 2f, enemyLayer);
    //         if ((groundCheck != null) || (enemyCheck != null)) {
    //               //Debug.Log("I am trouching ground!");
    //               jumpTimes = 0;
    //               return true;
    //         }
    //         return false;
    //   }
}
