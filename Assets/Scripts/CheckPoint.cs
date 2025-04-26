using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckPoint : MonoBehaviour
{
      private Animator anim;
      public string id;
      public bool activationStatus;
      

      private void Start()
      {
            anim = GetComponent<Animator>();
      }

      [ContextMenu("Generate CheckPoint ID")]
      private void GenerateId()
      {
            id = Guid.NewGuid().ToString();
      }

      private void OnTriggerEnter2D(Collider2D other)
      {
            if (other.GetComponent<Player>() != null)
            {
                  ActiveCheckPoint();
            }
      }

      public void ActiveCheckPoint()
      {
            activationStatus = true;
            anim.SetBool("active",true);
      }
}
