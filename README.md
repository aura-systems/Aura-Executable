# Aura Executable
Aura Executable files (.aexe) can be executed on Aura Operating System!

# Build your executable from C
You will need gcc. Then (in the right directory) make:

- gcc -c -std=gnu99 -Os -nostdlib -m32 -ffreestanding -o hello.o hello.c

- ld.exe -o aexe.tmp hello.o aura.ld -Ttext=0x0

- objcopy -O binary aexe.tmp hello.aexe

# Current features
- <b>printf</b> (to print a char[])
- <b>fgets</b> (to read a line and return it in a char[])
- <b>clear</b> (to clear the Aura shell)
- <b>setcursor</b> (to set the position of the cursor)
- <b>resetcursor</b> (if you want to use setcursor you MUST add this method after each printf...)
- <b>memcpy</b> (The memcpy function is used to copy a block of data from a source address to a destination address.)
- <b>memset</b> (memset is used to fill a block of memory with a particular value.)
- <b>memcmp</b> (Compares the first num bytes of the block of memory pointed by ptr1 to the first num bytes pointed by ptr2, returning zero if they all match or a value different from zero representing which is greater if they do not.)
- <b>memchr</b> (Searches within the first num bytes of the block of memory pointed by ptr for the first occurrence of value (interpreted as an unsigned char), and returns a pointer to it.)
- <b>strcat</b> (Appends a copy of the source string to the destination string. The terminating null character in destination is overwritten by the first character of source, and a null-character is included at the end of the new string formed by the concatenation of both in destination.)

# How is it made?
We were inspired by the MSDOS COM files. Indeed, each function uses an interrupt to call the Aura's system calls. 

To communicate between Aura Operating System and the compiled C file, the variables are saved into registers (EDI, ESI). 

The EAX register is how Aura knows (after an Aura System Call: "int 0x48") what function is called. For example, if EAX is 0x01 it's the print function, if it's 0x02 it's to clear the screen etc.
