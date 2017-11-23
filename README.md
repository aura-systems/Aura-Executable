# Aura Executable
Aura Executable files (.aexe) can be executed on Aura Operating System!

# Build your executable from C
You will need gcc. Then (in the right directory) make:

- gcc -c -std=gnu99 -Os -nostdlib -m32 -ffreestanding -o hello.o hello.c

- ld.exe -o aexe.tmp hello.o aura.ld -Ttext=0x0

- objcopy -O binary aexe.tmp hello.aexe

# Current features
- printf (to print a char[])
- fgets (to read a line and return it in a char[])
- clear (to clear Aura shell)
- setcursor (to set the position of the cursor)
- resetcursor (if you want to use setcursor you MUST add this method after each printf...)
