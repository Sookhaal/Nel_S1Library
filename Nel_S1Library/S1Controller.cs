using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Nel_S1Library
{
	public class S1Controller
	{
		private Process[] p;

		public enum KeyboardMod : int
		{
			VK_SHIFT = 0x10,   //SHIFT key
			VK_CONTROL = 0x11,   //CTRL key
			VK_MENU = 0x12,   //ALT key
		}

		public enum MouseMod : int
		{
			MK_LBUTTON = 0x0001,
			MK_RBUTTON = 0x0002,
			MK_SHIFT = 0x0004,
			MK_CONTROL = 0x0008,
			MK_MBUTTON = 0x0010,
			MK_XBUTTON1 = 0x0020,
			MK_XBUTTON2 = 0x0040,
		}

		/// <summary>
		/// Virtual Keys
		/// </summary>
		public enum VKeys : int
		{
			VK_LBUTTON = 0x01,   //Left mouse button
			VK_RBUTTON = 0x02,   //Right mouse button
			VK_CANCEL = 0x03,   //Control-break processing
			VK_MBUTTON = 0x04,   //Middle mouse button (three-button mouse)
			VK_BACK = 0x08,   //BACKSPACE key
			VK_TAB = 0x09,   //TAB key
			VK_CLEAR = 0x0C,   //CLEAR key
			VK_RETURN = 0x0D,   //ENTER key
			VK_PAUSE = 0x13,   //PAUSE key
			VK_CAPITAL = 0x14,   //CAPS LOCK key
			VK_ESCAPE = 0x1B,   //ESC key
			VK_SPACE = 0x20,   //SPACEBAR
			VK_PRIOR = 0x21,   //PAGE UP key
			VK_NEXT = 0x22,   //PAGE DOWN key
			VK_END = 0x23,   //END key
			VK_HOME = 0x24,   //HOME key
			VK_LEFT = 0x25,   //LEFT ARROW key
			VK_UP = 0x26,   //UP ARROW key
			VK_RIGHT = 0x27,   //RIGHT ARROW key
			VK_DOWN = 0x28,   //DOWN ARROW key
			VK_SELECT = 0x29,   //SELECT key
			VK_PRINT = 0x2A,   //PRINT key
			VK_EXECUTE = 0x2B,   //EXECUTE key
			VK_SNAPSHOT = 0x2C,   //PRINT SCREEN key
			VK_INSERT = 0x2D,   //INS key
			VK_DELETE = 0x2E,   //DEL key
			VK_HELP = 0x2F,   //HELP key
			VK_0 = 0x30,   //0 key
			VK_1 = 0x31,   //1 key
			VK_2 = 0x32,   //2 key
			VK_3 = 0x33,   //3 key
			VK_4 = 0x34,   //4 key
			VK_5 = 0x35,   //5 key
			VK_6 = 0x36,    //6 key
			VK_7 = 0x37,    //7 key
			VK_8 = 0x38,   //8 key
			VK_9 = 0x39,    //9 key
			VK_A = 0x41,   //A key
			VK_B = 0x42,   //B key
			VK_C = 0x43,   //C key
			VK_D = 0x44,   //D key
			VK_E = 0x45,   //E key
			VK_F = 0x46,   //F key
			VK_G = 0x47,   //G key
			VK_H = 0x48,   //H key
			VK_I = 0x49,    //I key
			VK_J = 0x4A,   //J key
			VK_K = 0x4B,   //K key
			VK_L = 0x4C,   //L key
			VK_M = 0x4D,   //M key
			VK_N = 0x4E,    //N key
			VK_O = 0x4F,   //O key
			VK_P = 0x50,    //P key
			VK_Q = 0x51,   //Q key
			VK_R = 0x52,   //R key
			VK_S = 0x53,   //S key
			VK_T = 0x54,   //T key
			VK_U = 0x55,   //U key
			VK_V = 0x56,   //V key
			VK_W = 0x57,   //W key
			VK_X = 0x58,   //X key
			VK_Y = 0x59,   //Y key
			VK_Z = 0x5A,    //Z key
			VK_NUMPAD0 = 0x60,   //Numeric keypad 0 key
			VK_NUMPAD1 = 0x61,   //Numeric keypad 1 key
			VK_NUMPAD2 = 0x62,   //Numeric keypad 2 key
			VK_NUMPAD3 = 0x63,   //Numeric keypad 3 key
			VK_NUMPAD4 = 0x64,   //Numeric keypad 4 key
			VK_NUMPAD5 = 0x65,   //Numeric keypad 5 key
			VK_NUMPAD6 = 0x66,   //Numeric keypad 6 key
			VK_NUMPAD7 = 0x67,   //Numeric keypad 7 key
			VK_NUMPAD8 = 0x68,   //Numeric keypad 8 key
			VK_NUMPAD9 = 0x69,   //Numeric keypad 9 key
			VK_SEPARATOR = 0x6C,   //Separator key
			VK_SUBTRACT = 0x6D,   //Subtract key
			VK_DECIMAL = 0x6E,   //Decimal key
			VK_DIVIDE = 0x6F,   //Divide key
			VK_F1 = 0x70,   //F1 key
			VK_F2 = 0x71,   //F2 key
			VK_F3 = 0x72,   //F3 key
			VK_F4 = 0x73,   //F4 key
			VK_F5 = 0x74,   //F5 key
			VK_F6 = 0x75,   //F6 key
			VK_F7 = 0x76,   //F7 key
			VK_F8 = 0x77,   //F8 key
			VK_F9 = 0x78,   //F9 key
			VK_F10 = 0x79,   //F10 key
			VK_F11 = 0x7A,   //F11 key
			VK_F12 = 0x7B,   //F12 key
			VK_SCROLL = 0x91,   //SCROLL LOCK key
			VK_LSHIFT = 0xA0,   //Left SHIFT key
			VK_RSHIFT = 0xA1,   //Right SHIFT key
			VK_LCONTROL = 0xA2,   //Left CONTROL key
			VK_RCONTROL = 0xA3,    //Right CONTROL key
			VK_LMENU = 0xA4,      //Left MENU key
			VK_RMENU = 0xA5,   //Right MENU key
			VK_PLAY = 0xFA,   //Play key
			VK_ZOOM = 0xFB, //Zoom key
		}

		/// <summary>
		/// Virtual Messages
		/// </summary>
		public enum WMessages : int
		{
			WM_LBUTTONDOWN = 0x201, //Left mousebutton down
			WM_LBUTTONUP = 0x202,  //Left mousebutton up
			WM_LBUTTONDBLCLK = 0x203, //Left mousebutton doubleclick
			WM_RBUTTONDOWN = 0x204, //Right mousebutton down
			WM_RBUTTONUP = 0x205,   //Right mousebutton up
			WM_RBUTTONDBLCLK = 0x206, //Right mousebutton doubleclick
			WM_KEYDOWN = 0x100,  //Key down
			WM_KEYUP = 0x101,   //Key up
			WM_CHAR = 0x102,	//Character
		}

		/// <summary>
		/// Hooks to Studio One's process.
		/// </summary>
		/// <returns>Studio One's handle.</returns>
		public IntPtr GetS1()
		{
			p = Process.GetProcessesByName("studio one");
			if (p.Count() == 0)
				throw new Exception("Studio one was not found.");
			if (p.Count() > 1)
				throw new Exception("Too many processes.");
			return p[0].MainWindowHandle;
		}

		public void LeftClick(int x, int y)
		{
			LeftClick(x, y, GetS1(), 0);
		}

		public void LeftClick(int x, int y, MouseMod mod)
		{
			LeftClick(x, y, GetS1(), mod);
		}

		public void LeftClick(int x, int y, IntPtr s1)
		{
			LeftClick(x, y, s1, 0);
		}

		public void LeftClick(int x, int y, IntPtr s1, MouseMod mod)
		{
			PostMessage(s1, (int)WMessages.WM_LBUTTONDOWN, 1 + (int)mod, ToLong(x, y));
			PostMessage(s1, (int)WMessages.WM_LBUTTONUP, 0, ToLong(x, y));
		}

		public void RightClick(int x, int y)
		{
			RightClick(x, y, GetS1(), 0);
		}

		public void RightClick(int x, int y, MouseMod mod)
		{
			RightClick(x, y, GetS1(), mod);
		}

		public void RightClick(int x, int y, IntPtr s1)
		{
			RightClick(x, y, s1, 0);
		}

		public void RightClick(int x, int y, IntPtr s1, MouseMod mod)
		{
			PostMessage(s1, (int)WMessages.WM_RBUTTONDOWN, 2 + (int)mod, ToLong(x, y));
			PostMessage(s1, (int)WMessages.WM_RBUTTONUP, 0, ToLong(x, y));
		}

		public void SendKey(VKeys key)
		{
			SendKey(key, 0, 0, 0, GetS1());
		}

		public void SendKey(VKeys key, IntPtr s1)
		{
			SendKey(key, 0, 0, 0, s1);
		}

		public void SendKey(VKeys key, KeyboardMod mod0)
		{
			SendKey(key, mod0, 0, 0, GetS1());
		}

		public void SendKey(VKeys key, KeyboardMod mod0, IntPtr s1)
		{
			SendKey(key, mod0, 0, 0, s1);
		}

		public void SendKey(VKeys key, KeyboardMod mod0, KeyboardMod mod1)
		{
			SendKey(key, mod0, mod1, 0, GetS1());
		}

		public void SendKey(VKeys key, KeyboardMod mod0, KeyboardMod mod1, IntPtr s1)
		{
			SendKey(key, mod0, mod1, 0, s1);
		}

		public void SendKey(VKeys key, KeyboardMod mod0, KeyboardMod mod1, KeyboardMod mod2)
		{
			SendKey(key, mod0, mod1, mod2, GetS1());
		}

		public void SendKey(VKeys key, KeyboardMod mod0, KeyboardMod mod1, KeyboardMod mod2, IntPtr s1)
		{
			keybd_event((int)mod2, 0, 1, 0);
			keybd_event((int)mod1, 0, 1, 0);
			keybd_event((int)mod0, 0, 1, 0);
			PostMessage(s1, (int)WMessages.WM_KEYDOWN, (int)key, 1);
			PostMessage(s1, (int)WMessages.WM_KEYUP, (int)key, 1);
			keybd_event((int)mod2, 0, 3, 0);
			keybd_event((int)mod1, 0, 3, 0);
			keybd_event((int)mod0, 0, 3, 0);
		}

		public int ToLong(int LoWord, int HiWord)
		{
			return ((HiWord << 16) | (LoWord & 0xffff));
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern void keybd_event(int bVk, byte bScan, int dwFlags, int dwExtraInfo);

		[DllImport("user32.dll")]
		private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
	}
}