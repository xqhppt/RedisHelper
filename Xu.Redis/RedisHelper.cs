using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.Redis
{
    /// <summary>
    /// Redis帮助类
    /// xqh 2015-12-26
    /// </summary>
    public sealed class RedisHelper
    {
        /// <summary>
        /// 延时加载主
        /// </summary>
        private static Lazy<ConnectionMultiplexer> lazyMaster = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["MasterRedis"]);
        });

        /// <summary>
        /// 延时加载从
        /// </summary>
        private static Lazy<ConnectionMultiplexer> lazyRedis = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["SlaveRedis"]);
        });

        /// <summary>
        /// 主写
        /// </summary>
        public static ConnectionMultiplexer writeConn
        {
            get
            {
                return lazyMaster.Value;
            }
        }

        /// <summary>
        /// 从读
        /// </summary>
        public static ConnectionMultiplexer readConn
        {
            get
            {
                return lazyRedis.Value;
            }
        }

        /// <summary>
        /// 获取写实例
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private static IDatabase GetWriteDb(int db = 0, ConnectionMultiplexer conn = null)
        {
            if (conn == null)
            {
                return writeConn.GetDatabase(db);
            }
            else
            {
                return GetDb(conn, db);
            }
        }

        /// <summary>
        /// 获取读实例
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private static IDatabase GetReadDb(int db = 0, ConnectionMultiplexer conn = null)
        {
            if (conn == null)
            {
                return readConn.GetDatabase(db);
            }
            else
            {
                return GetDb(conn, db);
            }
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private static IDatabase GetDb(ConnectionMultiplexer conn, int db = 0)
        {
            return conn.GetDatabase(db);
        }


        private static Lazy<ConnectionMultiplexer> conn = null;

        static RedisHelper()
        {

        }

        public RedisHelper()
        {

        }

        public RedisHelper(string connectionString)
        {
            conn = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(connectionString);
            });
        }

        #region String

        /// <summary>
        /// 获取key对应的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="db">数据库编号</param>
        /// <param name="conn">连接器</param>
        /// <returns></returns>
        public static async Task<string> Get(string key,int db = 0,ConnectionMultiplexer conn = null)
        {
            return await GetReadDb(db, conn).StringGetAsync(key);
        }

        /// <summary>
        /// 设置键值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="db">数据库编号</param>
        /// <param name="ts">过期时间</param>
        /// <param name="conn">连接器</param>
        /// <returns></returns>
        public static async Task<bool> Set(string key, string value, int db = 0, TimeSpan? ts = null, ConnectionMultiplexer conn = null)
        {
            return await GetWriteDb(db, conn).StringSetAsync(key, value, ts);
        }

        #endregion

        #region Hash
        
        #endregion

        #region List
        
        #endregion

        #region Set
        
        #endregion

        #region SortedSet
        
        #endregion
    }
}
