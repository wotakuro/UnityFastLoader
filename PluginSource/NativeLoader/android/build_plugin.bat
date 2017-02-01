echo ""
echo "Compiling NativeCode.c..."
%ANDROID_NDK_ROOT%/ndk-build NDK_PROJECT_PATH=. NDK_APPLICATION_MK=Application.mk

pause

;;mv libs/armeabi/libnative.so ..

