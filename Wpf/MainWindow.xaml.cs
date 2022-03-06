using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;
namespace WpfApp5{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window{
      //  string checkedPath;
       // System.Threading.Thread MyThread1;
        bool t = false;
        public bool _changeDIr = false;
        //int countStart = 0;
        public static string AppName = AppDomain.CurrentDomain.FriendlyName;
        static Mutex InstanceCheckMutex;
        [DllImport("lib_app.dll")]
        private static extern bool IsAppRunningAsAdminMode();
        [DllImport("lib_app.dll")]
        private static extern bool startCheckUpdate(string path);
        [DllImport("lib_app.dll")]
        private static extern void RunAsAdmin(string appName);
        [DllImport("lib_app.dll")]
        private static extern void OpenManagerFile(string fileName);
        [DllImport("lib_app.dll", CharSet = CharSet.Ansi)]
        private static extern void stopChange(string path);
        [DllImport("lib_app.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr setPath();
        [DllImport("lib_app.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int getFiles(string path,IntPtr[] fileName, IntPtr[] filePath, IntPtr[] fileSize, IntPtr[] fileDate);
        public MainWindow()
        {
            if (!InstanceCheck()){
                // нажаловаться пользователю и завершить процесс
                System.Windows.Application.Current.Shutdown();
            }
            else {
               
                InitializeComponent();
                TextB.IsReadOnly = true;
                TextB.TextWrapping = TextWrapping.NoWrap;
                UContrl.getMain(this);

                // TextB.Text = checkStatus().ToString();
                if (checkStatus()){
                    Butt_Admin.IsEnabled = false;
                }
            }
         

        }

      
      
        
        public void UseOpenManager(string fileName)
        {
            OpenManagerFile(fileName);
        }
        public void changeText(string text)
        {
            TextB.Text = text;
        }
      

       /*
        public void Polls(string s)
        {
            bool t;


            while (_enabled)
            {
                t= startCheckUpdate(s);
                if (t)
                {

                   
                   
                    MessageBox.Show(s);
                }
                t = false;

            }

           
            

           
            MyThread1 = new System.Threading.Thread(delegate () { Polls(s); });
            MyThread1.IsBackground = true;
            MyThread1.Start();
           
        }

     */
        /*
        BackPath p = new BackPath();
        public async void Poll(TextBox text)
        {
           
           
            while (true)
            {
                p = UContrl.GetBack();
                if (p.isClickBack == true)
                {
                   
                    text.Text = p.path;
                    p.isClickBack = false;
                }
                IntPtr[] first = createForCppStrinPTR();
                IntPtr[] second = createForCppStrinPTR();
                IntPtr[] third = createForCppStrinPTR();
                IntPtr[] fourth = createForCppStrinPTR();
                int m;

                m = getFiles(text.Text, first, second, third, fourth);
                string[] fileName = IntPtrToStr(first, m);
                string[] filePath = IntPtrToStr(second, m);
                string[] fileSize = IntPtrToStr(third, m);
                string[] fileDate = IntPtrToStr(fourth, m);
                int cnt = 0;
                UContrl.DataClear();
                while (cnt < m)
                {
                    UContrl.SetData(cnt, fileName[cnt], filePath[cnt], fileSize[cnt], fileDate[cnt]);
                    cnt++;
                }
                await Task.Delay(TimeSpan.FromMilliseconds(1000));
            }
        }
*/
        private IntPtr[] createForCppStrinPTR()
        {
            var strArray = new byte[500][];
            for (int i = 0; i < strArray.Length; i++)
            {

                strArray[i] = new byte[256];
            }
            var handles = new GCHandle[strArray.Length];
            var ptrs = new IntPtr[strArray.Length];

            for (int i = 0; i < strArray.Length; i++)
            {
                handles[i] = GCHandle.Alloc(strArray[i], GCHandleType.Pinned);
                ptrs[i] = handles[i].AddrOfPinnedObject();
            }
            for (int i = 0; i < strArray.Length; i++)
            {
                if (handles[i].IsAllocated)
                {
                    handles[i].Free();
                }
            }
            return ptrs;

        }
        private string[] IntPtrToStr(IntPtr[] ptrs, int lenArr)
        {
            string[] strings;
            strings = new string[lenArr];

            for (int i = 0; i < lenArr; i++)
            {
                strings[i] = Marshal.PtrToStringAnsi(ptrs[i]);
            }
            return strings;
        }

        private bool checkStatus(){
            return IsAppRunningAsAdminMode();

        }
        void DataWindow_Closing(object sender, CancelEventArgs e) {
            //MessageBox.Show("Closing called");
            //MyThread1.Abort();
            System.Windows.Application.Current.Shutdown();
            
        }

        private void Butt_Admin_Click(object sender, RoutedEventArgs e){
            RunAsAdmin(AppName);
            System.Windows.Application.Current.Shutdown();
            
        }
        
        static bool InstanceCheck(){
            bool isNew;
            InstanceCheckMutex = new Mutex(true, AppName, out isNew);
            return isNew;
        }
        
    

        public void BackPath(string path)
        {
          //  stopChange(checkedPath);
           // MyThread1.Abort();
            //_changeDIr = true;
           // startThread(path);
            
            TextB.Text = path;
            
            IntPtr[] first = createForCppStrinPTR();
            IntPtr[] second = createForCppStrinPTR();
            IntPtr[] third = createForCppStrinPTR();
            IntPtr[] fourth = createForCppStrinPTR();
            int m;

            m = getFiles(path, first, second, third, fourth);
            string[] fileName = IntPtrToStr(first, m);
            string[] filePath = IntPtrToStr(second, m);
            string[] fileSize = IntPtrToStr(third, m);
            string[] fileDate = IntPtrToStr(fourth, m);
            int cnt = 0;
            UContrl.DataClear();
            while (cnt < m)
            {
               
                UContrl.SetData(cnt, fileName[cnt], filePath[cnt], fileSize[cnt], fileDate[cnt]);
                cnt++;
            }
           
            //MyThread1 = new System.Threading.Thread(delegate () { Polls(path); });
            //_enabled = true;
           // MyThread1.Start();
        }

        private void Button_Path_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (countStart > 0)
            {
                _changeDIr = true;
                stopChange(checkedPath);
            }
            */
            //Poll(TextB);
            Butt_Folders.IsEnabled = false;
            IntPtr k = setPath();
            
            string path = Marshal.PtrToStringAnsi(k);
            //startThread(path);
            TextB.Text = path;
           
            IntPtr[] first = createForCppStrinPTR();
            IntPtr[] second = createForCppStrinPTR();
            IntPtr[] third = createForCppStrinPTR();
            IntPtr[] fourth = createForCppStrinPTR();
            int m;

            m = getFiles(path,first, second, third, fourth);
            string[] fileName = IntPtrToStr(first, m);
            string[] filePath = IntPtrToStr(second, m);
            string[] fileSize = IntPtrToStr(third, m);
            string[] fileDate = IntPtrToStr(fourth, m);
            int cnt = 0;
            UContrl.DataClear();
            while (cnt < m)
            {
                
                UContrl.SetData(cnt,fileName[cnt],filePath[cnt],fileSize[cnt],fileDate[cnt]);
                cnt++;
            }
           //countStart++;
            Butt_Folders.IsEnabled = true;
            
            //MyThread1 = new System.Threading.Thread(delegate () { Polls(path); });
            //enabled = true;
            // MyThread1.Start();
        }
    }
}

/**************************************************MULTITHREADING**************************************************************
 
     public  void check(string path)
        {
            checkedPath = path;
            t = startCheckUpdate(path);
           
            while (!t)
            {
                

            }
            MessageBox.Show("CHANGE " + _changeDIr);
            if (_changeDIr)
            {
                t = false;
                MessageBox.Show("THIS PATH CHANGE DIR " + path);
                MyThread1.Abort();
                return;
            }
            //MessageBox.Show("THIS PATH JUST CHANGE"+path);
            t = false;
            Dispatcher.Invoke((Action)(() =>
            {
                TextB.Text = path;
            }));
            
            IntPtr[] first = createForCppStrinPTR();
            IntPtr[] second = createForCppStrinPTR();
            IntPtr[] third = createForCppStrinPTR();
            IntPtr[] fourth = createForCppStrinPTR();
            int m;
            
            m = getFiles(path, first, second, third, fourth);
            
            string[] fileName = IntPtrToStr(first, m);
            string[] filePath = IntPtrToStr(second, m);
            string[] fileSize = IntPtrToStr(third, m);
            string[] fileDate = IntPtrToStr(fourth, m);
            int cnt = 0;
            Dispatcher.Invoke((Action)(() =>
            {
                UContrl.DataClear();
            }));
            
            while (cnt < m)
            {
                Dispatcher.Invoke((Action)(() =>
                {
                    UContrl.SetData(cnt, fileName[cnt], filePath[cnt], fileSize[cnt], fileDate[cnt]);
                }));
                
                cnt++;
            }
            startThread(path);
        }


      void startThread(string path)
        {

            MyThread1 = new System.Threading.Thread(delegate () { check(path); });
            MyThread1.IsBackground = true;
            MyThread1.Start();

        }
     
*/
