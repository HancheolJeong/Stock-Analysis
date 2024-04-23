using System.Text.Json;

namespace Stock_Analysis.Controllers
{
    public static class SessionExtensions
    {
        /// <summary>
        /// 세션에 키로 객체를 저장 객체는 JSON으로 직렬화
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session">세션 인스턴스</param>
        /// <param name="key">세션 키</param>
        /// <param name="value">세션에 저장 될 객체</param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// 세션에서 키로 객체를 검색 JSON을 역직렬화 해서 반환
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session">세션 인스턴스</param>
        /// <param name="key">세션 키</param>
        /// <returns></returns>
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
