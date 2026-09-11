    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        public int totalKoin;
        private int koinTerkumpul = 0;
        private int jumlahZombieMati = 0;

        void OnEnable()
        {
            Enemy.OnZombieMati += SaatZombieMati;
        }

        void OnDisable()
        {
            Enemy.OnZombieMati -= SaatZombieMati;
        }

        void Start()
        {
            totalKoin = GameObject.FindGameObjectsWithTag("Coin").Length;
        }

        void SaatZombieMati(Enemy zombie)
        {
            jumlahZombieMati++;
            Debug.Log("GameManager dengar event. Zombie mati: " + jumlahZombieMati + " (" + zombie.name + ")");
        }

        public void AmbilKoin()
        {
            koinTerkumpul++;
            if (koinTerkumpul == totalKoin)
            {
                Menang();
            }
        }

        void Menang()
        {
            Debug.Log("KAMU MENANG!");
        }

        void OnGUI()
        {
            GUI.skin.label.fontSize = 22;
            GUI.Label(new Rect(16, 16, 480, 36), "Koin: " + koinTerkumpul + " / " + totalKoin);
        }
    }
