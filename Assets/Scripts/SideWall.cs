using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    // Skrip GameManager untuk mengakses skor maksimal
    // SerializeField digunakan untuk memanggil class lain
    [SerializeField]
    private GameManager gameManager;

    // Pemain yang akan bertambah skornya jika bola menyentuh dinding ini
    public PlayerControl player;

    // Akan dipanggil ketika objek lain ber-collider (bola) bersentuhan dengan dinding
    void OnTriggerEnter2D(Collider2D anotherCollider)
    {
        // Jika objek tersebut bernama "ball"
        if (anotherCollider.name == "Ball")
        {
            // Tambahnkan skor ke pemain
            player.IncrementScore();

            // Jika skor pemain belum mencapai skor maksimal
            if (player.Score < gameManager.maxScore)
            {
                // Restart game setelah bola mengenai dinding
                anotherCollider.gameObject.SendMessage("RestartGame", 2.0f, SendMessageOptions.RequireReceiver);
            }
        }
    }
}
