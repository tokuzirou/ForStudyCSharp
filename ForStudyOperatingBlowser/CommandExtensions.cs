using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace ForStudyOperatingBlowser
{
    static class CommandExtensions
    {
        static RootCommand ConfigureFromMethod<T>(this RootCommand command, string methodName)
        {
            Command commandObject = registerHandler<T>(command, methodName);
            bool check = commandObject is RootCommand rootCommand;
            try
            {
                if (!check)
                    throw new Exception("型の不一致");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                rootCommand = commandObject as RootCommand;
            }
            return rootCommand;
        }

        static Command ConfigureFromMethod<T>(this Command command, string methodName)
        {
            return registerHandler<T>(command, methodName);
        }

        private static Command registerHandler<T>(Command command, string methodName)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(methodName, BindingFlags.Default);
            command.Handler = CommandHandler.Create(methodInfo);
            return command;
        }
    }
}
