using System.Runtime.InteropServices;
using HarmonyLib;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System;

namespace GTUnixSDKPatch 
{
    [InitializeOnLoad]
    public static class HarmonySetup 
    {
        static Harmony Harmony;
        static HarmonySetup() 
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

            Harmony = new Harmony("luna.gtunixsdkpatch");
            Harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(GT_CustomMapSupportEditor.MapExporter))]
    [HarmonyPatch("PackageMaps")]
    public static class PackagePatch 
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = new List<CodeInstruction>(instructions);
            var toLowerMethod = AccessTools.Method(typeof(string), nameof(string.ToLower), Type.EmptyTypes);

            for (int i = 0; i < code.Count; i++)
            {
                var instr = code[i];
                yield return instr;
                if (instr.opcode == OpCodes.Call &&
                    instr.operand is MethodInfo method &&
                    method.DeclaringType == typeof(string) &&
                    method.Name == "Concat")
                {
                    if (i >= 1 && code[i - 1].opcode == OpCodes.Ldstr)
                    {
                        var str = code[i - 1].operand as string;
                        if (str == "_Win64" || str == "_Android") yield return new CodeInstruction(OpCodes.Callvirt, toLowerMethod);
                    }
                }
            }
        }
    }
}
