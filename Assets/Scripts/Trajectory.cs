using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    // Skrip, collider, dan rigidbody bola
    public BallControl ball;
    CircleCollider2D ballCollider;
    Rigidbody2D ballRigidbody;

    // Bola "bayangan" yang akan ditmapilkan titik tumbukan
    public GameObject ballAtCollision;

    void Start()
    {
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        // Inisiasi status pantulan lintasan, yang hanya akan ditampilkan jika lintasan bertumbukan dengan objek tertentu
        bool drawBallAtCollision = false;

        // Titik tumbukan digeser, untuk menggambar ballAtCollsion
        Vector2 offsetHitPoint = new Vector2();

        // Tentukan totol tumbukan dengan deteksi pergerakan lingkaran
        RaycastHit2D[] circleCastHit2DArray = Physics2D.CircleCastAll(ballRigidbody.position, ballCollider.radius, ballRigidbody.velocity.normalized);

        // Untuk setiap titik tumbukan
        foreach (RaycastHit2D circleCastHit2D in circleCastHit2DArray)
        {
            // jika terjadi tumbukan, dan tumbukan tersebut tidak dengan bola
            // (karena garis lintasan digambar dari titik tengah bola)
            if (circleCastHit2D.collider != null && circleCastHit2D.collider.GetComponent<BallControl>() == null)
            {
                // Garis lintasan akan digambar dari titik tengah bola saat ini ke titik tengah bola pada saat tumbukan
                // yaitu sebuah titik yang di-offset dari titik tumbukan berdasar vektor normal titik tersebut sebesar
                // jari-jari bola
                // Tentukan titik tumbukan
                Vector2 hitPoint = circleCastHit2D.point;

                // Tentukan normal di titik tumbukan
                Vector2 hitNormal = circleCastHit2D.normal;

                // Tentukan offsetHitPoint, yaitu titik tengah bola pada saat bertumbukan
                offsetHitPoint = hitPoint + hitNormal * ballCollider.radius;

                // Gambar garis lintasan dari titik tengah bola saat ini ke titik tengah bola pada saat bertumbukan
                DottedLine.DottedLine.Instance.DrawDottedLine(ball.transform.position, offsetHitPoint);

                // Kalau bukan sidewall, gambar pantulannya
                if (circleCastHit2D.collider.GetComponent<SideWall>() == null)
                {
                    // Hitung vektor datang
                    Vector2 inVector = (offsetHitPoint - ball.TrajectoryOrigin).normalized;

                    // Hitung vector keluar
                    Vector2 outVector = Vector2.Reflect(inVector, hitNormal);

                    // Hitung dot product dari outVector dan hitNormal. Digunakan supaya garis lintasan ketika
                    // Terjadi tumbukan tidak digambar
                    float outDot = Vector2.Dot(outVector, hitNormal);
                    if (outDot > -1.0f && outDot < 1.0)
                    {
                        // Gambar lintasan pantulannya
                        DottedLine.DottedLine.Instance.DrawDottedLine(offsetHitPoint, offsetHitPoint + outVector * 10.0f);

                        // Untuk menggambar bola "bayangan" di prediksi titik tumbukan
                        drawBallAtCollision = true;
                    }
                }

                // Jika true
                if (drawBallAtCollision)
                {
                    // Gambar bola "bayangan" di prediksi titik tumbukan
                    ballAtCollision.transform.position = offsetHitPoint;
                    ballAtCollision.SetActive(true);
                }
                else
                {
                    // Sembunyikan bola "bayangan"
                    ballAtCollision.SetActive(false);
                }

                // Hanya gambar lintasan untuk satu titik tumbukan, jadi keluar dari loop
                break;

            }
        }
    }
}
