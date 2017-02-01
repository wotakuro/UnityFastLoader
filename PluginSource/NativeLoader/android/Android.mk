include $(CLEAR_VARS)

LOCAL_PATH := $(call my-dir)

# override strip command to strip all symbols from output library; no need to ship with those..
# cmd-strip = $(TOOLCHAIN_PREFIX)strip $1 

LOCAL_ARM_MODE  := arm
LOCAL_PATH      := $(NDK_PROJECT_PATH)
LOCAL_MODULE    := nativeloader
LOCAL_CFLAGS    := -Werror
LOCAL_SRC_FILES := ../src/3rd_party/lz4/lz4.c

ifeq ($(HOST_OS),windows)
    LOCAL_SRC_FILES += $(shell dir "$(LOCAL_PATH)/../src/*.cpp" /b /s /a-d) 
else
    LOCAL_SRC_FILES += $(shell find $(LOCAL_PATH)/../src/ -name *.cpp) 
endif

LOCAL_LDLIBS    := -llog

include $(BUILD_SHARED_LIBRARY)
