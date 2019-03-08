using System;
using UnityEngine;

namespace Network {
	public class ByteReader: ICursor {
		private int m_Cursor = 0;
		private byte[] m_DataSource;

		public int Cursor {
			get {
				return m_Cursor;
			}
		}

		public byte[] DataSource {
			get {
				return m_DataSource;
			}
		}

		public ByteReader(byte[] dataSource, int initOffset = 0) {
			this.m_DataSource = dataSource;
			this.m_Cursor = initOffset;
		}

		public void ResetCursor () {
			m_Cursor = 0;
		}

		public void MoveCursor(int offset) {
			m_Cursor = offset;
		}

		// Read Int32
		public int ReadInt() {
			var value = BitConverter.ToInt32(m_DataSource, m_Cursor);
			m_Cursor += sizeof(int);
			return value;
		}

		// Read Float
		public float ReadFloat() {
			var value = BitConverter.ToSingle(m_DataSource, m_Cursor);
			m_Cursor += sizeof(float);
			return value;
		}

		// Read Vector2
		public Vector2 ReadVector2() {
			var value = new Vector2(
				BitConverter.ToSingle(m_DataSource, m_Cursor + sizeof(float) * 0),
				BitConverter.ToSingle(m_DataSource, m_Cursor + sizeof(float) * 1)
			);

			m_Cursor += sizeof(float) * 2;

			return value;
		}

		// Read Vector3
		public Vector3 ReadVector3() {
			var value = new Vector3(
				BitConverter.ToSingle(m_DataSource, m_Cursor + sizeof(float) * 0),
				BitConverter.ToSingle(m_DataSource, m_Cursor + sizeof(float) * 1),
				BitConverter.ToSingle(m_DataSource, m_Cursor + sizeof(float) * 2)
			);

			m_Cursor += sizeof(float) * 3;

			return value;
		}

		// Read Quaternion
		public Quaternion ReadQuaternion() {
			var value = new Quaternion(
				BitConverter.ToSingle(m_DataSource, m_Cursor + sizeof(float) * 0),
				BitConverter.ToSingle(m_DataSource, m_Cursor + sizeof(float) * 1),
				BitConverter.ToSingle(m_DataSource, m_Cursor + sizeof(float) * 2),
				BitConverter.ToSingle(m_DataSource, m_Cursor + sizeof(float) * 3)
			);

			m_Cursor += sizeof(float) * 4;

			return value;
		}

		// Read byte[]
		public byte[] ReadBytes(int length) {
			byte[] result = new byte[length];

			for(int i = 0; i < result.Length; i++) {
				result[i] = m_DataSource[m_Cursor + i];
				m_Cursor++;
			}

			return result;
		}

		// Read byte
		public byte ReadByte() {
			var value = m_DataSource[m_Cursor];
			m_Cursor++;

			return value;
		}
	}
}