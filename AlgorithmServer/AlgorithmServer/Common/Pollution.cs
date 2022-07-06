using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AlgorithmServer
{
    /// <summary>
    /// 扩展方法包,用于给特定类型附加扩展方法
    /// </summary>
    public static class Pollution
    {
        public static R Mapping<R, T>(this T model, R model1)
        {

            foreach (PropertyInfo info in model1.GetType().GetProperties())
            {
                PropertyInfo pro = typeof(T).GetProperty(info.Name);
                if (pro != null)
                {
                    try
                    {
                        info.SetValue(model1, pro.GetValue(model, null), null);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(info.Name);
                        Console.WriteLine(e.Message);
                    }

                }

            }
            return model1;
        }

       
        /// <summary>
        ///  返回当前数组的拷贝(其实和拷贝没关系- -)
        /// </summary>
        /// <typeparam name="T">数组里的数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static List<T> CopyList<T>(this IList<T> source) => (from T item in source
                                                                    select item).ToList();
        /// <summary>
        /// 用于对比及返回两个数组相交的项目
        /// </summary>
        /// <typeparam name="T">数组内部数据类型</typeparam>
        /// <param name="source">原始数组</param>
        /// <param name="ContrastSource">对比数组</param>
        /// <param name="Item">ref 第一个相交项</param>
        /// <returns>是否有相交</returns>
        public static bool Contains<T>(this IList<T> source, IList<T> ContrastSource, ref T Item) where T : class
        {
            foreach (T item in from T item in ContrastSource
                               from T iteMc2 in source
                               where item.GetHashCode() == iteMc2.GetHashCode()
                               select item)
            {
                Item = item;
                return true;
            }

            Item = null;
            return false;
        }
       
        /// <summary>
        /// 判断当前数据是否为null
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="Source">需要判断的数据</param>
        /// <param name="NewSource">如果为null则返回这个参</param>
        /// <returns>返回NewSource参</returns>
        public static T Nullable<T>(this T Source, T NewSource) where T : class => Source ?? NewSource;

        /// <summary>
        /// 判断string类型是否为“”
        /// </summary>
        /// <param name="Source">值</param>
        /// <param name="NewSource">如果为“”则返回需要的值</param>
        /// <returns></returns>
        public static string DefaultValue(this string Source, string NewSource) => Source == "" ? NewSource : Source;

        /// <summary>
        /// 对IEnumerable泛型类进行污染,循环访问IEnumerable泛型类里的内容,将每个结果发送给指定的方法
        /// </summary>
        /// <typeparam name="T">类型标示符</typeparam>
        /// <param name="Source">IEnumerable泛型数组</param>
        /// <param name="ForEach">将值发送给指定的方法,该方法接受一个传入参数且不返回值,如:Console.WriteLine()</param>
        public static void ForEach<T>(this IEnumerable<T> Source, Action<T> ForEach)
        {
            foreach (T item in Source)
            {
                ForEach(item);
            }
        }
        /// <summary>
        /// 对IEnumerable泛型类进行污染,循环访问IEnumerable泛型类里的内容,将每个结果发送给指定的方法
        /// </summary>
        /// <typeparam name="T">类型标示符</typeparam>
        /// <param name="Source">IEnumerable泛型数组</param>
        /// <param name="ForEach">将值发送给指定的方法,该方法接受一个传入参数且不返回值,如:Console.WriteLine()</param>
        public static void ForEach<T, R>(this IEnumerable<T> Source, Action<T, R> ForEach)
        {
            for (int i = 0; i < Source.Count(); i++)
            {
                ForEach(Source.ElementAt(i), (dynamic)i);
            }
        }
        /// <summary>
        /// 根据条件循环访问IEnumerable泛型类的内容,将满足条件的结果发送给指定的方法,不满足条件的结果直接返回
        /// </summary>
        /// <typeparam name="T">类型标示符</typeparam>
        /// <typeparam name="R">返回值的类型标示符</typeparam>
        /// <param name="Source">IEnumerable泛型数组</param>
        /// <param name="ForEach">将值发送给指定的方法,该方法接受一个传入参数且不返回值,如:Console.WriteLine()</param>
        /// <param name="Condition">用于对比的判断类型</param>
        /// <returns>List数组</returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> Source, Func<T, R> ForEach, dynamic Condition)
        {
            List<R> list = new List<R>();
            Source.ForEach(i =>
            {
                Dictionary<Predicate<bool>, Action> dic = new Dictionary<Predicate<bool>, Action>()
                {
                   { x => x == true,new Action(()=> list.Add(ForEach(i)))},
                   {x => x == false,new Action(()=>list.Add((R)(object)i))}
                };
                dic.Where(j => j.Key(i.GetHashCode() == Condition.GetHashCode())).ForEach(k => k.Value());
            });
            return list;
        }
        /// <summary>
        /// 对IEnumerable泛型类进行污染,循环访问IEnumerable泛型类里的内容,将每个结果发送给指定的方法,并返回指定方法运行后的结果
        /// </summary>
        /// <typeparam name="T">类型标示符</typeparam>
        /// <typeparam name="R">返回值的类型标示符</typeparam>
        /// <param name="Source">IEnumerable泛型数组</param>
        /// <param name="ForEach">将值发送给指定的方法,该方法接受一个传入参数且不返回值,如:Console.WriteLine()</param>
        /// <returns>方法执行后的运行结果列表</returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> Source, Func<T, R> ForEach)
        {
            List<R> list = new List<R>();
            Source.ForEach(i => list.Add(ForEach(i)));
            return list;
        }
        /// <summary>
        /// 对IEnumerable泛型类进行污染,循环访问IEnumerable泛型类里的内容,将每个结果发送给指定的方法
        /// </summary>
        /// <typeparam name="T">类型标示符</typeparam>
        /// <param name="Source">IEnumerable泛型数组</param>
        /// <param name="ForEach">将值发送给指定的方法,该方法接受一个传入参数且不返回值,如:Console.WriteLine()</param>
        public static void ForEach<T>(this IEnumerable Source, Action<T> ForEach)
        {
            Source.Cast<T>().ForEach(i => ForEach(i));
        }

        /// <summary>
        /// 返回从一个符号到另一个符号中间的值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="start">起始符号</param>
        /// <param name="end">结束符号</param>
        /// <returns></returns>
        public static string GetInterspaceString(this string str, char start, char end)
        {
            int a = str.IndexOf(start);
            int b = str.LastIndexOf(end);
            string temp;
            try
            {
                temp = str.Substring(a + 1, b - a - 1);
            }
            catch { return ""; }
            return temp;
        }
        /// <summary>
        /// 基于谓词筛选值序列.
        /// </summary>
        /// <typeparam name="T">序列的类型</typeparam>
        /// <param name="_Source">用于筛选的序列</param>
        /// <param name="_Predicate">用于测试每个元素是否满足条件的函数</param>
        /// <returns></returns>
        public static IEnumerable<T> Where<T>(this IEnumerable _Source, Predicate<T> _Predicate) where T : class
        {
            List<T> list = new List<T>();
            foreach (object item in _Source)
            {
                if (item is T t)
                {
                    if (_Predicate(t))
                    {
                        list.Add(t);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// int转16进制string字符串
        /// </summary>
        /// <param name="a">需要转换的INT</param>
        /// <returns></returns>
        public static string IntToString(this int a)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((0xff & a).ToString("X4"));
            return sb.ToString();
        }
        /// <summary>
        /// 把Byte数组连接成FF FF FF格式的字符串
        /// </summary>
        /// <param name="by">Byte数组</param>
        /// <returns>连接后的字符串</returns>
        public static string GetString(this byte[] by)
        {
            StringBuilder sb = new StringBuilder();
            by.ForEach(i => sb.Append(i.ToString("x2") + " "));
            return sb.ToString();
        }
        /// <summary>
        /// 把Byte数组按设定的数量分割成小数组用于传输
        /// </summary>
        /// <param name="by">原始数组</param>
        /// <param name="number">设定分割数组用的数量</param>
        /// <returns>返回小数组的List</returns>
        public static List<byte[]> GetSegmentation(this byte[] by, int number)
        {
            double ceiling = Math.Ceiling((double)by.Length / number);
            List<byte[]> list = new List<byte[]>();
            for (int i = 0; i < ceiling; i++)
            {
                byte[] s = by.Skip(i * number).Take(number).ToArray();
                list.Add(s);
            }
            return list;
        }
        public delegate string charater(BitArray bit);
        /// <summary>
        /// 字符串反转
        /// </summary>
        /// <param name="bit">bit</param>
        /// <param name="charater">接受一个bit,返回string</param>
        public static string Evert(this string str)
        {
            string temp = string.Empty;
            int i = 0;
            while (i < str.Count())
            {
                IEnumerable<char> a = str.Skip(i).Take(8);
                a.Reverse<char>().ForEach<char>(c => temp += c);
                i += 8;
            }
            return temp;
        }
        /// <summary>
        /// 位转byte
        /// </summary>
        /// <param name="bits">位数组</param>
        /// <returns></returns>
        public static byte ConvertToByte(this BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }
        /// <summary>
        /// 污染委托,字符处理
        /// </summary>
        /// <param name="bit">bit</param>
        /// <param name="charater">接受一个bit,返回string</param>
        public static string Charater(this BitArray bit, charater charater) => charater(bit);

        /// <summary>
        /// 对位进行处理,返回string
        /// </summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static string BitAter(this BitArray bit, int Begin, int Ending, string name, string State1, string State2, Func<string, bool> func)
        {
            string StrBT = bit.GetString().Substring(Begin, Ending);
            return func(StrBT) ? name + ":" + State1 : name + ":" + State2;
        }
        /// <summary>
        /// 转换位为字符串
        /// </summary>
        /// <param name="bit">位</param>
        /// <returns></returns>
        public static string GetString(this BitArray bit)
        {
            string str = null;
            foreach (object item in bit)
            {
                str += Convert.ToByte(item).ToString();
            }
            return str;
        }
        /// <summary>
        /// 把位转换为int
        /// </summary>
        /// <param name="bit">位</param>
        /// <returns></returns>
        public static int GetInt(this BitArray bit)
        {
            string strBT = bit.GetString();
            return Convert.ToInt32(strBT, 2);
        }
        /// <summary>
        /// 反转当前bit数据
        /// </summary>
        /// <param name="bit">位</param>
        /// <returns></returns>
        public static void Evert(this BitArray bit, int w)
        {

            int a = w;
            int i = 0;
            List<bool[]> list = new List<bool[]>();
            while (a != bit.Length + w)
            {
                IEnumerable<bool> ie = bit.OfType<bool>().Skip(bit.Length - a).Take(w);
                list.Add(ie.ToArray());
                a += w;
            }
            foreach (bool[] item in list)
            {
                foreach (bool items in item)
                {
                    bit[i] = items;
                    i++;
                }

            }


        }
        //反转当前byte数组
        public static byte[] GetEvert(this byte[] by)
        {
            List<byte> by1 = new List<byte>();
            for (int i = by.Length - 1; i >= 0; i--)
            {
                by1.Add(by[i]);
            }
            return by1.ToArray();
        }
        /// <summary>
        /// 反转当前bit数据
        /// </summary>
        /// <param name="bit">位</param>
        /// <returns></returns>
        public static void Evert(this BitArray bit)
        {
            IEnumerable<bool> ie = bit.OfType<bool>();
            bool[] Bl = ie.ToArray<bool>().Reverse<bool>().ToArray<bool>();
            int i = 0;
            foreach (bool item in Bl)
            {
                bit[i] = item;
                i++;
            }
        }
        /// <summary>
        /// 连接IEnumerable全部值
        /// </summary>
        /// <param name="ie">数据</param>
        /// <returns></returns>
        public static string Link16(this IEnumerable ie)
        {
            string str = null;
            foreach (object item in ie)
            {
                str += Convert.ToByte(item).ToString("X2");
            }
            return str;
        }
        public static string ToHexString(this byte[] by)
        {
            string HexString = string.Empty;
            if (by != null)
            {
                StringBuilder strB = new StringBuilder();
                foreach (byte item in by)
                {
                    strB.Append(item.ToString("X2"));
                }
                HexString = strB.ToString();
            }
            return HexString;
        }

        /// <summary>
        /// 获取字节中的指定Bit的值
        /// </summary>
        /// <param name="this">字节</param>
        /// <param name="index">Bit的索引值(0-7)</param>
        /// <returns></returns>
        public static int GetBit(this ushort @this, short index)
        {
            return (@this & (1 << index)) == (1 << index) ? 1 : 0;
        }

        /// <summary>
        /// 以高字节在前的规则两个short合成一个int
        /// </summary>
        /// <param name="this"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int GetIntByShort(this ushort @this, int num)
        {
            return Convert.ToInt32((@this << 16) | num);
        }

        public static ushort[] GetArraryByInt(this int @this)
        {
            ushort[] result = new ushort[2];
            result[0] = (ushort)(@this & ushort.MaxValue);
            result[1] = (ushort)(@this >> 16);

            return result;
        }

        /// <summary>
        /// 连接IEnumerable全部值
        /// </summary>
        /// <param name="ie">数据</param>
        /// <returns></returns>
        public static string Link(this IEnumerable ie)
        {
            string str = null;
            foreach (object item in ie)
            {
                str += Convert.ToInt32(item);
            }
            return str;
        }

        public static Dictionary<string, dynamic> GetDic(this Dictionary<string, string> dic)
        {
            Dictionary<string, dynamic> Dic = new Dictionary<string, dynamic>();
            dic.ForEach(i =>
            {
                Dic.Add(i.Key, i.Value);
            });
            return Dic;
        }

        #region string
        public static bool IsNullOrEmptyOrWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }
        #endregion

        #region json
        public static string JsonSerialize(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static T JsonDeserialize<T>(this string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default;
            }
        }

        public static T JsonDeepCopy<T>(this T obj)
        {
            return obj.JsonSerialize().JsonDeserialize<T>();
        }
        #endregion

        #region IEnumerable
        public static List<T> ToSingletonList<T>(this T obj)
        {
            return new List<T>()
            {
                obj
            };
        }
            
        #endregion
    }
}
