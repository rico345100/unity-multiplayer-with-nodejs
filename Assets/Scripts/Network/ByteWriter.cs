using System;
using UnityEngine;

namespace Network {
	public class ByteWriter: ICursor {
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

		public ByteWriter(byte[] dataSource, int initOffset = 0) {
			this.m_DataSource = dataSource;
			this.m_Cursor = initOffset;
		}

		public void ResetCursor () {
			m_Cursor = 0;
		}

		public void MoveCursor(int offset) {
			m_Cursor = offset;
		}

		// Write Int32
		public void WriteInt(int variable) {
			Buffer.BlockCopy(BitConverter.GetBytes(variable), 0, m_DataSource, m_Cursor, sizeof(int));
			m_Cursor += sizeof(int);
		}

		// Write Float
		public void WriteFloat(float variable) {
			Buffer.BlockCopy(BitConverter.GetBytes(variable), 0, m_DataSource, m_Cursor, sizeof(float));
			m_Cursor += sizeof(float);
		}

		// Write Vector2
		public void WriteVector2(Vector2 vector) {
			Buffer.BlockCopy(BitConverter.GetBytes(vector.x), 0, m_DataSource, m_Cursor + sizeof(float) * 0, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(vector.y), 0, m_DataSource, m_Cursor + sizeof(float) * 1, sizeof(float));
			m_Cursor += sizeof(float) * 2;
		}

		// Write Vector3
		public void WriteVector3(Vector3 vector) {
			Buffer.BlockCopy(BitConverter.GetBytes(vector.x), 0, m_DataSource, m_Cursor + sizeof(float) * 0, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(vector.y), 0, m_DataSource, m_Cursor + sizeof(float) * 1, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(vector.z), 0, m_DataSource, m_Cursor + sizeof(float) * 2, sizeof(float));
			m_Cursor += sizeof(float) * 3;
		}

		// Write Quaternion
		public void WriteQuaternion(Quaternion quaternion) {
			Buffer.BlockCopy(BitConverter.GetBytes(quaternion.x), 0, m_DataSource, m_Cursor + sizeof(float) * 0, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(quaternion.y), 0, m_DataSource, m_Cursor + sizeof(float) * 1, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(quaternion.z), 0, m_DataSource, m_Cursor + sizeof(float) * 2, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(quaternion.w), 0, m_DataSource, m_Cursor + sizeof(float) * 3, sizeof(float));
			m_Cursor += sizeof(float) * 4;
		}

		// Write byte[]
		public void WriteBytes(byte[] byteArray) {
			for(int i = 0; i < byteArray.Length; i++) {
				m_DataSource[i + m_Cursor] = byteArray[i];
			}

			m_Cursor += byteArray.Length;
		}

		// Write byte
		public void WriteByte(byte b) {
			m_DataSource[m_Cursor] = b;
			m_Cursor++;
		}
	}
}