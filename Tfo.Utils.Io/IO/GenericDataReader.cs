using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tfo.Utils.Io
{
    public class GenericDataReader<T> : IDataReader where T : class
    {
        private IEnumerator<T> _enumerator;

        private List<FieldInfo> _fields = new List<FieldInfo>();

        #region Ctor

        public GenericDataReader(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;

            foreach (FieldInfo fieldinfo in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                _fields.Add(fieldinfo);
            }
        }

        #endregion

        #region Implemented IDataReader Methods/Members

        public int FieldCount
        {
            get { return _fields.Count; }
        }

        public void Dispose()
        {
            Close();
        }

        public bool Read()
        {
            return _enumerator.MoveNext();
        }

        public void Close()
        {
            _enumerator.Dispose();
        }

        public Type GetFieldType(int i)
        {
            return _fields[i].FieldType;
        }

        public string GetName(int i)
        {
            return _fields[i].Name;
        }

        public object GetValue(int i)
        {
            return _fields[i].GetValue(_enumerator.Current);
        }

        #endregion

        #region Non-Implemented IDataReader Methods/Members

        public int Depth
        {
            get { throw new NotImplementedException(); }
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool IsClosed
        {
            get { throw new NotImplementedException(); }
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public int RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public object this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public object this[int i]
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
