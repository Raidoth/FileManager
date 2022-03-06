#include "lib_app.h"
BOOL IsAppRunningAsAdminMode()
{
	BOOL fIsRunAsAdmin = FALSE;
	DWORD dwError = ERROR_SUCCESS;
	PSID pAdministratorsGroup = NULL;



	SID_IDENTIFIER_AUTHORITY NtAuthority = SECURITY_NT_AUTHORITY;
	if (!AllocateAndInitializeSid(
		&NtAuthority,
		2,
		SECURITY_BUILTIN_DOMAIN_RID,
		DOMAIN_ALIAS_RID_ADMINS,
		0, 0, 0, 0, 0, 0,
		&pAdministratorsGroup))
	{
		dwError = GetLastError();
	}

	

	if (!CheckTokenMembership(NULL, pAdministratorsGroup, &fIsRunAsAdmin))
	{
		dwError = GetLastError();
	
	}

	

	if (pAdministratorsGroup)
	{
		FreeSid(pAdministratorsGroup);
		pAdministratorsGroup = NULL;
	}

	

	if (ERROR_SUCCESS != dwError)
	{
		throw dwError;
	}

	return fIsRunAsAdmin;
}
void RunAsAdmin(const char* appName) {

	ShellExecute(NULL, "runas", appName, NULL, NULL, 0);

}
void OpenManagerFile(const char* fileName){
    ShellExecute(NULL, "open", fileName, NULL, NULL, 0);
}

const char* setPath() {
        setlocale(LC_ALL, "");
		string s;
		BROWSEINFO bi;
		char FolderName[MAX_PATH];
		LPITEMIDLIST item;
		memset(&bi, 0, sizeof(BROWSEINFO));
		bi.hwndOwner = NULL;
		bi.pszDisplayName = FolderName;
		bi.ulFlags = BIF_USENEWUI | BIF_RETURNONLYFSDIRS;
		item = SHBrowseForFolder(&bi);
        if(item){
            char *_path = (char*)malloc(MAX_PATH * sizeof(char));
			SHGetPathFromIDList(item, _path);
           s=_path;
           free(_path);
		}
		else {
			s = "No file selected ";
		}
     
		return s.c_str();
	}
string narrow(wstring const& text)
{
    locale const loc("");
    wchar_t const* from = text.c_str();
    size_t const len = text.size();
    vector<char> buffer(len + 1);
    use_facet<ctype<wchar_t> >(loc).narrow(from, from + len, '_', &buffer[0]);
    return string(&buffer[0], &buffer[len]);
}

wchar_t *convertCharArrayToLPCWSTR(const char* charArray)
{
    wchar_t* wString=new wchar_t[4096];
    MultiByteToWideChar(CP_ACP, 0, charArray, -1, wString, 4096);
    return wString;
}

string delLast(string path){

string tmp;
int n = 0;
int counter = 0;
int lenP=path.length();
int cntSlesh =0;
while(n<lenP){
        if(path[n]=='\\'){
            counter=0;
            cntSlesh++;
        }
    tmp+=path[n];
    counter++;
    n++;
}
int ans = lenP-1-counter;

for(int i = lenP-1;i>ans;i--){
    path[i]='\0';
}
return path;
}

