namespace DrawingProgram;
using System;
using System.Runtime.InteropServices;

//Not even gonna try understanding this haha, thank you stackoverflow
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct OpenFileName
{
    public int lStructSize;
    public IntPtr hwndOwner;
    public IntPtr hInstance;
    public string lpstrFilter;
    public string lpstrCustomFilter;
    public int nMaxCustFilter;
    public int nFilterIndex;
    public string lpstrFile;
    public int nMaxFile;
    public string lpstrFileTitle;
    public int nMaxFileTitle;
    public string lpstrInitialDir;
    public string lpstrTitle;
    public int Flags;
    public short nFileOffset;
    public short nFileExtension;
    public string lpstrDefExt;
    public IntPtr lCustData;
    public IntPtr lpfnHook;
    public string lpTemplateName;
    public IntPtr pvReserved;
    public int dwReserved;
    public int flagsEx;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct BROWSEINFO
{
    public IntPtr hwndOwner;
    public IntPtr pidlRoot;
    public string pszDisplayName;
    public string lpszTitle;
    public uint ulFlags;
    public IntPtr lpfn;
    public IntPtr lParam;
    public int iImage;
}

public class OpenDialog
{
    [DllImport("comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool GetOpenFileName(ref OpenFileName ofn);

    public static string GetFile()
    {
        var ofn = new OpenFileName();
        ofn.lStructSize = Marshal.SizeOf(ofn);
        ofn.lpstrFilter = "PNG files (*.png)\0*.png";
        ofn.lpstrFile = new string(new char[256]);
        ofn.nMaxFile = ofn.lpstrFile.Length;
        ofn.lpstrFileTitle = new string(new char[64]);
        ofn.nMaxFileTitle = ofn.lpstrFileTitle.Length;
        ofn.lpstrTitle = "Open File";
        if (GetOpenFileName(ref ofn))
            return ofn.lpstrFile;
        return string.Empty;
    }

    [DllImport("shell32.dll")]
    private static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

    public static string GetDirectory()
    {
        var folderInfo = new BROWSEINFO
        {
            hwndOwner = IntPtr.Zero,
            pidlRoot = IntPtr.Zero,
            pszDisplayName = new string('\0', 260),
            ulFlags = 0x00000001 // BIF_RETURNONLYFSDIRS flag
        };

        IntPtr pidl = SHBrowseForFolder(ref folderInfo);

        if (pidl != IntPtr.Zero)
        {
            IntPtr pszPath = Marshal.AllocHGlobal(260);
            if (SHGetPathFromIDList(pidl, pszPath))
            {
                string selectedPath = Marshal.PtrToStringAuto(pszPath);
                Marshal.FreeCoTaskMem(pidl);
                Marshal.FreeHGlobal(pszPath);
                return selectedPath;
            }
        }

        return string.Empty;
    }
}
