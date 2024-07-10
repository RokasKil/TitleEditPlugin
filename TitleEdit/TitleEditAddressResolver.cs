using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Graphics.Environment;
using System;
using System.Runtime.InteropServices;

namespace TitleEdit
{
    public static class TitleEditAddressResolver
    {
        private static unsafe IntPtr CameraBase => (IntPtr)CameraManager.Instance();

        public static unsafe Camera* RenderCamera => CameraManager.Instance()->Camera;

        public static IntPtr LobbyCamera => CameraBase == IntPtr.Zero ? IntPtr.Zero : Marshal.ReadIntPtr(CameraBase, 16);

        public static unsafe IntPtr WeatherPtr => ((IntPtr)EnvManager.Instance()) + 0x27;

        public static IntPtr LoadLogoResource { get; private set; }
        public static IntPtr SetTime { get; private set; }
        public static IntPtr CreateScene { get; private set; }
        public static IntPtr FixOn { get; private set; }
        public static IntPtr PlayMusic { get; private set; }
        public static IntPtr BgmControl { get; private set; }
        public static IntPtr LobbyThing { get; private set; }
        public static IntPtr LoadLobbyScene { get; private set; }
        public static IntPtr PlayMovie { get; private set; }

        public static unsafe void Setup64Bit()
        {
            LoadLogoResource = DalamudApi.SigScanner.ScanText("48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC ?? 41 8B F1 41 0F B6 F8");
            SetTime = DalamudApi.SigScanner.ScanText("40 53 48 83 EC ?? 44 0F BF C1");
            CreateScene = DalamudApi.SigScanner.ScanText("E8 ?? ?? ?? ?? 66 89 1D ?? ?? ?? ?? E9 ?? ?? ?? ??");
            FixOn = DalamudApi.SigScanner.ScanText("C6 81 ?? ?? ?? ?? ?? 8B 02 89 41 60");
            PlayMusic = DalamudApi.SigScanner.ScanText("E8 ?? ?? ?? ?? 48 89 47 18 89 5F 20");
            BgmControl = DalamudApi.SigScanner.GetStaticAddressFromSig("48 8B 1D ?? ?? ?? ?? 8B F1");
            LobbyThing = DalamudApi.SigScanner.GetStaticAddressFromSig("66 0F 7F 05 ?? ?? ?? ?? 4C 89 35");
            LoadLobbyScene = DalamudApi.SigScanner.ScanText("E8 ?? ?? ?? ?? B0 ?? 88 86");
            PlayMovie = DalamudApi.SigScanner.ScanText("E8 ?? ?? ?? ?? 48 8B 7C 24 ?? 48 89 43 ?? C7 43");
        }

        public static TitleScreenExpansion CurrentTitleScreenType
        {
            get => (TitleScreenExpansion)Marshal.ReadInt32(LobbyThing, 0x34);
            set => Marshal.WriteInt32(LobbyThing, 0x34, (int)value);
        }

    }
}