//using System;
//using UnityEngine;
//using static Utils;
//using static System.MathF;
//using vec = UnityEngine.Vector3;

//public class EntityBehaviour : MonoBehaviour
//{
//    const float ADJUSTMENT_PROMPTNESS = 0.001f;

//    vec _position;
//    Quaternion _rotation;

//    public IEntity Model { get; set; }

//    private void Update()
//    {
//        // View to Model

//        Model.Position += transform.position - _position;
//        Model.Rotate(Quaternion.Inverse(_rotation) * transform.rotation);
//        Model.Time += TimeSpan.FromSeconds(Time.deltaTime);

//        _position = transform.position;
//        _rotation = transform.rotation;
//        // �B���x�K�p�B

//        transform.position += Time.deltaTime * Model.Velocity;

//        transform.Rotate(Time.deltaTime * Model.AngularVelocity);
//    }

//    private void Model_PropertyChanged(object sender, EventArgs e)
//    {
//        // �A�ʒu���킹�B

//        transform.Adjust(to: Model, rate: 1 - Exp(-ADJUSTMENT_PROMPTNESS * Time.deltaTime));
//    }
//}