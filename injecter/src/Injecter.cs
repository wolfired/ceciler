using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Options;

namespace wolfired.com.injecter
{
    public class Injecter
    {
        public static void Inject(string[] args)
        {
            bool args_help = false;
            string dll_inject_to = null;
            string dll_hooker_at = null;
            string nameof_hooker = null;
            string method_name_b = null;
            string method_name_e = null;
            List<string> type_blacklist = new List<string>();
            List<string> type_whitelist = new List<string>();
            List<string> method_blacklist = new List<string>();
            List<string> method_whitelist = new List<string>();
            bool inject_getter = false;
            bool inject_setter = false;
            string finale_dll_suffix = ".ddd";
            var options = new OptionSet {
                { "h|help", "show this message and exit", h => args_help = h != null },
                { "t|dll_inject_to=", "被注入的DLL文件路径",  t => dll_inject_to = t },
                { "a|dll_hooker_at=", "钩子所在DLL文件路径",  a => dll_hooker_at = a },
                { "nameof_hooker=", "钩子完全类型名",  noh => nameof_hooker = noh },
                { "method_name_b=", "钩子方法名(前置)",  mnb => method_name_b = mnb },
                { "method_name_e=", "钩子方法名(后置)",  mne => method_name_e = mne },
                { "type_blacklist=", "被注入类型黑名单",  s => type_blacklist.Add(s) },
                { "type_whitelist=", "被注入类型白名单",  s => type_whitelist.Add(s) },
                { "method_blacklist=", "被注入方法黑名单",  s => method_blacklist.Add(s) },
                { "method_whitelist=", "被注入方法白名单",  s => method_whitelist.Add(s) },
                { "inject_getter", "要注入getter方法", s => inject_getter = s != null },
                { "inject_setter", "要注入setter方法", s => inject_setter = s != null },
                { "finale_dll_suffix=", "",  s => finale_dll_suffix = s },
            };
            List<string> extra;
            try
            {
                extra = options.Parse(args);
            }
            catch (Exception)
            {
                Console.WriteLine("Oops...参数解析错误\n\n");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            if (args_help)
            {
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            if (null == dll_inject_to || "" == dll_inject_to)
            {
                Console.WriteLine($"Oops...请指定被注入的DLL文件路径");
                return;
            }

            if (!File.Exists(dll_inject_to))
            {
                Console.WriteLine($"Oops...被注入的DLL文件 \"{dll_inject_to}\" 不存在");
                return;
            }

            if (null == dll_hooker_at || "" == dll_hooker_at)
            {
                Console.WriteLine($"Oops...请指定钩子所在DLL文件路径");
                return;
            }

            if (!File.Exists(dll_hooker_at))
            {
                Console.WriteLine($"Oops...钩子所在DLL文件路径 \"{dll_hooker_at}\" 不存在");
                return;
            }

            if (null == nameof_hooker || "" == nameof_hooker)
            {
                Console.WriteLine($"Oops...请指定钩子完全类型名");
                return;
            }

            if (null == method_name_b || "" == method_name_b)
            {
                Console.WriteLine($"Oops...钩子方法名(前置)");
                return;
            }

            if (null == method_name_e || "" == method_name_e)
            {
                Console.WriteLine($"Oops...钩子方法名(后置)");
                return;
            }

            var assembly_hooker_at = Assembly.LoadFrom(dll_hooker_at);
            var typeof_hooker = assembly_hooker_at.GetType(nameof_hooker);
            if (null == typeof_hooker)
            {
                Console.WriteLine($"Oops...钩子类型 \"{nameof_hooker}\" 不存在");
                return;
            }

            // var rp = new ReaderParameters(ReadingMode.Immediate);
            // rp.ReadWrite = true;
            // rp.InMemory = true;

            var assembly_inject_to = AssemblyDefinition.ReadAssembly(dll_inject_to);

            var types_inject_to = assembly_inject_to.MainModule.GetTypes();

            foreach (var type_inject_to in types_inject_to)
            {
                if (type_blacklist.Contains(type_inject_to.FullName) || nameof_hooker.Equals(type_inject_to.FullName))
                {
                    continue;
                }
                if (0 < type_whitelist.Count && !type_whitelist.Contains(type_inject_to.FullName))
                {
                    continue;
                }

                foreach (var method_inject_to in type_inject_to.Methods)
                {
                    if (method_inject_to.IsConstructor || method_inject_to.IsAbstract || method_inject_to.IsVirtual || (!inject_getter && method_inject_to.IsGetter) || (!inject_setter && method_inject_to.IsSetter) || method_inject_to.IsPInvokeImpl)
                    {
                        continue;
                    }

                    if (method_blacklist.Contains(method_inject_to.Name))
                    {
                        continue;
                    }

                    if (0 < method_whitelist.Count && !method_whitelist.Contains(method_inject_to.Name))
                    {
                        continue;
                    }

                    Console.WriteLine("Injecting to " + type_inject_to.Name + "." + method_inject_to.Name);
                    InjectMethod(method_inject_to, assembly_inject_to, typeof_hooker, method_name_b, new Type[] { typeof(string) }, typeof_hooker, method_name_e, new Type[] { typeof(string) });
                }
            }

            // var wp = new WriterParameters();
            // wp.WriteSymbols = true;

            assembly_inject_to.Write(dll_inject_to + finale_dll_suffix);
            Console.WriteLine("Finale DLL wrote to " + dll_inject_to + finale_dll_suffix);
        }

        private static void InjectMethod(Mono.Cecil.MethodDefinition method, AssemblyDefinition assembly, Type type_begin, string method_begin, Type[] types_begin, Type type_end, string method_end, Type[] types_end)
        {
            var firstIns = method.Body.Instructions.First();
            var lastIns = method.Body.Instructions.Last();
            var worker = method.Body.GetILProcessor();

            MethodReference hasPatchRefBegin = assembly.MainModule.ImportReference(type_begin.GetMethod(method_begin, types_begin));

            MethodReference hasPatchRefEnd = assembly.MainModule.ImportReference(type_end.GetMethod(method_end, types_end));

            var currentB = InsertBefore(worker, firstIns, worker.Create(OpCodes.Ldstr, method.FullName));
            currentB = InsertBefore(worker, firstIns, worker.Create(OpCodes.Call, hasPatchRefBegin));

            var currentE = InsertBefore(worker, lastIns, worker.Create(OpCodes.Ldstr, method.FullName));
            currentE = InsertBefore(worker, lastIns, worker.Create(OpCodes.Call, hasPatchRefEnd));

            ComputeOffsets(method.Body);
        }

        private static Instruction InsertBefore(ILProcessor worker, Instruction target, Instruction instruction)
        {
            worker.InsertBefore(target, instruction);
            return instruction;
        }

        private static Instruction InsertAfter(ILProcessor worker, Instruction target, Instruction instruction)
        {
            worker.InsertAfter(target, instruction);
            return instruction;

        }
        private static void ComputeOffsets(Mono.Cecil.Cil.MethodBody body)
        {
            var offset = 0;
            foreach (var instruction in body.Instructions)
            {
                instruction.Offset = offset;
                offset += instruction.GetSize();
            }
        }
    }
}
