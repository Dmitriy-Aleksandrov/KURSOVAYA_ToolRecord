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
    }
    public interface IToolObserver
    {
        void OnToolAdded(Instrument tool);
        void OnToolDeleted(Instrument tool);
        void OnToolUpdated(Instrument tool);
    }

    // Логгер как наблюдатель
    public class ToolLogger : IToolObserver
    {
        public void OnToolAdded(Instrument tool)
        {
            File.AppendAllText("log.txt", $"[{DateTime.Now}] Добавлен: {tool.Name}\n");
        }

        public void OnToolDeleted(Instrument tool)
        {
            File.AppendAllText("log.txt", $"[{DateTime.Now}] Удален: {tool.Name}\n");
        }

        public void OnToolUpdated(Instrument tool) { /* ... */ }
    

// В MainViewModel
        private List<IToolObserver> _observers = new List<IToolObserver>();

        public void AddObserver(IToolObserver observer) => _observers.Add(observer);

        private void NotifyToolAdded(Instrument tool)
        {
            foreach (var observer in _observers)
                observer.OnToolAdded(tool);
        }

        private void AddTool()
        {
            var tool = new Instrument { /*...*/ };
            NotifyToolAdded(tool); // Уведомляем всех наблюдателей
        }
    }
}
