namespace Network {
	interface ICursor {
		int Cursor {
			get;
		}
		byte[] DataSource {
			get;
		}

		void ResetCursor();
		void MoveCursor(int offset);
	}
}