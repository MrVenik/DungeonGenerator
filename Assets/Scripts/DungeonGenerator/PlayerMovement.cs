using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;

        public float speed = 10;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        private Vector3 movement;
        private void Update()
        {

            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.transform.position + movement * speed * Time.fixedDeltaTime);
        }

    }

}
