using System.ComponentModel;
using System.Reflection;

namespace Hike.Models
{
    public class EnumInfoModel
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static List<EnumInfoModel> Parse(Type type)
        {
            var result = new List<EnumInfoModel>();
            var values = Enum.GetValues(type);
            foreach (var value in values)
            {
                var memberInfo = type.GetMember(value.ToString());
                var attributes = memberInfo.FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>();

                result.Add(new EnumInfoModel
                {
                    Value = (int)Convert.ChangeType(value, typeof(int)),
                    Name = value.ToString(),
                    Description = string.IsNullOrWhiteSpace(attributes?.Description) ? null : attributes.Description,
                });
            }
            return result;
        }
    }
}
