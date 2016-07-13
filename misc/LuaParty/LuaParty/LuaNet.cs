using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NLua;

namespace LuaParty
{
    public interface IOutput
    {
        void WriteOut(string value);
    }

    class LuaNet
    {
        Lua state = new Lua();
        Dictionary<string, LuaFunction> _functions = new Dictionary<string, LuaFunction>();

        IOutput output;

        public LuaNet()
        {
            state["nobj"] = this;
            Ticks = 0;
        }

        public void ShowText(string text)
        {
            MessageBox.Show(text);
        }

        public void HookOutput(IOutput output)
        {
            this.output = output;
        }

        public void WriteOut(string s)
        {
            output.WriteOut(s);
        }

        public object[] Call(string functionName, params object[] values)
        {
            LuaFunction function = null;

            if (_functions.ContainsKey(functionName))
            {
                function = _functions[functionName];
            }
            else
            {
                function = state[functionName] as LuaFunction;
                if (function != null)
                {
                    _functions.Add(functionName, function);
                }
            }

            if (function != null)
            {
                if (values == null)
                    return function.Call();
                else
                    return function.Call(values);
            }

            return null;
        }

        public void SetScript(string script)
        {            
            state.DoString(script);         
        }

        public int Ticks { get; set; }

        public event EventHandler StopAction;
        public void Stop()
        {
            var s = StopAction;
            if (s != null)
            {
                s(this, new EventArgs());
            }
        }
    }
}
