using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace Triton.Core.Extension
{

    public static class Extension
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> self, string tableName, bool? removeFirstColumn)
        {
            var properties = typeof(T).GetProperties();

            var dataTable = new DataTable();
            foreach (var info in properties)
                dataTable.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType)
                                                 ?? info.PropertyType);

            foreach (var entity in self)
                dataTable.Rows.Add(properties.Select(p => p.GetValue(entity)).ToArray());

            if (removeFirstColumn == true)
                dataTable.Columns.RemoveAt(0);

            dataTable.TableName = tableName;
            return dataTable;
        }

        public static DataTable ToDataTableFromList<T>(this IList<T> data, bool removeFirstColumn)
        {
            var props = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            for (var i = 0; i < props.Count; i++)
            {
                var prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            var values = new object[props.Count];
            foreach (var item in data)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }

            if (removeFirstColumn == true)
                table.Columns.RemoveAt(0);
            return table;
        }

        public static DataTable ToDataTable<T>(T item, string tableName, bool? removeFirstColumn)
        {
            return ToDataTable(FromSingleItem(item), tableName, removeFirstColumn);
        }


        // usage: IEnumerableExt.FromSingleItem(someObject);
        public static IEnumerable<T> FromSingleItem<T>(T item)
        {
            yield return item;
        }

        public static bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
    }
}
