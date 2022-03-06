#ifndef LIB_H
#define LIB_H
#include<windows.h>
#include<string>
#include<shlobj.h>
#include<sstream>
#include<vector>
#include <fstream>
using namespace std;

#ifdef __cplusplus
    extern "C" {
#endif

#ifdef BUILD_MY_DLL
    #define LIB __declspec(dllexport)
#else
    #define LIB __declspec(dllimport)

#endif

BOOL LIB IsAppRunningAsAdminMode();
void LIB RunAsAdmin(const char* appName);
const char* LIB setPath();
string narrow(wstring const& text);
int LIB  getFiles(char* path,char** fileName,char** filePath,char** fileSize,char** fileDate);
wchar_t *convertCharArrayToLPCWSTR(const char* charArray);
string delLast(string path);
void LIB OpenManagerFile(const char* fileName);
bool LIB startCheckUpdate(char* path);
void LIB stopChange(char* path);
#ifdef __cplusplus
    }
#endif

#endif //end of dll