int getFiles(char* path, char** fileName,char** filePath,char** fileSize,char** fileDate){
            setlocale(LC_ALL, "");
            string result;
            result = path;
            if(result !="No file selected"){
                result+="\\*";
            }else {
                return 0;
            }
            wchar_t* tmp = convertCharArrayToLPCWSTR(result.c_str());
            WIN32_FIND_DATAW wfd;
            HANDLE const hFind = FindFirstFileW(tmp, &wfd);
            SYSTEMTIME stUTC;
            int cnt = 0;
            
            if (INVALID_HANDLE_VALUE != hFind)
            {
                do
                {
                   
                    if(wfd.dwFileAttributes&FILE_ATTRIBUTE_DIRECTORY){

                        //wcout  << wfd.cFileName  <<" folder"<<endl;
                        string conv = narrow(wfd.cFileName);
                        if(conv=="."){
                            continue;
                        }
                        strcpy(fileName[cnt],conv.c_str());
                        
                        if(conv==".."){
                            int tmpLen = result.length();
                            result[tmpLen-1]='\0';
                            result[tmpLen-2]='\0';
                            conv =  delLast(result);
                          
                        }else{
                        string tmp=conv;
                        conv = path;
                        conv +="\\";
                        conv+=tmp;
                        }
                        strcpy(filePath[cnt],conv.c_str());
                        conv = "Folder";
                        strcpy(fileSize[cnt],conv.c_str());
                        cnt++;
                        continue;
                    }
                   
                    FileTimeToSystemTime(&wfd.ftLastWriteTime, &stUTC);
                    char time[20];
                    memset(time,0,20);
                    
                     stringstream ss;
                    //wcout  << wfd.cAlternateFileName  <<" "<<stUTC.wYear<<endl;
                        string conv = narrow(wfd.cFileName);
                        strcpy(fileName[cnt],conv.c_str());
                        string tmp=conv;
                        conv = path;
                        conv +="\\";
                        conv+=tmp;
                        strcpy(filePath[cnt],conv.c_str());
                        ss<<wfd.nFileSizeLow;
                        conv = ss.str();
                        strcpy(fileSize[cnt],conv.c_str());
                        conv = to_string(stUTC.wDay);
                        if(conv.length()==1){
                            string tmp = conv;
                            conv ="0";
                            conv+=tmp;
                        }
                        strcat(time,conv.c_str());
                        strcat(time,".");
                        conv = to_string(stUTC.wMonth);
                         if(conv.length()==1){
                            string tmp = conv;
                            conv ="0";
                            conv+=tmp;
                        }
                        strcat(time,conv.c_str());
                        strcat(time,".");
                        conv = to_string(stUTC.wYear);
                        strcat(time,conv.c_str());
                        strcat(time," ");
                        conv = to_string(stUTC.wHour);
                        if(conv.length()==1){
                            string tmp = conv;
                            conv ="0";
                            conv+=tmp;
                        }
                        strcat(time,conv.c_str());
                        strcat(time,":");
                        conv = to_string(stUTC.wMinute);
                         if(conv.length()==1){
                            string tmp = conv;
                            conv ="0";
                            conv+=tmp;
                        }
                        strcat(time,conv.c_str());
                        strcpy(fileDate[cnt],time);
                    cnt++;

              } while (NULL != FindNextFileW(hFind, &wfd));

                FindClose(hFind);
            }
            int tmpSort = 0;
          
return cnt;

}
bool startCheckUpdate(char* path){
    char* dir = path;
    HANDLE hDir = CreateFile(
        dir,                                // pointer to the file name
        FILE_LIST_DIRECTORY,                // access (read/write) mode
        // Share mode MUST be the following to avoid problems with renames via Explorer!
        FILE_SHARE_DELETE | FILE_SHARE_READ | FILE_SHARE_WRITE, // share mode
        NULL,                               // security descriptor
        OPEN_EXISTING,                      // how to create
        FILE_FLAG_BACKUP_SEMANTICS,         // file attributes
        NULL                                // file with attributes to copy
    );
    TCHAR szBuffer[1024 * 128];
    DWORD BytesReturned;
    if (ReadDirectoryChangesW(
        hDir,                          // handle to directory
        &szBuffer,                       // read results buffer
        sizeof(szBuffer),                // length of buffer
        FALSE,                          // monitoring option
        FILE_NOTIFY_CHANGE_SECURITY |
        FILE_NOTIFY_CHANGE_CREATION |
        FILE_NOTIFY_CHANGE_LAST_WRITE |
        FILE_NOTIFY_CHANGE_SIZE |
        FILE_NOTIFY_CHANGE_ATTRIBUTES |
        FILE_NOTIFY_CHANGE_DIR_NAME |
        FILE_NOTIFY_CHANGE_FILE_NAME,  // filter conditions
        &BytesReturned,                // bytes returned
        NULL,                          // overlapped buffer
        NULL                           // completion routine
        )
    ) {

        return true;
    }
}
void stopChange(char* path) {
    char* dir = path;
    strcpy(dir,"\\tmp.txt");
	HANDLE hDir = CreateFile(
		dir,                                // pointer to the file name
		FILE_ALL_ACCESS,                // access (read/write) mode
		// Share mode MUST be the following to avoid problems with renames via Explorer!
		FILE_SHARE_DELETE | FILE_SHARE_READ | FILE_SHARE_WRITE, // share mode
		NULL,                               // security descriptor
		CREATE_NEW,                      // how to create
		FILE_FLAG_BACKUP_SEMANTICS,         // file attributes
		NULL                                // file with attributes to copy
	);
	DeleteFile(dir);

}
