using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;

namespace Scan_log_lib
{
    //Интерфейс IMyClass для класс MyClass
    [Guid("7213FA50-F0D5-4030-9274-29C3DA5EE3DC")]
    public interface IMyClass
    {        
        [DispId(1)]
        string FindString(string file);             
        [DispId(2)]
        string Help { get; }        
        [DispId(3)]
        string RegexString { get; set; }        
        [DispId(4)]
        string Position { get; set; }
        [DispId(5)]
        string GetPositionEnd(string file);
    }

    //Интерфейс для событий 
    [Guid("A3210251-3879-4EE8-B94E-3266A6934CC2"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMyEvents    
    { 
    }

    [Guid("E43C7DB7-C22E-45E0-BE1E-67B9CB733138"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMyEvents))]
    [ComVisible(true)]
    //regasm Scan_log_lib.dll /tlb:Scan_log_lib.tlb регистрация описания открытых типов
    //Класс MyClass реализует интерфейс IMyClass
    public class Scanloglib : IMyClass
    {
        //field
        private string regStr = string.Empty; 
        private long Iblock = 0;
        private long indLine = 1;

        private Match match;
        public Scanloglib() { }

        //property
        public string Help// Справочник
        {
            get
            {
                return "************************************\r\n"+
                       "RegexString(свойство {get,set}) - искомое выражение\r\n" +
                       "Position(свойство {get,set}) - ID блока и номер строки в формате - 1|2\r\n" +
                       "GetPositionEnd(string file) - метод возвращает последнюю позицию в файле в формате - 1|2\r\n" +
                       "FindString(string file) - метод поиска выражений. Возвращает найденную строку. Может использовать свойство Position.\r\n" +
                       "************************************";
            }
        }
        public string RegexString// Регулярное выражение
        {
            get
            {
                return regStr;
            }
            set
            {
                regStr = value;
            }
        }
        public string Position// Позиция в файле
        {
            get
            {
                return Iblock.ToString() + "|" + indLine.ToString();
            }
            set
            {
                
                string[] sp = value.Split('|');
                if (sp.Length < 2)
                {
                    Iblock = 0;
                    indLine = 1;
                }
                else
                {
                    Iblock = Convert.ToInt64(sp[0]);
                    indLine = Convert.ToInt64(sp[1]);
                }
            }
        }

        public string GetPositionEnd(string fScan)
        {
            using (FileStream fs = new FileStream(fScan, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                Iblock = fs.Seek(0, SeekOrigin.End) / 1024;
                return Position;
            }
        }

        public string FindString(string fScan)
        {
            Regex reg = new Regex(RegexString);
            string Curline = string.Empty;
            int CurIndLineSuccess = 0;
            int CurIndBlockSuccess = 0;
            using (FileStream fs = new FileStream(fScan, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader StrRead = new StreamReader(fs))
                {  
                    while ((Curline = StrRead.ReadLine()) != null)
                    {
                        match = reg.Match(Curline);
                        if (match.Success)
                        {
                            if (StrRead.BaseStream.Position <  1024)
                            {
                                CurIndBlockSuccess = 1;
                            }
                            else
                            {
                                CurIndBlockSuccess = (int)StrRead.BaseStream.Position / 1024;
                            }
                            CurIndLineSuccess++;

                            if (Iblock == CurIndBlockSuccess)
                            {
                                if (indLine == CurIndLineSuccess)
                                {
                                    if (StrRead.BaseStream.Position >= 1024)
                                        Iblock = StrRead.BaseStream.Position /  1024;
                                    indLine = CurIndLineSuccess + 1;                                   
                                    return Curline;
                                }
                            }
                            else if (Iblock < CurIndBlockSuccess)
                            {
                                Iblock = StrRead.BaseStream.Position /  1024;
                                indLine = CurIndLineSuccess + 1;                                
                                return Curline;
                            }
                        }
                        else
                        {
                            if (Iblock != (StrRead.BaseStream.Position /  1024))
                            {
                                CurIndLineSuccess = 0;
                            }
                        }
                    }
                    StrRead.Dispose();
                }
                fs.Dispose();
            }
            return "";
        }            
    }   
}