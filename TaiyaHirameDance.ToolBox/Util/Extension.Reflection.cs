using System.Reflection;

namespace TaiyaHirameDance.ToolBox
{
    public static class ReflectionExtension
    {
        /// <summary>
        /// 指定したカスタム属性が設定されている場合は true を返します。
        /// </summary>
        public static bool AnyCustomAttribute<T>(this MemberInfo member) where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), false).Any();
        }

        /// <summary>
        /// 指定したカスタム属性を取り出します。
        /// </summary>
        public static T FirstCustomAttribute<T>(this MemberInfo member) where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), false).Cast<T>().FirstOrDefault();
        }

        /// <summary>
        /// メソッドがオーバーライドしたものなら true を返します。
        /// </summary>
        public static bool IsOverrided(this MethodInfo method)
        {
            return method.GetBaseDefinition().DeclaringType != method.DeclaringType;
        }

        /// <summary>
        /// プロパティの型がnull許容型の場合、元の値型を返します。
        /// </summary>
        public static Type GetPrimitiveType(this PropertyInfo prop)
        {
            if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return prop.PropertyType.GenericTypeArguments[0];
            }
            else
            {
                return prop.PropertyType;
            }
        }
    }
}
