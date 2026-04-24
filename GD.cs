using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_WPF_InstrumentControl.Model;

namespace APP_WPF_InstrumentControl
{
    class GD
    {
        public static int CompID;
        public static int AddrID;
        public static string CompName;
        public static string CompINN;
    }
    
    public class ToolLogger 
    {
        public static void OnToolAdded(Instrument tool)
        {
            File.AppendAllText("log.txt", $"[{DateTime.Now}] Добавлен: {tool.Id}; {tool.FirmId}; {tool.AddressId}; {tool.Name}; {tool.Category}; {tool.Type}; {tool.Index};\n");
        }

        public static void OnToolDeleted(Instrument tool)
        {
            File.AppendAllText("log.txt", $"[{DateTime.Now}] Удален: {tool.Id}; {tool.FirmId}; {tool.AddressId}; {tool.Name}; {tool.Category}; {tool.Type}; {tool.Index};\n");
        }

        public static void OnToolUpdated(Instrument tool1, Instrument tool2)
        {
            File.AppendAllText("log.txt", $"[{DateTime.Now}] Объект: {tool1.Id}; {tool1.FirmId}; {tool1.AddressId}; {tool1.Name}; {tool1.Category}; {tool1.Type}; {tool1.Index}; изменен на: {tool2.Id}; {tool2.FirmId}; {tool2.AddressId}; {tool2.Name}; {tool2.Category}; {tool2.Type}; {tool2.Index};\n");
        }
    
        
    }
}
