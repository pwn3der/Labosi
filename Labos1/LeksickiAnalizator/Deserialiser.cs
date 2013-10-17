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
    public static class Deserialiser
    {
        public static T ReadClass<T>(String json)
        {
            Engine.JSON.Parser parser = new Engine.JSON.Parser(json);
            return (T) ReadClass(parser.Parse(), typeof(T));
        }

        private static Object ReadClass(Dictionary<String, Object> input, Type type)
        {
            Object obj = Activator.CreateInstance(type);

            foreach (KeyValuePair<String, Object> kp in input)
            {
                SetProperty(obj, kp.Key, ReadObject(kp.Value, obj.GetType().GetProperty(kp.Key).PropertyType));
            }

            return obj;
        }

        //obj is an object either Dictionary, list or numerical value/bool etc
        //returns an processed object
        private static Object ReadObject(Object obj, Type t)
        {
            if (obj is Dictionary<String, Object>) //class
            {
                return ReadClass((Dictionary<String, Object>) obj, t);
            }
            else if (obj is List<Object>) //array
            {
                Type listType;

                if (obj.GetType().IsGenericType)
                {
                    //we need to find out the type of the object list we need to create
                    listType = t.GetGenericArguments()[0];
                }
                else
                {
                    //array
                    listType = t.GetElementType();
                }

                //the list we'll set the property
                object createdList = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { listType }));

                //transfer all the elements into the created list
                foreach (Object arrayObj in (List<Object>) obj)
                {
                    //arrayobj needs processing too
                    createdList.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, Type.DefaultBinder, createdList, new Object[] { ReadObject(arrayObj, listType) });
                }

                //probably need a toarray T method sigh
                return createdList;
            }
            else if (obj == null)
            {
                return null;
            }
            else //it's a double, integer, bool w/e
            {
                return Convert.ChangeType(obj, t);
            }
        }

        private static void SetProperty(Object obj, String propertyName, Object value)
        {
            obj.GetType().InvokeMember(propertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, Type.DefaultBinder, obj, new Object[] {value} );
        }
    }
}
