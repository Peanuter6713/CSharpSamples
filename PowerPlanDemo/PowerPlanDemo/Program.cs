using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlanDemo
{
    public enum AccessFlags : uint
    {
        ACCESS_SCHEME = 16,
        ACCESS_SUBGROUP = 17,
        ACCESS_INDIVIDUAL_SETTING = 18
    }

    internal class Program
    {
        [DllImport("PowrProf.dll")]
        public static extern uint PowerGetActiveScheme(IntPtr UserRootPowerKey, ref IntPtr activePowerId);
        [DllImport("PowrProf.dll")]
        public static extern uint PowerEnumerate(IntPtr RootPowerKey, IntPtr SchemeGuid, IntPtr SubGroupOfPowerSettingGuid, uint AcessFlags, uint Index, ref Guid Buffer, ref uint BufferSize);


        static void Main(string[] args)
        {
            GetPowerScheme();

            Console.ReadLine();
        }

        static void GetPowerScheme()
        {
            IntPtr activePowerId = IntPtr.Zero;

            var result = PowerGetActiveScheme(IntPtr.Zero, ref activePowerId);
            var activePowerSchemeId = Marshal.PtrToStructure(activePowerId, typeof(Guid));

            Console.WriteLine("result = {0}, Id = {1}, Guid = {2}", result, activePowerId, activePowerSchemeId);

            var schemes = GetAllPowerSchemes().ToList();

            schemes.ForEach((scheme) =>
            {
                Console.WriteLine("scheme = {0}", scheme);
            });

        }

        static IEnumerable<Guid> GetAllPowerSchemes()
        {
            var schemeGuid = Guid.Empty;

            uint sizeSchemeGuid = (uint)Marshal.SizeOf(typeof(Guid));
            uint schemeIndex = 0;

            while (PowerEnumerate(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, (uint)AccessFlags.ACCESS_SCHEME, schemeIndex, ref schemeGuid, ref sizeSchemeGuid) == 0)
            {
                yield return schemeGuid;
                schemeIndex++;
            }
        }

    }
}
