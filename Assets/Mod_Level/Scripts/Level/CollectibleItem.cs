using ER;
using ER.Items;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 物品收集光环
    /// </summary>
    public class CollectibleItem : Water
    {
        private SpriteRenderer sprite;
        public Sprite defaultSprite;

        private float speed = 10;
        private Transform aim;
        private bool follow = false;
        private Item item;
        public Item Item
        { get { return item; } }

        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        /// <summary>
        /// 更新物品信息(使用浅拷贝)
        /// </summary>
        /// <param name="item"></param>
        public void UpdateItemInfo(Item item)
        {
            this.item = item;
        }

        public override void ResetState()
        {
            sprite.sprite = defaultSprite;
            transform.position = Vector3.zero;
            speed = 10;
            aim = null;
            item = null;
            follow = false;
        }

        protected override void OnHide()
        {
            ResetState();
        }

        public void Follow(Transform _aim, float _speed = 10)
        {
            aim = _aim;
            speed = _speed;
            follow = true;
        }

        private void Update()
        {
            if (aim != null && follow)
            {
                Vector2 dir = (aim.position - transform.position).normalized;
                //Debug.Log(dir);
                transform.position += new Vector3(dir.x, dir.y, 0) * speed * Time.deltaTime;
            }
        }
    }
}