using ER.Entity2D;

namespace Test
{
    public class Test : MonoAttribute
    {
        public Test()
        {
            AttributeName = "unim";
            print("UUIUI");
        }

        public override void Initialize()
        {
        }

        public void Awake()
        {
            print(AttributeName);
        }
    }
}