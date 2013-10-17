using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

/**
 * Part of lwjson library
 * Written by LLLF
 * Released to the public domain
 */
namespace Engine.JSON
{
    public static class Serialiser
    {
        //the inputs will be classes with properties of <type> or a list<type>
        //in the case of properties of <type> then we make a class
        //in the case of list<type>, write an array;
        public static String WriteClass(Object obj)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            PropertyInfo[] pi = obj.GetType().GetProperties();

            //loop thru all the properties in this type
            for (int i = 0; i < pi.Length; i++)
            {
                Type propertyType = pi[i].PropertyType.GetType();
                Object propertyValue = GetProperty(obj, pi[i].Name);

                if (propertyType.IsGenericType || propertyType.IsArray)
                {
                    sb.Append(WriteArray(propertyValue));
                }
                else
                {
                    //write a DataPair
                    sb.Append(WriteDataPair(pi[i].Name, propertyValue));
                }

                if (i != pi.Length - 1)
                {
                    sb.Append(',');
                }
            }

            sb.Append("}");

            return sb.ToString();
        }

        private static String WriteArray(Object obj)
        {
            StringBuilder sb = new StringBuilder();

            //need to cast this list<T> or array to list<Object>
            List<Object> objList;

            //have to use reflection to call this generic method
            MethodInfo method = typeof(Serialiser).GetMethod("CastToObjectList", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo generic = method.MakeGenericMethod(obj.GetType().GetGenericArguments()[0]);

            objList = (List<Object>)generic.Invoke(null, new Object[] { obj });

            //it's a type list, array it is, write a [ value, ... ]
            sb.Append('[');

            for (int i = 0; i < objList.Count; i++)
            {
                sb.Append(WriteValue(objList[i]));

                if (i != objList.Count - 1) //last one
                {
                    sb.Append(',');
                }
            }

            sb.Append(']');

            return sb.ToString();
        }

        //write a pair "name" : value
        private static String WriteDataPair(String name, Object obj)
        {
            StringBuilder sb = new StringBuilder();

            //write the name
            sb.Append('\"' + name + "\":");
            //write the value
            sb.Append(WriteValue(obj));

            return sb.ToString();
        }

        //just writes the 'value'
        private static String WriteValue(Object obj)
        {
            StringBuilder sb = new StringBuilder();
            if (obj == null)
                sb.Append("null");
            else
            {
                Type type = obj.GetType();

                //if its an object, WriteOut<T> again.
                //if its a primitative, write it, special case for string
                if (type.IsGenericType)
                {
                    sb.Append(WriteArray(obj));
                }
                else if (type == typeof(double))
                {
                    sb.Append(((double)obj).ToString());
                }
                else if (type == typeof(int))
                {
                    sb.Append(((int)obj).ToString());
                }
                else if (type == typeof(bool))
                {
                    sb.Append(((bool)obj).ToString());
                }
                else if (type == typeof(String))
                {
                    sb.Append('\"' + ((String)obj) + '\"');
                }
                else
                {
                    //its a class
                    sb.Append(WriteClass(obj));
                }
            }
            return sb.ToString();
        }

        private static Object GetProperty(Object obj, String propertyName)
        {
            return obj.GetType().InvokeMember(propertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, Type.DefaultBinder, obj, null);
        }

        private static List<Object> CastToObjectList<T>(List<T> list)
        {
            List<Object> objList = new List<Object>();
            foreach (T t in list)
                objList.Add(t);

            return objList;

        }

        //private static List<Object> CastToObjectList<T>(T[] array)
        //{
        //    List<Object> objList = new List<Object>();
        //    foreach (T t in array)
        //        objList.Add(t);

        //    return objList;
        //}
    }
}
