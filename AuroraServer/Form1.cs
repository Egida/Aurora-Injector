﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AuroraServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Process[] PC = Process.GetProcesses().Where(p => (long)p.MainWindowHandle != 0).ToArray();
            comboBox1.Items.Clear();
            foreach (Process p in PC)
            {
                comboBox1.Items.Add(p.ProcessName);
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            this.Capture = false;
            var msg = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref msg);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private static string DLLP { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.InitialDirectory = @"C:\";
                OFD.Title = "Locate DLL File for Injection";
                OFD.DefaultExt = "dll";
                OFD.Filter = "DLL Files (*.dll)|*.dll";
                OFD.CheckFileExists = true;
                OFD.CheckPathExists = true;
                OFD.ShowDialog();


                textBox1.Text = OFD.FileName;
                DLLP = OFD.FileName;
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DLLP = textBox1.Text;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process[] PC = Process.GetProcesses().Where(p => (long)p.MainWindowHandle != 0).ToArray();
            comboBox1.Items.Clear();
            foreach (Process p in PC)
            {
                comboBox1.Items.Add(p.ProcessName);
            }
        }



        static readonly IntPtr INTPTR_ZERO = (IntPtr)0;
        [DllImport("kernel32.dll", SetLastError = true)]

        static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true)]

        static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll", SetLastError = true)]

        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        [DllImport("kernel32.dll", SetLastError = true)]


        static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll", SetLastError = true)]

        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]

        static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", SetLastError = true)]

        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreatationFlags, IntPtr lpThreadId);


        public static int Inject( string PN, string DLLP)
        {
            // 1 = file does not exists

            //2 = process not active
            if (!File.Exists(DLLP)) { return 1; }

            uint _procId = 0;
            Process[] _procs = Process.GetProcesses();
            for (int i = 0; i < _procs.Length; i++)
            {
                if (_procs[i].ProcessName == PN)
                {
                    _procId = (uint)_procs[i].Id;
                }


            }

            if (_procId == 0) { return 2; }

            if (!SI(_procId, DLLP))
            {
                return 3;
            }

            return 4;




        }

        public static bool SI(uint P, string DDLP)
        {
            IntPtr hndProc = OpenProcess((0x2 | 0x8 | 0x10 | 0x20 | 0x400), 1, P);

            if (hndProc == INTPTR_ZERO) { return false; }

            IntPtr lpAddress = VirtualAllocEx(hndProc, (IntPtr)null, (IntPtr)DLLP.Length, (0x1000 | 0x2000), 0x40);
            
            if (lpAddress == INTPTR_ZERO)
            {
                return false;
            }

            byte[] bytes = Encoding.ASCII.GetBytes(DLLP);

            if (WriteProcessMemory(hndProc, lpAddress, bytes, (uint)bytes.Length, 0) == 0)
            {
                return false;
            }

            CloseHandle(hndProc);

            return true;


        }

        private void button3_Click(object sender, EventArgs e)
        {
            int Result = Inject(comboBox1.Text, DLLP);

            if (Result == 1)
            {
                MessageBox.Show("File does not exists!");
            }
            else if (Result == 2)
            {
                MessageBox.Show("Process Does not Exists!");
            }

            else if (Result == 3)
            {
                MessageBox.Show("Injection Fails!");
            }

            else if (Result == 4)
            {
                MessageBox.Show("Injection Successfull!");
            }

        }
    }
}
