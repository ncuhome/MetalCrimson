using ER;

namespace Mod_Forge
{

    public class PGroup : Water
    {
        private Water[] water;
        private int count = 0;

        public bool IsFull()
        {
            return count >= water.Length;
        }

        /// <summary>
        /// 向该容器添加元素
        /// </summary>
        /// <param name="w"></param>
        public void Add(Water w)
        {
            if (IsFull()) return;
            water[count] = w;
            count++;
            w.transform.SetParent(transform);
        }
        public void Clear()
        {
            for(int i = 0; i < count; i++)
            {
                water[i].Destroy();
            }
        }

        public Water this[int index]
        {
            get=> water[index];
        }

        public override void ResetState()
        {
            water = new Water[ComponentMaker.lay_max];
            count = 0;
        }

        protected override void OnHide()
        {
            Clear();
        }

        private void Awake()
        {
            ResetState();
        }
    }
}