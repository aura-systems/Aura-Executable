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
- clear (to clear the Aura shell)
- setcursor (to set the position of the cursor)
- resetcursor (if you want to use setcursor you MUST add this method after each printf...)

# How is it made?
We were inspired by the MSDOS COM files. Indeed, each function uses an interrupt to call the Aura's system calls. To communicate between Aura Operating System and the compiled C file, the variables are saved into registers (EDI, ESI). The EAX register is how Aura knows (after an Aura System Call: "int 0x48") what function is called. For example, if EAX is 0x01 it's the print function, if it's 0x02 it's to clear the screen etc.
