using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class TestScript : MonoBehaviour
{
    public float lev;
    public float q_lev = 0.5f;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private async void Update()
    {
        rb.AddForce(lev * Vector3.forward);
        if (Input.GetKey(KeyCode.W)) lev += Time.deltaTime * 0.1f;
        if (Input.GetKey(KeyCode.S)) lev -= Time.deltaTime * 0.1f;
        if (Input.GetKey(KeyCode.Q))
        {
            lev = q_lev;
            await Task.Delay(1000);
            lev = 0;
        }
    }
}