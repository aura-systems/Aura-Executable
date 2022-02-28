.PHONY: build run clean debug

ILC=/mnt/c/Users/valentinbreiz/.nuget/packages/runtime.win-x64.microsoft.dotnet.ilcompiler/7.0.0-alpha.1.21430.2/tools/ilc.exe
DOTNET = dotnet
CC = gcc

CFLAGS = -std=gnu99 -Os -nostdlib -m32 -ffreestanding -fno-PIC
LDFLAGS = -m i386pe --nmagic -no-PIE --entry Main

OBJ_DIR=obj

C_SOURCES = $(wildcard *.c)
OS_SOURCES = $(wildcard *.cs)

OS_DLL=bin/Debug/net6.0/noruntime.dll
OS_OBJ=obj/noruntime.o
EXECUTABLE=bin/noruntime.exe

_OBJ = $(C_SOURCES:.c=.o)
OBJ = $(patsubst %,$(OBJ_DIR)/%,$(_OBJ))

build: $(EXECUTABLE)
	@echo Done building executable!;

$(EXECUTABLE): $(OS_OBJ) $(OBJ)
	@echo Linking $(EXECUTABLE); \
	ld $(LDFLAGS) --output=$(EXECUTABLE) $(OBJ_DIR)/Lib.o $(OS_OBJ)

$(OS_OBJ): $(OS_DLL)
	@echo Converting CIL to native code; \
	$(ILC) --targetarch x86 --systemmodule noruntime --out $(OS_OBJ) $(OS_DLL)

$(OBJ_DIR): $(OBJ_DIR)
	@mkdir $(OBJ_DIR)

$(OBJ_DIR)/%.o: $(C_SOURCES)
	@echo Assembling $@; \
	$(CC) -c -o $@ $< $(CFLAGS)

$(OS_DLL): $(OS_SOURCES)
	@echo Building executable dlls; \
	$(DOTNET) restore && \
	$(DOTNET) build

clean:
	rm -rf *.log
	rm -rf *.map
	rm -rf obj/
	rm -rf bin/
	rm -rf *.o

re: clean build

print-%  : ; @echo $* = $($*)