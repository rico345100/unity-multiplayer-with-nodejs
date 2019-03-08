using System;
using UnityEngine;

namespace Network {
	public static class ByteIO {
		// Write Int32 into specified offset
		public static void WriteInt(ref byte[] data, int offset, int variable) {
			Buffer.BlockCopy(BitConverter.GetBytes(variable), 0, data, offset, sizeof(int));
		}

		// Write Float into specified offset
		public static void WriteFloat(ref byte[] data, int offset, float variable) {
			Buffer.BlockCopy(BitConverter.GetBytes(variable), 0, data, offset, sizeof(float));
		}

		// Write Vector2 into specified offset
		public static void WriteVector2(ref byte[] data, int offset, Vector2 vector) {
			Buffer.BlockCopy(BitConverter.GetBytes(vector.x), 0, data, offset + sizeof(float) * 0, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(vector.y), 0, data, offset + sizeof(float) * 1, sizeof(float));
		}

		// Write Vector3 into specified offset
		public static void WriteVector3(ref byte[] data, int offset, Vector3 vector) {
			Buffer.BlockCopy(BitConverter.GetBytes(vector.x), 0, data, offset + sizeof(float) * 0, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(vector.y), 0, data, offset + sizeof(float) * 1, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(vector.z), 0, data, offset + sizeof(float) * 2, sizeof(float));
		}

		// Write Quaternion into specified offset
		public static void WriteQuaternion(ref byte[] data, int offset, Quaternion quaternion) {
			Buffer.BlockCopy(BitConverter.GetBytes(quaternion.x), 0, data, offset + sizeof(float) * 0, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(quaternion.y), 0, data, offset + sizeof(float) * 1, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(quaternion.z), 0, data, offset + sizeof(float) * 2, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(quaternion.w), 0, data, offset + sizeof(float) * 3, sizeof(float));
		}

		// Write byte[] into specified offset
		public static void WriteBytes(ref byte[] data, int offset, byte[] byteArray) {
			for(int i = 0; i < byteArray.Length; i++) {
				data[i + offset] = byteArray[i];
			}
		}

		// Write byte into specified offset
		public static void WriteByte(ref byte[] data, int offset, byte b) {
			data[offset] = b;
		}

		// Read Int32 from specified offset
		public static int ReadInt(ref byte[] data, int offset) {
			return BitConverter.ToInt32(data, offset);
		}

		// Read Float from specified offset
		public static float ReadFloat(ref byte[] data, int offset) {
			return BitConverter.ToSingle(data, offset);
		}

		// Read Vector2 from specified offset
		public static Vector2 ReadVector2(ref byte[] data, int offset) {
			return new Vector2(
				BitConverter.ToSingle(data, offset + sizeof(float) * 0),
				BitConverter.ToSingle(data, offset + sizeof(float) * 1)
			);
		}

		// Read Vector3 from specified offset
		public static Vector3 ReadVector3(ref byte[] data, int offset) {
			return new Vector3(
				BitConverter.ToSingle(data, offset + sizeof(float) * 0),
				BitConverter.ToSingle(data, offset + sizeof(float) * 1),
				BitConverter.ToSingle(data, offset + sizeof(float) * 2)
			);
		}

		// Read Quaternion from specified offset
		public static Quaternion ReadQuaternion(ref byte[] data, int offset) {
			return new Quaternion(
				BitConverter.ToSingle(data, offset + sizeof(float) * 0),
				BitConverter.ToSingle(data, offset + sizeof(float) * 1),
				BitConverter.ToSingle(data, offset + sizeof(float) * 2),
				BitConverter.ToSingle(data, offset + sizeof(float) * 3)
			);
		}

		// Read byte[] from specified offset
		public static byte[] ReadBytes(ref byte[] data, int offset, int length) {
			byte[] result = new byte[length];

			for(int i = 0; i < result.Length; i++) {
				result[i] = data[offset + i];
			}

			return result;
		}

		// Read byte from specified offset
		public static byte ReadByte(ref byte[] data, int offset) {
			return data[offset];
		}

		// API with Cursors
		public static void WriteInt(ref int cursor, ref byte[] data, int variable) {
			WriteInt(ref data, cursor, variable);
			cursor += sizeof(int);
		}

		public static void WriteFloat(ref int cursor, ref byte[] data, float variable) {
			WriteFloat(ref data, cursor, variable);
			cursor += sizeof(float);
		}

		public static void WriteVector2(ref int cursor, ref byte[] data, Vector2 vector) {
			WriteVector2(ref data, cursor, vector);
			cursor += sizeof(float) * 2;
		}

		public static void WriteVector3(ref int cursor, ref byte[] data, Vector3 vector) {
			WriteVector3(ref data, cursor, vector);
			cursor += sizeof(float) * 3;
		}

		public static void WriteQuaternion(ref int cursor, ref byte[] data, Quaternion quaternion) {
			WriteQuaternion(ref data, cursor, quaternion);
			cursor += sizeof(float) * 4;
		}

		public static void WriteBytes(ref int cursor, ref byte[] data, byte[] byteArray) {
			WriteBytes(ref data, cursor, byteArray);
			cursor += byteArray.Length;
		}

		public static void WriteByte(ref int cursor, ref byte[] data, byte b) {
			WriteByte(ref data, cursor, b);
			cursor++;
		}

		public static int ReadInt(ref int cursor, ref byte[] data) {
			int value = ReadInt(ref data, cursor);
			cursor += sizeof(int);
			return value;
		}

		public static float ReadFloat(ref int cursor, ref byte[] data) {
			float value = ReadFloat(ref data, cursor);
			cursor += sizeof(float);
			return value;
		}

		public static Vector2 ReadVector2(ref int cursor, ref byte[] data) {
			Vector2 value = ReadVector2(ref data, cursor);
			cursor += sizeof(float) * 2;
			return value;
		}

		public static Vector3 ReadVector3(ref int cursor, ref byte[] data) {
			Vector3 value = ReadVector3(ref data, cursor);
			cursor += sizeof(float) * 3;
			return value;
		}

		public static Quaternion ReadQuaternion(ref int cursor, ref byte[] data) {
			Quaternion value = ReadQuaternion(ref data, cursor);
			cursor += sizeof(float) * 4;
			return value;
		}

		public static byte[] ReadBytes(ref int cursor, ref byte[] data, int length) {
			byte[] value = ReadBytes(ref data, cursor, length);
			cursor += length;
			return value;
		}

		public static byte ReadByte(ref int cursor, ref byte[] data) {
			byte value = ReadByte(ref data, cursor);
			cursor++;
			return value;
		}
	}	
}
