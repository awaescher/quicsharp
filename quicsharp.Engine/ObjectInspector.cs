using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace quicsharp.Engine
{
	public static class ObjectInspector
	{
		public static string[] Inspect(this object o)
		{
			Type t = o.GetType();
			if (t != null)
			{
				var list = new List<string>();
				var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

				var members = t.GetMembers(bindingFlags).OrderBy(m => m.MemberType).ThenBy(m => m.Name);
				var methods = t.GetMethods(bindingFlags);

				var longestMemberCategory = members.Max(m => m.MemberType.ToString().Length);

				foreach (var member in members)
				{
					bool isPrivate = true;

					var pi = member.GetType().GetProperty("BindingFlags", bindingFlags);
					if (pi != null)
					{
						BindingFlags memberBindingFlags = (BindingFlags)pi.GetValue(member, null);
						isPrivate = (memberBindingFlags & BindingFlags.NonPublic) == BindingFlags.NonPublic;
					}

					bool skip = false;
					string memberName = member.Name;

					if (member.MemberType == MemberTypes.Method)
					{
						skip = true;
						if (!GetIsPropertyGetterOrSetter(memberName))
						{
							var methodInfos = methods.Where(m => m.Name == memberName).ToArray();

							foreach (var mi in methodInfos)
							{
								memberName += "(" + string.Join(", ", GetMethodParameters(mi)) + ")";
								AddMember(ref list, longestMemberCategory, MemberTypes.Method, isPrivate, memberName);
							}
						}
					}

					if (!skip)
						AddMember(ref list, longestMemberCategory, member, isPrivate, memberName);
				}

				return list.ToArray();
			}

			return null;
		}

		private static void AddMember(ref List<string> list, int longestMemberCategory, MemberInfo member, bool isPrivate, string memberName)
		{
			AddMember(ref list, longestMemberCategory, member.MemberType, isPrivate, memberName);
		}

		private static void AddMember(ref List<string> list, int longestMemberCategory, MemberTypes memberType, bool isPrivate, string memberName)
		{
			string memberCategory = (memberType.ToString() + ":").PadRight(longestMemberCategory + 1, ' ');
			list.Add(string.Format("[{0}] {1} {2}", isPrivate ? "-" : "+", memberCategory, memberName));
		}


		public static string[] GetMethodParameters(MethodInfo method)
		{
			// methods[i].GetParameters().OrderBy(p => p.Position).Select(p => p.ParameterType.Name + "" "" + p.Name))
			List<ParameterInfo> parameters = method.GetParameters().OrderBy(p => p.Position).ToList();

			if (parameters.Count == 0)
				return new string[0];

			string[] result = new string[parameters.Count - 1];
			for (int i = 0; i < parameters.Count - 1; i++)
			{
				result[i] = parameters[i].ParameterType.Name + " " + parameters[i].Name;
			}

			return result;
		}

		public static bool GetIsPropertyGetterOrSetter(string methodName)
		{
			return methodName.StartsWith("set_") ||
				methodName.StartsWith("get_") ||
				methodName.StartsWith("add_") ||
				methodName.StartsWith("remove_");
		}
	}
}
