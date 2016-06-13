using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    [Serializable]
    public class FormItem
    {
        public int ID { get; set; }

        public string ItemName { get; set; }

        public string ItemValue { get; set; }

        string itemType = "System.String";
        public string ItemType
        {
            get
            {
                return itemType;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    itemType = value;
            }
        }

        public int Flag { get; set; }

        public const string FieldNamePrex = "Field";

        public static FormItem Empty(string newName)
        {
            return new FormItem() { ItemName = newName, ItemValue = "", ItemType = "System.String" };
        }

        public override string ToString()
        {
            return this.ItemName;
        }
    }

    public static class FormItemExtensions
    {
        public static int GetIndex(this List<FormItem> list, FormItem element)
        {
            int thisKey = element.GetHashCode();
            for (int i = 0; i < list.Count; i++)
            {
                int currKey = list[i].GetHashCode();
                if (thisKey == currKey)
                    return i;
            }
            return -1;
        }
        public static int GetIndex(this List<FormItem> list, FormItem element, out string newName)
        {
            int index = -1;
            int max = 0;
            int thisKey = element.GetHashCode();
            for (int i = 0; i < list.Count; i++)
            {
                int currKey = list[i].GetHashCode();
                if (thisKey == currKey)
                    index = i;
                string name = list[i].ItemName;
                if (name.StartsWith(FormItem.FieldNamePrex, StringComparison.InvariantCultureIgnoreCase))
                {
                    string num = name.Substring(FormItem.FieldNamePrex.Length);
                    int R;
                    if (int.TryParse(num, out R))
                    {
                        if (R > max)
                            max = R;
                    }
                }
            }
            newName = FormItem.FieldNamePrex + Convert.ToString(max + 1);
            return index;
        }
    }
}
