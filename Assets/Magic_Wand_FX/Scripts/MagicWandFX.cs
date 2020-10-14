using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWandFX : MonoBehaviour {

    public GameObject wand_FX;
    public GameObject spiralAnimNode;
    public GameObject fireBoltAnimNode;
    public GameObject wandParent;
    public GameObject wand_FX_LocationNode;

    public Light fireBoltLight;
    public Light wandFireLight;

    private Animation animA;
    private Animation animB;
    private bool wandMagicActive = false;

    // Use this for initialization
    void Start()
    {

         wand_FX.SetActive(false);

         animA = spiralAnimNode.GetComponent<Animation>();
         animB = fireBoltAnimNode.GetComponent<Animation>();
         animA["spiral"].speed = 5;
         animB["firebolt"].speed = 5;

    }


// Update is called once per frame
    void Update ()
    {

        if (Input.GetButtonDown("Fire1"))
        {

            if (wandMagicActive == false)

            {

                StartCoroutine("FireWand");

            }

        }

        if (FireBoltCollision.wandHit == true)
        {

            StartCoroutine("WandHitCollision");

        }

    }


    IEnumerator FireWand()
    {

        wandMagicActive = true;

        wand_FX.SetActive(true);

        // Unparent the wand fx so they don't move with the player
        wand_FX.transform.parent = null;

        animA["spiral"].speed = 5;
        animB["firebolt"].speed = 5;
        
        spiralAnimNode.GetComponent<Animation>().Play();
        fireBoltAnimNode.GetComponent<Animation>().Play();

        StartCoroutine("FireBoltFadeLight");
        StartCoroutine("WandFireFadeLight");

        yield return new WaitForSeconds(2.8f);
        
        // Parent the wand fx back to the player
        wand_FX.transform.parent = wandParent.transform;
        wand_FX.transform.position = new Vector3(wand_FX_LocationNode.transform.position.x, wand_FX_LocationNode.transform.position.y, wand_FX_LocationNode.transform.position.z);
        Vector3 newRotation = new Vector3 (wand_FX_LocationNode.transform.eulerAngles.x, wand_FX_LocationNode.transform.eulerAngles.y, wand_FX_LocationNode.transform.eulerAngles.z);
        wand_FX.transform.eulerAngles = newRotation;

        wand_FX.SetActive(false);

        wandMagicActive = false;

    }


    IEnumerator FireBoltFadeLight()
    {

        float t = 0.0f;

        while (t < 0.5f)
        {
            t += Time.deltaTime;
            fireBoltLight.intensity = Mathf.Lerp(4, 0, t / 0.5f);
            yield return 0;
        }

        t = 0;

    }


        IEnumerator WandFireFadeLight()
        {

        float t = 0.0f;

        while (t < 0.5f)
        {
            t += Time.deltaTime;
            wandFireLight.intensity = Mathf.Lerp(5, 0, t / 0.5f);
            yield return 0;
        }

            t = 0;

        }


    IEnumerator WandHitCollision()
    {

        print("Collision!");

        yield return new WaitForSeconds(0.2f);

        spiralAnimNode.GetComponent<Animation>().Stop();
        fireBoltAnimNode.GetComponent<Animation>().Stop();

        FireBoltCollision.wandHit = false;

    }


}
