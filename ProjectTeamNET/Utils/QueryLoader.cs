using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ProjectTeamNET.Utils
{
    /// <summary>

    /// ''' クエリを取得するためのユーティリティクラス

    /// ''' </summary>
    public static class QueryLoader
    {

        /// <summary>
        ///     ''' クエリを取得する
        ///     ''' </summary>
        ///     ''' <param name="functionId">機能ID</param>
        ///     ''' <param name="queryId">クエリID</param>
        ///     ''' <returns></returns>
        public static string GetQuery(string functionId, string queryId)
        {
            // 対象のXMLファイルを取得
            XmlDocument xmlDoc = new XmlDocument()
            {
                XmlResolver = new XmlUrlResolver()
            };

            xmlDoc.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\netcoreapp3.1\\", ""), $"SQL\\{functionId}.xml"));

            // 対象のSQLを取得
            var rawQuery = xmlDoc.GetElementById(queryId).InnerText;

            // CDATAセクションを除去
            var query = rawQuery.Replace("<![CDATA[", "").Replace("]]>", "");

            return query;
        }

        /// <summary>
        ///     ''' クエリを取得する
        ///     ''' </summary>
        ///     ''' <param name="functionId"></param>
        ///     ''' <param name="queryId"></param>
        ///     ''' <param name="addList"></param>
        ///     ''' <returns></returns>
        public static string GetQuery(string functionId, string queryId, List<string> addList)
        {
            // 対象のXMLファイルを取得
            XmlDocument xmlDoc = new XmlDocument()
            {
                XmlResolver = new XmlUrlResolver()
            };
            xmlDoc.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\netcoreapp3.1\\", ""), $"SQL/{functionId}.xml"));

            // 対象idの子ノードを取得
            var childNodes = xmlDoc.GetElementById(queryId).ChildNodes;

            var sb = new StringBuilder();

            // 子ノードの数分、処理を繰り返す
            foreach (XmlNode child in childNodes)
            {
                if ((child is XmlText))
                    // TypeがXmlTextの場合はValueをそのまま追加
                    sb.Append(child.Value);
                else if (("addif".Equals(child.Name)))
                {
                    // TypeがXmlText以外（XmlElement）でNameが「addif」の場合
                    if ((addList.Contains(child.Attributes["key"].Value)))
                        // keyが追加対象に含まれている場合はValueを追加
                        sb.Append(child.FirstChild.Value);
                }
                else
                {
                    // 上記以外
                    // TypeがXmlText以外（XmlElement）でNameが「choose」の場合
                    var added = false;

                    // 孫ノードの数分、処理を繰り返す
                    foreach (XmlNode grandson in child.ChildNodes)
                    {
                        // keyが追加対象に含まれていない場合は処理を飛ばす
                        if ((!addList.Contains(grandson.Attributes["key"].Value)))
                            continue;

                        if ((added))
                            // 孫ノード内のノードの値が既に追加されている場合はそのままValueを追加
                            sb.Append(grandson.FirstChild.Value);
                        else
                        {
                            // addStatementが指定されている場合はaddStatementのValueを追加
                            if ((HasValue(child.Attributes, "addStatement")))
                                sb.Append(child.Attributes["addStatement"].Value);

                            // プレフィックス（「AND」や「OR」）を削除してValueを追加
                            var prefix = grandson.Attributes["prefix"].Value;
                            if ((grandson.FirstChild.Value.Replace(Constants.vbCrLf, "").Trim().StartsWith(prefix)))
                                sb.Append(Strings.Replace(grandson.FirstChild.Value, prefix, "", Count: 1));
                            else
                                sb.Append(grandson.FirstChild.Value);
                            added = true;
                        }
                    }
                }
            }

            var query = sb.ToString();
            return query;
        }

        /// <summary>
        ///     ''' 値の有無をチェックする
        ///     ''' </summary>
        ///     ''' <param name="attributes"></param>
        ///     ''' <param name="key"></param>
        ///     ''' <returns></returns>
        private static bool HasValue(XmlAttributeCollection attributes, string key)
        {
            var result = (attributes[key] != null && !string.IsNullOrEmpty(attributes[key].Value));
            return result;
        }
    }
}
