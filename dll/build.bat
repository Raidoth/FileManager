g++ -c -w -DBUILD_MY_DLL lib_app.cpp
g++ -shared  -o lib_app.dll lib_app.o -Wl,--out-implib,liblib_app.a 