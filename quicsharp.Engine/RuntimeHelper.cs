using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace quicksharp.Engine
{
	internal static class RuntimeHelper
	{
		internal static string[] Inspect(object o)
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

		private static string getMemberValueString(object value)
		{
			if (value != null)
			{
				if (value is char && char.IsControl((char)value))
					return "0x" + Convert.ToByte((char)value).ToString("X2");
				else
					return value.ToString().Replace(@"\", @"\\");
			}
			return "(null)";
		}

		internal static object Resolve(object startObj, string accessor)
		{
			string[] accessors = accessor.Split('.');

			if (accessors.Length == 0)
				return startObj;

			object nextObj = startObj;

			var sb = new StringBuilder();
			var lastMember = new StringBuilder();

			for (int i = 0; i < accessors.Length; i++)
			{
				string member = accessors[i];

				if (member.StartsWith("!"))
					member = member.Substring(1);

				nextObj = GetAccessorValue(nextObj, member);

				//if (i == 0)
				//	sb.Append(member);
				//else
				//{
				//	if (lastMember.Length > 0)
				//		lastMember.Append(".");
				//	lastMember.Append(accessors[i - 1]);

				//	sb.AppendFormat(".GetType().GetProperty(\"{0}\").GetValue({1}, null)", member, lastMember.ToString());
				//}
			}

			return nextObj;
		}

		internal static List<string> Resolve2(string line)
		{

			/* TODO
			 * ASSIGNMENTS WONT WORK 
			 * abc.!Name = def.!Name;
			 * 
			 * METHODS WONT WORK
			 * ?abc.!GetType().!Name (arguments?!)
			*/

			line = line.Trim();
			if (line.EndsWith(";"))
				line = line.Substring(0, line.Length - 1);

			if (!line.Contains(".!"))
				return new List<string>() { line };

			var result = new List<string>();


			string[] expressions = line.Split('=').Select(s => s.Trim()).ToArray();
			string[] resultExpressions = new string[expressions.Length];

			// not supported -> "abc.!Name = def.!Name = ghi.!Name"
			if (expressions.Length > 2)
				throw new NotSupportedException("Forced member access is not supported within codelines with more than one value assignment!");

			for (int i = 0; i < expressions.Length; i++)
			{
				bool isAssignment = expressions.Length == 2 && i == 0;

				string[] accessors = expressions[i].Split('.');

				string prevAccessor = accessors[0];
				for (int x = 1; x < accessors.Length; x++)
				{
					bool isLastAccessor = (x == accessors.Length - 1);

					if (accessors[x].StartsWith("!"))
						prevAccessor = GetAccessorValueString(prevAccessor, accessors[x].Substring(1), isAssignment, isLastAccessor, ref result);
					else
						prevAccessor = GetAccessorValueString(prevAccessor, accessors[x], isAssignment, isLastAccessor, ref result);
						//prevAccessor += "." + accessors[x];
					
					resultExpressions[i] = prevAccessor;

				}

			}

			string lastLine = null;
			if (resultExpressions.Length == 1)
				lastLine = resultExpressions[0];
			else
				lastLine = resultExpressions[0].Replace("%DYNAMIC_GET_EXPRESSION%", resultExpressions[1]);

			result.Add(lastLine);

			return result;
		}

		private static string GetAccessorValueString(string objectName, string accessor, bool isAssignment, bool isLastAccessor, ref List<string> lines)
		{
			//string methodName = isAssignment ? "SetMemberValue" : "GetMemberValue";
			//return string.Format("{0}(\"{1}\")", methodName, accessor);
	
			if (isAssignment && isLastAccessor)
				return string.Format("{0}", GenerateReflectionWriteAccess(objectName, accessor, ref lines));

			return GenerateReflectionReadAccess(objectName, accessor, ref lines);
		}

		private static string GenerateReflectionWriteAccess(string objectName, string accessor, ref List<string> lines)
		{
			string accessorWithQuotes = "\"" + accessor + "\"";

			string notFoundMessage = string.Format("\"Could not resolve member \\\"{0}\\\"\"", accessor);
			return string.Format("GetType().GetProperty({2}) == null ? {0} : {1}.GetType().GetProperty({2}).SetValue({1}, %DYNAMIC_GET_EXPRESSION%)", notFoundMessage, objectName, accessorWithQuotes);
		}

		private static string GenerateReflectionReadAccess(string objectName, string accessor, ref List<string> lines)
		{
			string typeVar = "T" + Guid.NewGuid().ToString("n");
			string propertyVar = "P" + Guid.NewGuid().ToString("n");
			string methodVar = "M" + Guid.NewGuid().ToString("n");
			string fieldVar = "F" + Guid.NewGuid().ToString("n");
			string resultVar = "R" + Guid.NewGuid().ToString("n");

			

			string accessorWithQuotes = "\"" + accessor + "\"";

			lines.Add(string.Format("System.Type {0} = {1}.GetType();", typeVar, objectName));
			lines.Add(string.Format("System.Reflection.PropertyInfo {0} = {1}.GetProperty({2}, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);", propertyVar, typeVar, accessorWithQuotes));
			lines.Add(string.Format("System.Reflection.MethodInfo {0} = {1}.GetMethod({2});", methodVar, typeVar, accessorWithQuotes));
			lines.Add(string.Format("System.Reflection.FieldInfo {0} = {1}.GetField({2}, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);", fieldVar, typeVar, accessorWithQuotes));

			string notFoundMessage = string.Format("\"Could not resolve member \\\"{0}\\\"\"", accessor);
			//return string.Format("GetType().GetProperty({2}) == null ? {0} : {1}.GetType().GetProperty({2}).GetValue({1}, null)", notFoundMessage, objectName, "\"" + accessor + "\"");
			string exp = string.Format("({0} != null ? {0}.GetValue({3}, null) : ({1} != null ? {1}.Invoke({3}, new System.Object[0]) : ({2} != null ? {2}.GetValue({3}) : null)));", propertyVar, methodVar, fieldVar, objectName);
			lines.Add(string.Format("object {0} = {1}", resultVar, exp));
			return resultVar;
		}


		private static object GetAccessorValue(object obj, string member, object[] indexer = null)
		{
			return GetAccessorValue(obj, obj.GetType(), member, indexer);
		}

		private static object GetAccessorValue(object obj, Type t, string member, object[] indexer = null)
		{

			if (member.EndsWith(")"))
				return GetMethodValue(obj, t, member);

			if (member.EndsWith("]"))
				return GetIndexedValue(obj, t, member);

			var pi = t.GetProperty(member);
			if (pi != null)
				return pi.GetValue(obj, indexer);

			var fi = t.GetField(member);
			if (fi != null)
				return fi.GetValue(obj);

			//var mi = t.GetMethod(member);
			//if (mi != null)
			//	return mi.Invoke(obj, null);

			return null;
		}

		private static object GetMethodValue(object obj, Type t, string member)
		{
			var firstStop = member.IndexOf('(');
			var argumentsMatch = Regex.Match(member, @"\(.*(\w+).*\)");

			var methodName = member.Substring(0, firstStop);

			var mi = t.GetMethod(methodName);
			if (mi != null)
				return mi.Invoke(obj, null);

			return null;
		}

		private static object GetIndexedValue(object obj, Type t, string member)
		{
			var firstStop = member.IndexOf('[');
			object[] arguments = null;
			var argumentsMatch = Regex.Match(member, @"\[.*(\w+).*\]");
			if (argumentsMatch.Success)
			{
				string a = argumentsMatch.ToString();
				a = a.Substring(1, a.Length - 2);
				arguments = a.Split('.').Select(x => ChangeType(x)).ToArray();
			}

			member = member.Substring(0, firstStop);

			object resolvedObj = GetAccessorValue(obj, t, member);
			var memberInfo = resolvedObj.GetType().GetDefaultMembers().FirstOrDefault();
			if (memberInfo != null)
				return GetAccessorValue(resolvedObj, memberInfo.Name, arguments);

			return null;
		}

		private static object ChangeType(string value)
		{
			int a;
			if (int.TryParse(value, out a))
				return a;

			bool b;
			if (bool.TryParse(value, out b))
				return b;

			char c;
			if (char.TryParse(value, out c))
				return c;

			double d;
			if (double.TryParse(value, out d))
				return d;

			return value;
		}

		internal static MethodInfo[] GetMethods(Type t)
		{
			List<MethodInfo> result = new List<MethodInfo>();

			foreach (MethodInfo methodInfo in t.GetMethods(BindingFlags.Public | BindingFlags.Instance))
			{
				if (!GetIsPropertyGetterOrSetter(methodInfo.Name))
					result.Add(methodInfo);
			}

			return result.ToArray();
		}

		internal static string[] GetMethodParameters(MethodInfo method)
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

		internal static bool GetIsPropertyGetterOrSetter(string methodName)
        {
			return methodName.StartsWith("set_") ||
				methodName.StartsWith("get_") ||
				methodName.StartsWith("add_") ||
				methodName.StartsWith("remove_");
        }
    }
}
