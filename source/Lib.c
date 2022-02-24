#include "Lib.h"

int printf(const char *string, ...)
{
	int len = strlen(string);

	asm volatile (
				"mov   %1, %%edx\n"
				"mov   %0, %%ecx\n"
				"mov   $0x1, %%ebx\n"
				"mov   $0x4, %%eax\n"
				"int   $0x48\n"
				"mov   $0x1, %%eax\n"
				"int   $0x48\n"
				: "=r"(len)
				: "r"(string)
				: "edx", "ebx", "eax" , "ecx", "memory"
				);
	return (0);
}

int strlen(char const *str)
{
    int counter = 0;

    while (str[counter] != 0) {
        counter++;
    }
    return counter;
}